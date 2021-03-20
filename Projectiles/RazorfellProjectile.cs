using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using HMPlus.Lib;

namespace HMPlus.Projectiles {
	public class RazorfellProjectile : ModProjectile {
		private int frame;

		public override void SetStaticDefaults () {
		}

		public override void SetDefaults () {
			projectile.friendly = true;
			projectile.width = 42;
			projectile.height = 42;
			projectile.melee = true;
			projectile.tileCollide = true;
			projectile.timeLeft = 180;
			projectile.penetrate = 4;
			projectile.light = 0f;
			projectile.extraUpdates = 1;
			projectile.ignoreWater = false;

			frame = 0;
        }

		public override void AI () {
			projectile.rotation += 0.03f;
			if (frame ++ >= 32)
				projectile.velocity += new Vector2 (0f, 0.267f); // 16 tiles/second^2, equal to gravity of earth
		}

		public override bool OnTileCollide (Vector2 oldVelocity) {
			if (projectile.velocity.X != oldVelocity.X) {
				projectile.velocity.X = -oldVelocity.X * 0.8f;
			}
			if (projectile.velocity.Y != oldVelocity.Y) {
				projectile.velocity.Y = -oldVelocity.Y * 0.7f;
			}
			Main.PlaySound (SoundID.Item10, projectile.position);
			return false;
		}

		public override void OnHitNPC (NPC target, int damage, float knockback, bool crit) {

		}
	}
}