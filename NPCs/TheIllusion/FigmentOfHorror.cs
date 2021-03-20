using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace HMPlus.NPCs.TheIllusion {
	public class FigmentOfHorror : ModNPC {
		private Player player;

		private int frameY;
		private int frameDirection;
		private int frameIteraion;

		public static readonly Color [] colorCycle = new Color [] {
			new Color (255, 255, 0),
			new Color (255, 255, 255)
		};

		private int colorIndex;
		private float brightness;

		public override void SetStaticDefaults () {
			DisplayName.SetDefault ("Figment of Horror");
			Main.npcFrameCount [npc.type] = 6;
		}

		public override void SetDefaults () {
			npc.aiStyle = -1; // My own AI!
			npc.lifeMax = 10000;
			npc.damage = 40;
			npc.defense = 0;
			npc.knockBackResist = 0f;
			npc.width = 56;
			npc.height = 56;
			npc.scale = 1.5f;
			npc.value = 0;
			npc.npcSlots = 1;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit3; // fire ball collision
			npc.DeathSound = SoundID.NPCDeath1; // zombie

			frameY = 0;
			frameDirection = 1;
			frameIteraion = 0;

			colorIndex = 0;
		}

		public override void ScaleExpertStats (int numPlayers, float bossLifeScale) {
			npc.lifeMax = (int) (npc.lifeMax * bossLifeScale);
		}

		public override bool PreNPCLoot () {
			for (int k = 0; k < 200; k ++) {
				NPC otherNPC = Main.npc [k];
				if (otherNPC.type == mod.NPCType ("TheIllusion")) {
					otherNPC.ai [2] -= 1f;
					if (otherNPC.ai [2] <= 0f) {
						otherNPC.immortal = false;
					}
				}
			}
			return false;
		}

		public override bool? DrawHealthBar (byte hbPosition, ref float scale, ref Vector2 position) {
			scale = 1.6f;
			return null;
		}

		public override void FindFrame (int frameSize) {
			npc.frameCounter += 1f;
			if (++frameIteraion < 7) return;
			frameIteraion = 0;
			if (frameY == 4) {
				frameDirection = -1;
				colorIndex = (colorIndex + 1) % colorCycle.Length;
				npc.color = colorCycle [colorIndex];
			} else if (frameY == 0) {
				frameDirection = 1;
			}
			brightness = (5 - frameY) * 0.5f;
			frameY += frameDirection;
			npc.frame.Y = frameY * frameSize;
			npc.rotation = frameY * 0.2f * MathHelper.Pi;
		}

		public override void ModifyHitByItem (Player player, Item item, ref int damage, ref float knockback, ref bool crit) {
			if (player.name == "modding") damage *= 10;
		}

		public override void AI () {
			Target ();

			Color rgb = colorCycle [colorIndex];
			Lighting.AddLight (npc.Center + new Vector2 (40f, 40f), rgb.R * 0.02f * brightness, rgb.G * 0.02f * brightness, rgb.B * 0.02f * brightness);

			npc.ai [0] += 1f;
			FlyAroundPlayer ();
			ShootAtPlayer ();
		}

		void Target () {
			npc.TargetClosest (false);
			player = Main.player [npc.target];
		}

		void FlyAroundPlayer () {
			npc.ai [1] += 0.007f;
			npc.velocity = (player.Center + new Vector2 ((float) Math.Cos (npc.ai [1]) * TheIllusion.figmentDistance, (float) Math.Sin (npc.ai [1]) * TheIllusion.figmentDistance) - npc.Center) * 0.025f;
		}

		void ShootAtPlayer () {
			if (npc.ai [0] < 110f) return;
			npc.ai [0] = 0f;
			if (Main.rand.Next (3) != 0) return;
			Vector2 velocity = (player.Center - npc.Center).SafeNormalize (new Vector2 (1f, 0f)) * 2.5f;
			Projectile.NewProjectile (npc.Center, velocity, mod.ProjectileType ("PhantasmaOrb"), 35, 6.0f, Main.myPlayer, 0, 0);
		}
	}
}