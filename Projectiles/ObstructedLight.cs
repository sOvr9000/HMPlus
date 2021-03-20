using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using HMPlus.Lib;

namespace HMPlus.Projectiles {
	public class ObstructedLight : ModProjectile {
		int dustIteration;

		public override void SetStaticDefaults () {
			DisplayName.SetDefault ("The Obstructed Light");
		}

		public override void SetDefaults () {
			projectile.friendly = true;
			projectile.width = 113;
			projectile.height = 41;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 40;
			projectile.penetrate = 1;
			projectile.light = 1.0f;
			projectile.extraUpdates = 1;
			projectile.ignoreWater = true;

			dustIteration = 0;
        }

		public override void AI () {
			projectile.rotation = (float) System.Math.Atan2 ((double) projectile.velocity.Y, (double) projectile.velocity.X);
			projectile.velocity *= 1.04f;
			if (++dustIteration >= 6) {
				dustIteration = 0;
				Dust.NewDust (new Vector2 (projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType ("DeathParticle"), projectile.velocity.X, projectile.velocity.Y);
			}
		}

		public override void OnHitNPC (NPC target, int damage, float knockback, bool crit) {
			target.AddBuff (24, 120); // OnFire!
			if (HMPlusLib.NPCIsMechanicalBoss (mod, target) == true) {
				target.AddBuff (169, 120); // Penetrated
				target.AddBuff (189, 120); // Daybroken
			}
		}
	}
}