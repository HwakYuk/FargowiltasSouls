﻿using FargowiltasSouls.Toggler;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;
using System;
using Terraria.Localization;
using Terraria.UI.Chat;
using System.Text.RegularExpressions;

namespace FargowiltasSouls.UI
{
    public class SoulToggler : UIState
    {
        public static Regex RemoveItemTags = new Regex(@"\[[^\[\]]*\]");

        public bool NeedsToggleListBuilding;
        public string DisplayMod;
        public string SortCatagory;

        public const int BackWidth = 400;
        public const int BackHeight = 620;

        public UIDragablePanel BackPanel;
        public UIPanel InnerPanel;
        public UIScrollbar Scrollbar;
        public UIList ToggleList;
        public UISearchBar SearchBar;

        public override void OnInitialize()
        {
            NeedsToggleListBuilding = true;
            DisplayMod = "Terraria";
            SortCatagory = "";

            // This entire layout is cancerous and dangerous to your health because red protected UIElements children
            // If I want to give extra non-children to BackPanel to count as children when seeing if it should drag, I have to abandon
            // all semblence of organization in favour of making it work. Enjoy my write only UI laying out.
            // Oh well, at least it works...

            Scrollbar = new UIScrollbar();
            Scrollbar.SetView(100f, 1000f);
            Scrollbar.Width.Set(20, 0f);
            Scrollbar.OverflowHidden = true;
            Scrollbar.OnScrollWheel += hotbarScrollFix;

            ToggleList = new UIList();
            ToggleList.SetScrollbar(Scrollbar);
            ToggleList.OnScrollWheel += hotbarScrollFix;

            BackPanel = new UIDragablePanel(Scrollbar, ToggleList);
            BackPanel.Left.Set(0, 0f);
            BackPanel.Top.Set(0, 0f);
            BackPanel.Width.Set(BackWidth, 0f);
            BackPanel.Height.Set(BackHeight, 0f);
            BackPanel.PaddingLeft = BackPanel.PaddingRight = BackPanel.PaddingTop = BackPanel.PaddingBottom = 0;
            BackPanel.BackgroundColor = new Color(29, 33, 70) * 0.7f;

            InnerPanel = new UIPanel();
            InnerPanel.Width.Set(BackWidth - 12, 0f);
            InnerPanel.Height.Set(BackHeight - 38, 0);
            InnerPanel.Left.Set(6, 0f);
            InnerPanel.Top.Set(32, 0f);
            InnerPanel.BackgroundColor = new Color(73, 94, 171) * 0.9f;

            SearchBar = new UISearchBar(BackWidth - 8, 26);
            SearchBar.Left.Set(4, 0f);
            SearchBar.Top.Set(4, 0f);
            SearchBar.OnTextChange += SearchBar_OnTextChange;

            ToggleList.Width.Set(InnerPanel.Width.Pixels - InnerPanel.PaddingLeft * 2f - Scrollbar.Width.Pixels, 0f);
            ToggleList.Height.Set(InnerPanel.Height.Pixels - InnerPanel.PaddingTop * 2f, 0f);

            Scrollbar.Height.Set(InnerPanel.Height.Pixels - 16, 0f);
            Scrollbar.Left.Set(InnerPanel.Width.Pixels - Scrollbar.Width.Pixels - 18, 0f);

            Append(BackPanel);
            BackPanel.Append(InnerPanel);
            BackPanel.Append(SearchBar);
            InnerPanel.Append(Scrollbar);
            InnerPanel.Append(ToggleList);

            base.OnInitialize();
        }

        private void SearchBar_OnTextChange(string oldText, string currentText)
        {
            NeedsToggleListBuilding = true;
        }

        private void hotbarScrollFix(UIScrollWheelEvent evt, UIElement listeningElement)
        {
            Main.LocalPlayer.ScrollHotbar(PlayerInput.ScrollWheelDelta / 120);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (NeedsToggleListBuilding)
            {
                BuildList();
                NeedsToggleListBuilding = false;
            }
        }

        public void BuildList()
        {
            ToggleList.Clear();
            Player player = Main.LocalPlayer;
            ToggleBackend toggler = player.GetModPlayer<FargoPlayer>().Toggler;

            IEnumerable<Toggle> displayToggles = toggler.Toggles.Values.Where((toggle) =>
            toggle.Mod == DisplayMod &&
            (string.IsNullOrEmpty(SortCatagory) || toggle.Catagory == SortCatagory) &&
            (SearchBar.IsEmpty || GetRawToggleName(toggle.InternalName).StartsWith(SearchBar.Input)));

            foreach (Toggle toggle in displayToggles)
            {
                ToggleList.Add(new UIToggle(toggle.InternalName));
            }
        }

        public string GetRawToggleName(string key)
        {
            string baseText = Language.GetTextValue($"Mods.FargowiltasSouls.{key}Config");
            List<TextSnippet> parsedText = ChatManager.ParseMessage(baseText, Color.White);
            string rawText = "";

            foreach (TextSnippet snippet in parsedText)
            {
                if (!snippet.Text.StartsWith("["))
                {
                    rawText += snippet.Text.Trim();
                }
            }

            return rawText;
        }

        public void SetPositionToPoint(Point point)
        {
            BackPanel.Left.Set(point.X, 0f);
            BackPanel.Top.Set(point.Y, 0f);
        }

        public Point GetPositionAsPoint() => new Point((int)BackPanel.Left.Pixels, (int)BackPanel.Top.Pixels);
    }
}
