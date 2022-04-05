using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Champions
{
    public class CosmosReticle : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cosmic Reticle");
        }

        public override void SetDefaults()
        {
            Projectile.width = 110;
            Projectile.height = 110;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            
            //CooldownSlot = 1;
        }

        public override bool? CanDamage()
        {
            return false;
        }

        public override void AI()
        {
            NPC npc = FargoSoulsUtil.NPCExists(Projectile.ai[0], ModContent.NPCType<NPCs.Champions.CosmosChampion>());
            if (npc == null || npc.ai[0] != 11)
            {
                Projectile.Kill();
                return;
            }

            Player player = Main.player[npc.target];

            Projectile.velocity = Vector2.Zero;

            if (++Projectile.ai[1] > 45)
            {
                if (Projectile.ai[1] % 5 == 0)
                {
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item89, Projectile.Center);

                    if (Main.netMode != NetmodeID.MultiplayerClient) //rain meteors
                    {
                        Vector2 spawnPos = Projectile.Center;
                        spawnPos.X += Main.rand.Next(-200, 201);
                        spawnPos.Y -= 700;
                        Vector2 vel = Main.rand.NextFloat(10, 15f) * Vector2.Normalize(Projectile.Center - spawnPos);
                        Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), spawnPos, vel, ModContent.ProjectileType<CosmosMeteor>(), FargoSoulsUtil.ScaledProjectileDamage(npc.damage), 0f, Main.myPlayer, 0f, Main.rand.NextFloat(1f, 1.5f));
                    }
                }

                if (Projectile.ai[1] > 90)
                {
                    Projectile.ai[1] = 0;
                    Projectile.netUpdate = true;
                }

                Projectile.rotation = 0;
                Projectile.alpha = 0;
                Projectile.scale = 1;
            }
            else
            {
                Projectile.Center = player.Center;
                Projectile.position.X += player.velocity.X * 30;

                if (Projectile.ai[1] == 45)
                {
                    Projectile.netUpdate = true;

                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<GlowRing>(), 0, 0f, Main.myPlayer, -1, -5);
                }

                float spindown = 1f - Projectile.ai[1] / 45f;
                Projectile.rotation = MathHelper.TwoPi * 1.5f * spindown;
                Projectile.alpha = (int)(255 * spindown);
                Projectile.scale = 1 + 2 * spindown;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 128) * (1f - Projectile.alpha / 255f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Projectile.GetAlpha(lightColor), Projectile.rotation, origin2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}