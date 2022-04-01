using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using FargowiltasSouls.Toggler;
using FargowiltasSouls.Projectiles.Souls;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class CopperEnchant : SoulsItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            DisplayName.SetDefault("Copper Enchantment");
            Tooltip.SetDefault(
@"Attacks have a chance to shock enemies with chain lightning
'Behold'");

            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "铜魔石");
            string tooltip_ch =
@"攻击有几率释放闪电击打敌人
'凝视'";
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, tooltip_ch);
        }

        public override void SafeModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(213, 102, 23);
                }
            }
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = 100000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CopperEffect(player);
        }

        public static void CopperEffect(Player player)
        {
            FargoSoulsPlayer modPlayer = player.GetModPlayer<FargoSoulsPlayer>();
            modPlayer.CopperEnchantActive = true;

            if (modPlayer.CopperProcCD > 0)
                modPlayer.CopperProcCD--;
        }

        public static void CopperProc(FargoSoulsPlayer modPlayer, NPC target)
        {
            if (modPlayer.Player.GetToggleValue("Copper") && modPlayer.CopperProcCD == 0)
            {
                target.AddBuff(BuffID.Electrified, 180);

                int dmg = 20;
                int maxTargets = 1;
                int cdLength = 300;

                if (modPlayer.TerraForce)
                {
                    dmg = 100;
                    maxTargets = 10;
                    cdLength = 200;
                }

                List<int> npcIndexes = new List<int>();
                float closestDist = 500f;
                NPC closestNPC;

                for (int i = 0; i < maxTargets; i++)
                {
                    closestNPC = null;

                    //find closest npc to target that has not been chained to yet
                    for (int j = 0; j < Main.maxNPCs; j++)
                    {
                        NPC npc = Main.npc[j];

                        if (npc.active && npc.whoAmI != target.whoAmI && npc.Distance(target.Center) < closestDist && !npcIndexes.Contains(npc.whoAmI))
                        {
                            closestNPC = npc;
                            closestDist = npc.Distance(target.Center);
                            //break;
                        }
                    }

                    if (closestNPC != null)
                    {
                        npcIndexes.Add(closestNPC.whoAmI);

                        Vector2 ai = closestNPC.Center - target.Center;
                        float ai2 = Main.rand.Next(100);
                        Vector2 velocity = Vector2.Normalize(ai) * 20;

                        Projectile p = FargoSoulsUtil.NewProjectileDirectSafe(modPlayer.Player.GetProjectileSource_Item(modPlayer.Player.HeldItem), target.Center, velocity, ModContent.ProjectileType<CopperLightning>(), FargoSoulsUtil.HighestDamageTypeScaling(modPlayer.Player, dmg), 0f, modPlayer.Player.whoAmI, ai.ToRotation(), ai2);
                    }
                    else
                    {
                        break;
                    }

                    target = closestNPC;
                }

                modPlayer.CopperProcCD = cdLength;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CopperHelmet)
                .AddIngredient(ItemID.CopperChainmail)
                .AddIngredient(ItemID.CopperGreaves)
                .AddIngredient(ItemID.CopperShortsword)
                .AddIngredient(ItemID.WandofSparking)
                .AddIngredient(ItemID.ThunderStaff)
                
            .AddTile(TileID.DemonAltar)
            .Register();
        }
    }
}
