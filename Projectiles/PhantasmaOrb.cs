using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using HMPlus.Lib;

namespace HMPlus.Projectiles {
	public class PhantasmaOrb : ModProjectile {
		private Color color = default (Color);
		private Color [] colorCycle;
		private int colorIndex = 0;

		public override void SetStaticDefaults () {
			DisplayName.SetDefault ("Phantasma Orb");
		}

		public override void SetDefaults () {
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.damage = 25;
			projectile.width = 24;
			projectile.height = 24;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 240;
			projectile.penetrate = 2;
			projectile.light = 1.0f;
			projectile.extraUpdates = 1;
			projectile.ignoreWater = true;

			colorIndex = 0;
			GetColorCycle ();
			color = colorCycle [colorIndex];
        }

		void GetColorCycle () {
			switch ((int) projectile.ai [1]) {
				case 0:
					colorCycle = NPCs.TheIllusion.FigmentOfHorror.colorCycle;
					break;
				case 1:
					colorCycle = NPCs.TheIllusion.FigmentOfDarkness.colorCycle;
					break;
				case 2:
					colorCycle = NPCs.TheIllusion.FigmentOfHell.colorCycle;
					break;
				case 3:
					colorCycle = NPCs.TheIllusion.FigmentOfFear.colorCycle;
					break;
			}
		}

		public override void AI () {
			projectile.rotation = (float) System.Math.Atan2 (projectile.velocity.Y, projectile.velocity.X);
			if (++projectile.ai [0] >= 3) {
				projectile.ai [0] = 0;
				colorIndex = (colorIndex + 1) % colorCycle.Length;
				color = colorCycle [colorIndex];
				Dust.NewDust (projectile.Center, projectile.width, projectile.height, mod.DustType ("GhostDust"), projectile.velocity.X * 0.3f, projectile.velocity.Y * 0.3f, 0, color, 1.2f);
			}
			Lighting.AddLight (projectile.Center, color.R * 0.03f, color.G * 0.03f, color.B * 0.03f);
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
