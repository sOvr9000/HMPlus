using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using HMPlus.Lib;

namespace HMPlus.Projectiles {
	public class DustZap : ModProjectile {
		static ColorCycle colorCycle = new ColorCycle (new Color [] { Color.White, Color.PaleVioletRed, Color.GhostWhite, Color.BlueViolet });
		public Vector2 startPos;
		public Projectile emission;

		bool canDamage = true;

		public override void SetStaticDefaults () {
			
		}

		public override void SetDefaults () {
			projectile.friendly = true;
			projectile.width = 42;
			projectile.height = 42;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 16;
			projectile.penetrate = 2;
			projectile.light = 1f;
			projectile.extraUpdates = 1;
			projectile.ignoreWater = true;
		}

		public override void AI () {
			projectile.Center = Vector2.Lerp (startPos, emission.Center, projectile.ai [0] * 0.0625f); // divide by initial timeLeft
			Dust.NewDust (projectile.Center, 10, 10, mod.DustType ("GhostDust"), 0f, 0f, 0, colorCycle.Next (), 0.6f);
			if (++ projectile.ai [0] >= 16f) {
				projectile.active = false;
			}
		}

		public override bool CanDamage () {
			return canDamage;
		}

		public override void OnHitNPC (NPC target, int damage, float knockback, bool crit) {
			canDamage = false;
			target.immune [projectile.owner] = 3;
		}
	}
}