using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HMPlus.Lib;

namespace HMPlus.Projectiles {
	public class HomingObstructedLight : ModProjectile {
		private List<NPC> hitNPCs;
		private static int criticalStrikeCounter = 0;
		public static int inverseCritChance = 25;
		private int dustIteration;

		private int currentTarget;

		public override void SetStaticDefaults () {
			DisplayName.SetDefault ("The Obstructed Light");
		}

		public override void SetDefaults () {
			projectile.damage = 0;
			projectile.friendly = true;
			projectile.width = 113;
			projectile.height = 41;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 300;
			projectile.penetrate = 42;
			projectile.light = 1.0f;
			projectile.extraUpdates = 1;
			projectile.ignoreWater = true;

			hitNPCs = new List<NPC> ();

			dustIteration = 0;
			currentTarget = -1;
		}

		public override void AI () {
			if (currentTarget == -1) {
				FindNewTarget ();
			}
			if (currentTarget != -1) {
				NPC target = Main.npc [currentTarget];
				target.CheckActive ();
				if (target.active == false || target.life <= 0) {
					SetTarget (null);
				} else {
					projectile.velocity = (target.Center - projectile.Center).SafeNormalize (new Vector2 (1f, 0f)) * 15f;
				}
			}

			projectile.rotation = (float) System.Math.Atan2 (projectile.velocity.Y, projectile.velocity.X);
			if (++dustIteration >= 20) {
				dustIteration = 0;
				Dust.NewDust (new Vector2 (projectile.Center.X, projectile.Center.Y), projectile.width, projectile.height, mod.DustType ("DeathParticle"), projectile.velocity.X, projectile.velocity.Y);
			}
			for (int n = hitNPCs.Count - 1; n >= 0; n --) {
				NPC npc = hitNPCs [n];
				if (npc == null || npc.active == false) {
					hitNPCs.RemoveAt (n);
					continue;
				}
				criticalStrikeCounter += 1;
				bool criticalStrike = false;
				if (criticalStrikeCounter >= inverseCritChance) {
					criticalStrikeCounter -= inverseCritChance;
					criticalStrike = true;
				}
				//npc.StrikeNPC ((int) (projectile.damage * ((float) Main.rand.NextDouble () * 0.5f + 0.75f)), 0f, 0, criticalStrike);
			}
		}

		void FindNewTarget () {
			float lastDist = 1e6f;
			NPC theTarget = null;
			for (int i = 0; i < 200; i++) {
				NPC target = Main.npc [i];
				if (HMPlusLib.NPCCannotReceiveDamage (target) || target.friendly) continue;
				if (RevealedLight.targetedNPCs.Contains (i)) continue;
				//Get the shoot trajectory from the projectile and target
				float distance = (target.Center - projectile.Center).LengthSquared ();
				if (distance >= lastDist) continue;
				lastDist = distance;
				theTarget = target;
				if (distance < 10000f) { // 10 tiles
					break; // the NPC is close enough
				}
			}
			if (theTarget != null) {
				SetTarget (theTarget);
			}
		}

		public override void Kill (int timeLeft) {
			SetTarget (null);
			base.Kill (timeLeft);
		}

		void SetTarget (NPC npc) {
			if (npc == null) {
				RevealedLight.RemoveTargetedNPC (currentTarget);
				currentTarget = -1;
			} else {
				currentTarget = npc.whoAmI;
				RevealedLight.AddTargetedNPC (currentTarget);
			}
		}

		public override void OnHitNPC (NPC target, int damage, float knockback, bool crit) {
			target.immune [projectile.owner] = 2;
			target.AddBuff (BuffID.Burning, 120);
			if (HMPlusLib.NPCIsMechanicalBoss (mod, target) == true) {
				target.AddBuff (169, 120); // Penetrated
				target.AddBuff (189, 120); // Daybroken
			}
			if (target.friendly == true || target.defense > 999) return;
			if (hitNPCs.Contains (target) == false) {
				hitNPCs.Add (target);
			}
		}
	}
}