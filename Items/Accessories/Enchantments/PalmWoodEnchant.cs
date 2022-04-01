﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using FargowiltasSouls.Toggler;
using FargowiltasSouls.Projectiles.Minions;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class PalmWoodEnchant : SoulsItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            DisplayName.SetDefault("Palm Wood Enchantment");
            Tooltip.SetDefault(
@"Double tap down to spawn a palm tree sentry that throws nuts at enemies
'Alarmingly calm'");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "棕榈木魔石");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, 
@"双击'下'键会召唤一个会向敌人扔橡子的棕榈树哨兵
'出奇的宁静'");
        }

        public override void SafeModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(183, 141, 86);
                }
            }
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.rare = ItemRarityID.Green;
            Item.value = 10000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PalmEffect(player);
        }

        public static void PalmEffect(Player player)
        {
            FargoSoulsPlayer modPlayer = player.GetModPlayer<FargoSoulsPlayer>();
            modPlayer.PalmEnchantActive = true;

            if (player.GetToggleValue("Palm") && player.whoAmI == Main.myPlayer && modPlayer.DoubleTap)
            {
                Vector2 mouse = Main.MouseWorld;

                if (player.ownedProjectileCounts[ModContent.ProjectileType<PalmTreeSentry>()] > 0)
                {
                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        Projectile proj = Main.projectile[i];

                        if (proj.type == ModContent.ProjectileType<PalmTreeSentry>() && proj.owner == player.whoAmI)
                        {
                            proj.Kill();
                        }
                    }
                }

                Projectile.NewProjectile(player.GetProjectileSource_Misc(0), mouse.X, mouse.Y - 10, 0f, 0f, ModContent.ProjectileType<PalmTreeSentry>(), 0, 0f, player.whoAmI);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PalmWoodHelmet)
                .AddIngredient(ItemID.PalmWoodBreastplate)
                .AddIngredient(ItemID.PalmWoodGreaves)
                .AddIngredient(ItemID.BreathingReed)
                .AddIngredient(ItemID.Coconut)
                .AddIngredient(ItemID.Seagull)

            .AddTile(TileID.DemonAltar)
            .Register();
        }
    }
}
