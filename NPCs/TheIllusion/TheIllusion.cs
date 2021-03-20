using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using HMPlus.Lib;

namespace HMPlus.NPCs.TheIllusion {
	[AutoloadBossHead]
	public class TheIllusion : ModNPC {
        public const float figmentDistance = 375f;
		private Player player;

		private bool hasTeleported;
		private bool hasSummonedMinions = false;

		private int frameY = 0;
		private bool fullTexture = false;

		private int swipeCharge = 0;

		public override void SetStaticDefaults () {
			DisplayName.SetDefault ("The Illusion");
			Main.npcFrameCount [npc.type] = 16;
		}

		public override void SetDefaults () {
			npc.aiStyle = -1; // My own AI!
			npc.lifeMax = 3000;
			npc.damage = 35;
			npc.defense = 0;
			npc.knockBackResist = 0f;
			npc.width = 80;
			npc.height = 80;
			npc.scale = 2.0f;
			npc.value = 400000;
			npc.npcSlots = 1;
			npc.boss = true;
			npc.lavaImmune = true;
			npc.immortal = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit3; // fire ball collision
			npc.DeathSound = SoundID.NPCDeath6; // ethereal gasp
			music = MusicID.Boss5;
			//music = mod.GetSoundSlot (SoundType.Music, "Sounds/Music/ItsNotReal");

			npc.ai [0] = 100;
			npc.ai [1] = 1; // teleport stage
			hasTeleported = false;
			hasSummonedMinions = false;
			swipeCharge = 0;

			frameY = 0;
			fullTexture = false;
		}

		public override void ScaleExpertStats (int numPlayers, float bossLifeScale) {
			// life is already multiplied by two automatically in Expert mode.. I guess? Maybe it's Revengeance mode?
			npc.lifeMax = (int) (npc.lifeMax * 1.2f);
		}

		public override void NPCLoot () {
			if (Main.expertMode) {
				HMPlusLib.NPCDropModItem (mod, npc, "TheIllusionTreasureBag", 1);
			} else {
				HMPlusLib.NPCDropItem (mod, npc, ItemID.SpectreBar, 5);
			}

			// summon 9 dungeon spirits with increased defense
			for (int x = -40; x <= 40; x+= 40) {
				for (int y = -40; y <= 40; y+= 40) {
					Main.npc [NPC.NewNPC ((int) npc.Center.X + x, (int) npc.Center.Y + x, NPCID.DungeonSpirit)].defense += 100;
				}
			}

			HMPlusWorld.downedTheIllusion = true;
		}

		public override bool? DrawHealthBar (byte hbPosition, ref float scale, ref Vector2 position) {
			scale = 1.6f;
			return null;
		}

		public override void FindFrame (int frameSize) {
			npc.frameCounter += 1f;
			if (Main.rand.Next (2) == 0) {
				frameY += 1;
			} else {
				frameY -= 1;
			}
			if (fullTexture && Main.rand.Next (6) == 0) {
				frameY += 4;
			}
			if (frameY >= 8) frameY -= 8;
			if (frameY < 0) frameY += 8;
			if (Main.rand.Next (10) == 0) { // chance to thin or thicken texture
				fullTexture = !fullTexture;
			}
			npc.frame.Y = (fullTexture ? frameY + 8 : frameY) * frameSize;
		}

		public override bool? CanBeHitByItem (Player player, Item item) {
			return npc.ai [2] <= 0f;
		}

		public override bool? CanBeHitByProjectile (Projectile projectile) {
			return npc.ai [2] <= 0f;
		}

		public override void ModifyHitByItem (Player player, Item item, ref int damage, ref float knockback, ref bool crit) {
			if (player.name == "modding") damage *= 10;
		}

		public override void AI () {
			Target ();
			UpdateRotation ();

			float lightEmission = 1.0f - npc.alpha * 0.0039215f;
			Vector3 rgb = new Vector3 (0.67f * lightEmission, 1.33f * lightEmission, 2.0f * lightEmission);
			Lighting.AddLight (npc.Center + new Vector2 (30f, 30f), rgb);
			Lighting.AddLight (npc.Center + new Vector2 (-30f, 30f), rgb);
			Lighting.AddLight (npc.Center + new Vector2 (-30f, -30f), rgb);
			Lighting.AddLight (npc.Center + new Vector2 (30f, -30f), rgb);

			TrySummonMinions ();
			TrySpearSwipe ();

			//npc.ai [3] += 1f;
			//if (npc.ai [3] >= timeToKill) {
			//	npc.life = -10;
			//	player.AddBuff (mod.BuffType ("Overwhelmed"), 3600);
			//}

			if (player.ZoneDungeon == false) {
				Target ();
				if (player.ZoneDungeon == false) {
					npc.life = -10;
					for (int k = 199; k >= 0; k --) {
						if (Main.npc [k].TypeName.Contains ("Figment") == true) {
							Main.npc [k].life = -10;
						}
					}
				}
			}

			npc.ai [0] -= 1;
			switch ((int) npc.ai [1]) {
				case 1:
					FloatingStage ();
					break;
				case 2:
					TeleportStage ();
					break;
				default:
					goto case 1;
			}
		}

		void Target () {
			npc.TargetClosest (false);
			player = Main.player [npc.target];
		}

		void SetRandomVelocity () {
			Vector2 direction = (player.Center - npc.Center).SafeNormalize (new Vector2 (1f, 0f));
			if (Main.rand.Next (2) == 0) {
				npc.velocity = new Vector2 (direction.Y, -direction.X); // rotate clockwise
			} else {
				npc.velocity = new Vector2 (-direction.Y, direction.X); // rotate counter-clockwise
			}
		}

		void ChangeToFloatingStage () {
			npc.ai [1] = 1;
			npc.ai [0] = 100;
			hasTeleported = false;
			npc.alpha = 0;
		}
		
		void FloatingStage () {
			if (npc.ai [0] <= 0) {
				ChangeToTeleportStage ();
			}
		}

		void ChangeToTeleportStage () {
			npc.ai [1] = 2;
			npc.ai [0] = 40;
		}

		void TeleportStage () {
			if (hasTeleported) {
				npc.alpha = (int) (6.375f * npc.ai [0]); // go from 255 to 0
				if (npc.ai [0] <= 0) {
					ChangeToFloatingStage ();
				}
			} else {
				npc.alpha = (int) (6.375f * (40 - npc.ai [0])); // go from 0 to 255
				if (npc.ai [0] <= 0) {
					npc.Center = player.Center + Main.rand.NextVector2CircularEdge (400f, 400f);
					npc.ai [0] = 40;
					hasTeleported = true;
					SetRandomVelocity ();
				}
			}
		}

		void TrySummonMinions () {
			if (hasSummonedMinions == true) return;
			NPC.NewNPC ((int) npc.Center.X, (int) npc.Center.Y, mod.NPCType ("FigmentOfHorror"), 0, 0, 0, 0, 0);
			NPC.NewNPC ((int) npc.Center.X, (int) npc.Center.Y, mod.NPCType ("FigmentOfHell"), 0, 0, MathHelper.PiOver2, 0, 0);
			NPC.NewNPC ((int) npc.Center.X, (int) npc.Center.Y, mod.NPCType ("FigmentOfDarkness"), 0, 0, MathHelper.Pi, 0, 0);
			NPC.NewNPC ((int) npc.Center.X, (int) npc.Center.Y, mod.NPCType ("FigmentOfFear"), 0, 0, MathHelper.PiOver2 * 3f, 0, 0);
			npc.ai [2] = 4;
			hasSummonedMinions = true;
		}

		void TrySpearSwipe () {
			if (++swipeCharge < 90) return;
			swipeCharge = 0;
			if (Main.rand.Next (3) != 0) return;
			Vector2 direction = Main.rand.NextVector2CircularEdge (1f, 1f);
			Vector2 origin = player.Center - direction * 1000f;
            Vector2 orthogonal = new Vector2 (direction.Y, -direction.X);
			for (int n = 24; n <= 500; n += n - 1) {
				if (Main.rand.Next (2) == 0) Projectile.NewProjectile (origin + orthogonal * n, direction * 3.2f + new Vector2 ((float) Main.rand.NextDouble () - 0.5f, (float) Main.rand.NextDouble () - 0.5f), mod.ProjectileType ("GhostSpear"), 30, 10.0f, Main.myPlayer);
				if (Main.rand.Next (2) == 0) Projectile.NewProjectile (origin + orthogonal * -n, direction * 3.2f + new Vector2 ((float) Main.rand.NextDouble () - 0.5f, (float) Main.rand.NextDouble () - 0.5f), mod.ProjectileType ("GhostSpear"), 30, 10.0f, Main.myPlayer);
			}
		}

		void UpdateRotation () {
			npc.rotation += 0.005f;
		}
	}
}