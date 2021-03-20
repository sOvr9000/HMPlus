using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using HMPlus.Lib;

namespace HMPlus.Projectiles {
	public class GhostSpear : ModProjectile {
		private int alphaIteration = 0;

        public override void SetStaticDefaults () {
			DisplayName.SetDefault ("Transcendental Spear");
		}

		public override void SetDefaults () {
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.width = 56;
			projectile.height = 24;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 240;
			projectile.penetrate = 4;
			projectile.light = 1.0f;
			projectile.extraUpdates = 1;
			projectile.ignoreWater = true;

			alphaIteration = 0;
        }

		public override void AI () {
			projectile.rotation = (float) System.Math.Atan2 (projectile.velocity.Y, projectile.velocity.X);
			alphaIteration += 6;
			projectile.alpha = 255 - alphaIteration;
			projectile.velocity *= 1.01f;
			if (++projectile.ai [0] >= 3) {
				projectile.ai [0] = 0;
				Dust.NewDust (new Vector2 (projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType ("GhostDust"), projectile.velocity.X, projectile.velocity.Y, 0, new Color (0, 190, 255));
			}
		}
	}
}