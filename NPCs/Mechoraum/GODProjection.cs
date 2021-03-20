using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HMPlus.Lib;

namespace HMPlus.NPCs.Mechoraum {
	public class GODProjection : ModNPC {
		private Player player;

		static ColorCycle colorCycleG = new ColorCycle (new Color [] { Color.Red, Color.DarkOrange, Color.LightYellow, Color.Black, Color.LightCoral });
		static ColorCycle colorCycleO = new ColorCycle (new Color [] { Color.Green, Color.Azure, Color.GreenYellow, Color.Black, Color.LightGreen });
		static ColorCycle colorCycleD = new ColorCycle (new Color [] { Color.Blue, Color.White, Color.SteelBlue, Color.Black, Color.LightSteelBlue });
		ColorCycle colorCycle = colorCycleG;

		private bool defined = false;
		private float acceleration;

		public override void SetStaticDefaults () {
			Main.npcFrameCount [npc.type] = 12;
		}

		public override void SetDefaults () {
			npc.aiStyle = -1; // My own AI!
			npc.lifeMax = 999999;
			npc.damage = 100;
			npc.defense = 999999;
			npc.knockBackResist = 0f;
			npc.width = 172;
			npc.height = 172;
			npc.scale = 1.5f;
			npc.value = 0;
			npc.npcSlots = 1;
			npc.boss = false;
			npc.immortal = true;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit4; // metal
			npc.DeathSound = SoundID.NPCDeath4; // mechanical explosion

			npc.alpha = 255;
		}

		public override void ScaleExpertStats (int numPlayers, float bossLifeScale) {
			npc.damage = (int) (npc.damage * 1.2f);
		}

		public override void AI () {
			DefineType ();
			Target ();

			AnimateSprite ();
			FadeIn ();
			CreateDust ();
			TryMechSpecialtyAttack ();
			UpdateVelocity ();
			UpdateRotation ();
		}

		public override bool? CanBeHitByItem (Player player, Item item) {
			return false;
		}

		public override bool? CanBeHitByProjectile (Projectile projectile) {
			return false;
		}

		void DefineType () {
			if (defined == true) return;
			defined = true;
			npc.GivenName = npc.ai [0] == 0f ? "Gharza, The Destroyer Afterlife" : npc.ai [0] == 1f ? "Orzithel, The Twins Afterlife" : "Dijkstra, The Skeletron Afterlife";
			colorCycle = npc.ai [0] == 0f ? colorCycleG : npc.ai [0] == 1f ? colorCycleO : colorCycleD;
		}

		void Target () {
			npc.TargetClosest (false);
			player = Main.player [npc.target];
		}

		void AnimateSprite () {
			npc.frameCounter += 1f;
			npc.ai [1] += 1f;
			int offset = npc.ai [0] == 0f ? 0 : npc.ai [0] == 1f ? 688 : 1376;
			switch ((int) npc.ai [1]) {
				case 0:
					npc.frame.Y = offset;
					break;
				case 1:
					npc.frame.Y = offset + 172;
					break;
				case 2:
					npc.frame.Y = offset + 344;
					break;
				case 3:
					npc.frame.Y = offset + 516;
					break;
				case 4:
					goto case 2;
				case 5:
					goto case 1;
				case 6:
					npc.ai [1] = 0f;
					goto case 0;
				default:
					goto case 6;
			}
		}

		void FadeIn () {
			if (npc.alpha > 80) {
				npc.alpha -= 2;
				if (npc.alpha < 80)
					npc.alpha = 80;
			}
		}

		void CreateDust () {
			npc.ai [2] += 1f;
			if (npc.ai [2] >= 3f) {
				npc.ai [2] = 0f;
				Dust.NewDust (npc.Center + Main.rand.NextVector2Circular (129f, 129f), 10, 10, mod.DustType ("GhostDust"), 0f, 0f, 0, colorCycle.Next (), 1.6f);
			}
		}

		void TryMechSpecialtyAttack () {
			npc.ai [3] += 1f;
			if (npc.ai [3] >= 0f) {
				DoMechSpecialtyAttack ();
				return; // return so that an attack can never be commenced while one is already in action
			}
		}

		void DoMechSpecialtyAttack () {
			if (npc.ai [0] == 0f) {
				// Gharza
				// launch Destroyer probes
				if (npc.ai [3] <= 30f) { // summon 10 probes
					if (npc.ai [3] % 3 == 0) {
						int probe = NPC.NewNPC ((int) npc.Center.X, (int) npc.Center.Y, NPCID.Probe /*mod.NPCType ("AfterlifeProbe")*/);
						Main.npc [probe].velocity = Main.rand.NextVector2Unit () * 11f;
						Main.npc [probe].lifeMax = 1500;
						Main.npc [probe].life = 1500;
					}
				} else {
					npc.ai [3] = -480f; // 8s cooldown
				}
			} else if (npc.ai [0] == 1f) {
				// Orzithel
				// cast a barrage of cursed flames
				if (npc.ai [3] <= 10f) {
					if (npc.ai [3] % 2 == 0) {
						double angle = Math.Atan2 (player.Center.Y + 20f * player.velocity.Y - npc.Center.Y, player.Center.X + 20f * player.velocity.X - npc.Center.X) + (npc.ai [3] - 5f) * 0.06981d;
						Projectile.NewProjectileDirect (npc.Center, new Vector2 ((float) Math.Cos (angle) * 17f, (float) Math.Sin (angle) * 17f), ProjectileID.CursedFlameHostile, 60, 5f);
					}
				} else {
					npc.ai [3] = -120f; // 2s cooldown
				}
			} else {
				// Dijkstra
				// launch Skeletron Prime bombs
				const float vel = 4.5f;
				Projectile.NewProjectile (npc.Center, new Vector2 (vel, -10f), ProjectileID.Skull, 70, 6.5f);
				Projectile.NewProjectile (npc.Center, new Vector2 (vel, 10f), ProjectileID.Skull, 70, 6.5f);
				Projectile.NewProjectile (npc.Center, new Vector2 (0f, vel), ProjectileID.Skull, 70, 6.5f);
				Projectile.NewProjectile (npc.Center, new Vector2 (-vel, -10f), ProjectileID.Skull, 70, 6.5f);
				Projectile.NewProjectile (npc.Center, new Vector2 (-vel, 10f), ProjectileID.Skull, 70, 6.5f);
				Projectile.NewProjectile (npc.Center, new Vector2 (0f, -vel), ProjectileID.Skull, 70, 6.5f);
				npc.ai [3] = -130f; // 2.17s cooldown
			}
		}

		void UpdateVelocity () {
			Vector2 delta = player.Center - npc.Center;
			float sqrDist = delta.LengthSquared ();
			if (sqrDist > 1e6f) {
				acceleration = 0.11f + (npc.ai [0] + 1f) * 0.01f * (float) Math.Sqrt (sqrDist - 1e6f);
			} else {
				acceleration = 0.11f;
            }
            if (npc.velocity.LengthSquared () > 40f) {
				npc.velocity *= 0.975f;
			} else {
				npc.velocity += (player.Center - npc.Center).SafeNormalize (Vector2.Zero) * acceleration;
			}
		}

		void UpdateRotation () {
			npc.rotation -= 0.07f;
		}
	}
}