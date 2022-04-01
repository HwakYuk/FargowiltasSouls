using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class SpiderEnchant : SoulsItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            DisplayName.SetDefault("Spider Enchantment");
            
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "蜘蛛魔石");
            
            string tooltip =
@"Your minions and sentries can now crit with a 15% chance
'Arachnophobia is punishable by arachnid induced death'";
            Tooltip.SetDefault(tooltip);

            string tooltip_ch =
@"你的仆从和哨兵现在可以造成暴击且有15%基础暴击率
'对恐蛛症者可惩罚他们死于蜘蛛之口'";
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, tooltip_ch);

        }

        public override void SafeModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(109, 78, 69);
                }
            }
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = 150000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoSoulsPlayer>().SpiderEffect(hideVisual);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.SpiderMask)
            .AddIngredient(ItemID.SpiderBreastplate)
            .AddIngredient(ItemID.SpiderGreaves)
            .AddIngredient(ItemID.SpiderStaff)
            .AddIngredient(ItemID.QueenSpiderStaff)
            .AddIngredient(ItemID.WebSlinger)
            //web rope coil
            //rainbow string
            //fried egg
            //.AddIngredient(ItemID.SpiderEgg);

            .AddTile(TileID.CrystalBall)
            .Register();
        }
    }
}
