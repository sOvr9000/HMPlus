using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HMPlus.Lib;
using System;

namespace HMPlus.NPCs.Mechoraum {
	[AutoloadBossHead]
	public class Mechoraum : ModNPC {
		private Player player;
		private float lifeMaxInv;

		private float angularVelocity;
		private float maxFlySpeed;
		private float chargeSpeed;
		private float acceleration;
		private float deceleration;
		private float timeDuringCharge;
		private bool isShootingAtPlayer;
		private int lasersFired;

		private int maxSummonTimer = 100;

		private int phase;
		private int hp75;
		private int hp50;
		private int hp25;

		private int gharza;
		private int orzithel;
		private int dijkstra;

		public override void SetStaticDefaults () {
			DisplayName.SetDefault ("Mechoraum");
			Main.npcFrameCount [npc.type] = 2;
		}

		public override void SetDefaults () {
			npc.aiStyle = -1; // My own AI!
			npc.lifeMax = 60000;
			npc.damage = 50;
			npc.defense = 28;
			npc.knockBackResist = 0f;
			npc.width = 172;
			npc.height = 172;
			npc.value = 200000;
			npc.npcSlots = 1;
			npc.boss = true;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit4; // metal
			npc.DeathSound = SoundID.NPCDeath14; // mechanical explosion
			music = MusicID.Boss3;
			//music = mod.GetSoundSlot (SoundType.Music, "Sounds/Music/SteelFreak");
			lifeMaxInv = 1f / npc.lifeMax;
			
			angularVelocity = 0.159f; // more than 3 revolutions per second
			deceleration = 0.97f;
			npc.ai [0] = 0; // mini spawner timer
			npc.ai [1] = 0; // number of minis
			npc.ai [2] = 30f; // charge timer
			isShootingAtPlayer = false;

			phase = 1;
			hp75 = (int) (npc.lifeMax * 0.75f);
			hp50 = (int) (npc.lifeMax * 0.50f);
			hp25 = (int) (npc.lifeMax * 0.25f);
		}

		public override void ScaleExpertStats (int numPlayers, float bossLifeScale) {
			npc.lifeMax = (int) (npc.lifeMax * 0.5f * bossLifeScale);
			npc.damage = (int) (npc.damage * 1.4f);
			lifeMaxInv = 1f / npc.lifeMax;

			hp75 = (int) (npc.lifeMax * 0.75f);
			hp50 = (int) (npc.lifeMax * 0.50f);
			hp25 = (int) (npc.lifeMax * 0.25f);
		}

		public override void NPCLoot () {
			KillMinions ();

			if (Main.expertMode) {
				HMPlusLib.NPCDropModItem (mod, npc, "MechoraumTreasureBag", 1);
			} else {
				player.QuickSpawnItem (ItemID.HallowedBar, Main.rand.Next (15, 30));
				HMPlusLib.NPCDropModItem (mod, npc, "EvilSawtooth", 8);
				HMPlusLib.NPCDropModItem (mod, npc, "MechanicalEyeSocket", 1);
			}

			HMPlusWorld.downedMechoraum = true;
		}

		public override bool? DrawHealthBar (byte hbPosition, ref float scale, ref Vector2 position) {
			scale = 1.5f;
			return null;
		}

		public override void AI () {
			Target ();

			int oldPhase = phase;
			if (npc.life <= hp25) {
				phase = 4;
				npc.frame.Y = 172;
			} else {
				if (npc.life <= hp50) {
					phase = 3;
				} else {
					if (npc.life <= hp75) {
						phase = 2;
					} else {
						phase = 1;
						npc.frame.Y = 0;
					}
				}
			}
			if (oldPhase != phase) {
				PhaseChanged (phase, oldPhase);
			}
			
			UpdateRotation ();

			if (player.dead == true || player.active == false) {
				npc.velocity -= new Vector2 (0f, 0.3f);
				return;
			}

			SeparateAfterlives ();
			AdjustBehavior ();
			FlyTowardNextLocation ();
			ChargeTowardPlayer ();
			SpawnMinis ();
			TryShootAtPlayer ();
		}

		public override void OnHitPlayer (Player target, int damage, bool crit) {
			base.OnHitPlayer (target, damage, crit);
			if (target.statLife <= 0) {
				Target ();
			}
		}

		void PhaseChanged (int phase, int oldPhase) {
			switch (phase) {
				case 1:
					break;
				case 2:
					// summon Gharza
					gharza = NPC.NewNPC ((int) npc.position.X, (int) npc.position.Y, mod.NPCType ("GODProjection"), ai0: 0f);
					Main.npc [gharza].velocity = npc.velocity * 0.6f;
					break;
				case 3:
					// summon Orzithel
					orzithel = NPC.NewNPC ((int) npc.position.X, (int) npc.position.Y, mod.NPCType ("GODProjection"), ai0: 1f);
					Main.npc [orzithel].velocity = npc.velocity * 0.6f;
					break;
				case 4:
					// summon Dijkstra
					dijkstra = NPC.NewNPC ((int) npc.position.X, (int) npc.position.Y, mod.NPCType ("GODProjection"), ai0: 2f);
					Main.npc [dijkstra].velocity = npc.velocity * 0.6f;
					break;
			}
		}

		void KillMinions () {
			Main.npc [gharza].active = false;
			Main.npc [orzithel].active = false;
			Main.npc [dijkstra].active = false;
			for (int k = 0; k < 200; k ++) {
				if (Main.npc [k].type == mod.NPCType ("Miniraum")) {
					Main.npc [k].active = false;
				}
			}
		}

		void Target () {
			npc.TargetClosest (false);
			player = Main.player [npc.target];
			if (player.active == true && player.statLife > 0 && player.dead == false) return;
			//npc.active = false;
			KillMinions ();
		}

		void SeparateAfterlives () {
			if (phase == 3) {
				Vector2 delta = Main.npc [orzithel].Center - Main.npc [gharza].Center;
				if (delta.LengthSquared () < 10000) {
					Vector2 norm = delta.SafeNormalize (Vector2.UnitX) * 0.3f;
					Main.npc [gharza].velocity -= norm;
					Main.npc [orzithel].velocity += norm;
				}
			} else if (phase == 4) {
				NPC [] afterlives = new NPC [] { Main.npc [gharza], Main.npc [orzithel], Main.npc [dijkstra] };
				for (int a = 0; a < 3; a ++) {
					NPC al1 = afterlives [a];
					NPC al2 = afterlives [(a + 1) % 3];
					Vector2 delta = al2.Center - al1.Center;
					if (delta.LengthSquared () < 10000) {
						Vector2 norm = delta.SafeNormalize (Vector2.UnitX) * 0.5f;
						al1.velocity -= norm;
						al2.velocity += norm;
					}
				}
			}
		}

		void FlyTowardNextLocation () {
			if (npc.velocity.LengthSquared () >= maxFlySpeed) {
				npc.velocity *= deceleration;
			} else {
				npc.velocity += (player.Center - npc.Center).SafeNormalize (Vector2.Zero) * acceleration;
			}
		}

		void ChargeTowardPlayer () {
			npc.ai [2] -= 1f;
			if (npc.ai [2] > 0) return;
			npc.ai [2] = timeDuringCharge;
			npc.velocity = (player.Center - npc.Center).SafeNormalize (Vector2.Zero) * chargeSpeed;
		}

		void AdjustBehavior () {
			float progress = 1f - npc.life * lifeMaxInv;
			deceleration = 0.99f - progress * 0.025f;
			timeDuringCharge = 140 - (int) (progress * 50f); // go from 140 to 90
			chargeSpeed = 25f + progress * 20f;
			acceleration = 0.35f + progress * 0.08f;
			maxFlySpeed = 100f + progress * 20f;
			npc.defense = 28 - (int) (progress * 20f); // go from 28 to 8
			angularVelocity = 0.159f + progress * 0.04f; // just spin faster!
		}

		void UpdateRotation () {
			npc.rotation += angularVelocity;
		}

		void SpawnMinis () {
			if (phase == 1) return;
			npc.ai [0] -= 1f;
			if (npc.ai [0] > 0) return;
			npc.ai [0] = 79f + npc.life * 55f * lifeMaxInv; // go from (79 + 1) to (79 + 1 - 40) --> 80 to 25
			if (Main.rand.Next (4) != 0) return;
			if (npc.ai [1] >= 6) return; // max number of minis
			npc.ai [1] ++;
			NPC.NewNPC ((int) npc.Center.X, (int) npc.Center.Y, mod.NPCType ("Miniraum"));
			Main.PlaySound (2, (int) npc.Center.X, (int) npc.Center.Y, 8); // 11 = bullet noise
		}

		void TryShootAtPlayer () {
			if (isShootingAtPlayer == true) {
				DoShootAtPlayer ();
				return;
			}
			if (phase > 1) return;
			if (++ npc.ai [3] % 50 != 0) return;
			if (Main.rand.Next (3) != 0) return;
			isShootingAtPlayer = true;
			lasersFired = 0;
        }

		void DoShootAtPlayer () {
			if (Main.rand.Next (3) != 0) return;
			if (lasersFired >= 5) {
				isShootingAtPlayer = false;
			} else {
				Vector2 vel = (player.Center + Main.rand.NextVector2Square (-200f, 200f) - npc.Center).SafeNormalize (Vector2.UnitX) * 15f;
				Projectile.NewProjectile (npc.Center, vel, ProjectileID.DeathLaser, 20, 0, Main.myPlayer, 0f, 0f);
				lasersFired++;
			}
		}
	}
}