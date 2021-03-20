using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HMPlus.Lib;

namespace HMPlus.Projectiles {
	public class RevealedLight : ModProjectile {
		public static List<int> targetedNPCs = new List<int> ();
		int dustIteration;

		public static void RemoveTargetedNPC (int npc) {
			if (targetedNPCs.Contains (npc)) {
				targetedNPCs.Remove (npc);
			}
		}

		public static void AddTargetedNPC (int npc) {
			if (targetedNPCs.Contains (npc) == false) {
				targetedNPCs.Add (npc);
			}
		}

		public override void SetStaticDefaults () {
			DisplayName.SetDefault ("The Revealed Light");
		}

		public override void SetDefaults () {
			projectile.friendly = true;
			projectile.width = 141;
			projectile.height = 65;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 60;
			projectile.penetrate = 16;
			projectile.light = 1.0f;
			projectile.extraUpdates = 1;
			projectile.ignoreWater = true;

			dustIteration = 0;
			targetedNPCs.Clear ();
        }

		public override void AI () {
			projectile.ai [0] += 1f;

			if (projectile.ai [0] >= 4f) {
				projectile.ai [0] = 0f;
				for (int i = 0; i < 200; i++) {
					NPC target = Main.npc [i];
					if (HMPlusLib.NPCCannotReceiveDamage (target) || target.friendly) continue;
					if (targetedNPCs.Contains (i)) continue;
					float distance = (target.Center - projectile.Center).LengthSquared ();
					if (distance < 250000f) {
						float critChance = Main.player [projectile.owner].meleeCrit + 0.04f;
						HomingObstructedLight.inverseCritChance = critChance == 0f ? 1000000 : (int) (1f / critChance);
                        Projectile.NewProjectile (projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType ("HomingObstructedLight"), (int) (projectile.damage * 1f), 0, Main.myPlayer, 0f, critChance);
						Main.PlaySound (2, (int) projectile.Center.X, (int) projectile.Center.Y, 10); // 11 = bullet noise
						break; // summon only one projectile, as it will probably be enough for this target!
					}
				}
			}

			projectile.rotation = (float) System.Math.Atan2 (projectile.velocity.Y, projectile.velocity.X);
            projectile.velocity *= 1.040918f; // cool number
			if (++dustIteration >= 6) {
				dustIteration = 0;
				Dust.NewDust (new Vector2 (projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType ("DeathParticle"), projectile.velocity.X, projectile.velocity.Y);
			}
		}

		public override void OnHitNPC (NPC target, int damage, float knockback, bool crit) {
			target.immune [projectile.owner] = 1;
			target.AddBuff (24, 120); // OnFire!
			if (HMPlusLib.NPCIsMechanicalBoss (mod, target) == true) {
				target.AddBuff (169, 120); // Penetrated
				target.AddBuff (189, 120); // Daybroken
			}
		}
	}
}