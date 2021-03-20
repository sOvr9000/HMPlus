using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using HMPlus.Lib;
using HMPlus.Dusts;

namespace HMPlus.Projectiles {
	public class DarknessEmission : ModProjectile {
		static ColorCycle colorCycle = new ColorCycle (new Color [] { Color.DarkCyan, Color.Silver, Color.Purple });

		List<Dust> dusts = new List<Dust> ();

		int frame = 0;

		public override void SetStaticDefaults () {

		}

		public override void SetDefaults () {
			projectile.friendly = true;
			projectile.width = 12;
			projectile.height = 12;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 65;
			projectile.penetrate = 11;
			projectile.light = 0f;
			projectile.extraUpdates = 1;
			projectile.ignoreWater = true;
			projectile.alpha = 255;
			
			frame = 0;
        }

		public override void AI () {
			projectile.velocity *= 0.985f;

			if (frame++ % 3 == 0) {
				Dust dust = Dust.NewDustDirect (projectile.Center, 10, 10, mod.DustType ("DarknessParticle"), newColor: colorCycle.Next (), Scale: 1.0f);
				dusts.Add (dust);
				for (int n = dusts.Count - 1; n >= 0; n--) {
					Dust d = dusts [n];
					if (d.active == false) {
						dusts.RemoveAt (n);
					} else {
						DustShoot (d);
					}
				}
			}
			if (frame > 40) {
				foreach (Dust d in dusts) {
					d.alpha = 55 + 10 * frame;
					if (d.alpha >= 255) {
						d.active = false;
					}
				}
			}
		}

		void DustShoot (Dust dust) {
			if (dust.active == false || dust.alpha >= 255) return;
			for (int k = 0; k < 200; k ++) {
				NPC npc = Main.npc [k];
				if (npc.friendly == true) continue;
				if (HMPlusLib.NPCCannotReceiveDamage (npc)) continue;
				if ((npc.Center - projectile.Center).LengthSquared () > 68000f) continue;
				if (npc.immune [projectile.owner] > 0) continue;

				projectile.penetrate -= 1;
				DustZap proj = (DustZap) Projectile.NewProjectileDirect (npc.Center, Vector2.Zero, mod.ProjectileType ("DustZap"), projectile.damage, 1.0f, projectile.owner, 0f, 0f).modProjectile;
				proj.startPos = npc.Center;
				proj.emission = projectile;
				npc.AddBuff (BuffID.Electrified, 120);

				dust.alpha = 255;
				dust.active = false;
				dusts.Remove (dust);
				return;
			}
		}
	}
}