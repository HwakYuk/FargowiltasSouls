﻿//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;
//using Terraria.Localization;

//namespace FargowiltasSouls.Items.Accessories.Enchantments
//{
//    public class PinkEskimoEnchant : SoulsItem
//    {
//        public override bool Autoload(ref string name)
//        {
//            return false;
//        }

//        public override void SetStaticDefaults()
//        {
//            DisplayName.SetDefault("Pink Eskimo Enchantment");
//            Tooltip.SetDefault(
//@"''");
//            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "粉爱斯基摩魔石");
//            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese,
//@"''");
//        }

//        public override void SetDefaults()
//        {
//            item.width = 20;
//            item.height = 20;
//            item.accessory = true;
//            ItemID.Sets.ItemNoGravity[item.type] = true;
//            item.rare = ItemRarityID.Lime;
//            item.value = 100000;
//        }

//        public override void UpdateAccessory(Player player, bool hideVisual)
//        {
//            /*
//             * if(player.walkingOnWater)
//{
//	Create Ice Rod Projectile right below you
//}

//NearbyEffects:

//if(modPlayer.EskimoEnchant && tile.type == IceRodBlock)
//{
//	Create spikes
//}
//             */
//        }

//        public override void AddRecipes()
//        {
//            CreateRecipe()

//            recipe.AddIngredient(ItemID.PinkEskimoHood);
//            recipe.AddIngredient(ItemID.PinkEskimoCoat);
//            recipe.AddIngredient(ItemID.PinkEskimoPants);
//            //recipe.AddIngredient(ItemID.IceRod);
//            recipe.AddIngredient(ItemID.FrostMinnow);
//            recipe.AddIngredient(ItemID.AtlanticCod);
//            recipe.AddIngredient(ItemID.MarshmallowonaStick);

//            //grinch pet? or steal pets from frost??!

//            recipe.AddTile(TileID.CrystalBall);
//            recipe.SetResult(this);
//            recipe.AddRecipe();
//        }
//    }
//}
