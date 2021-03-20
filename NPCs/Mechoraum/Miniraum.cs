using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HMPlus.Lib;

namespace HMPlus.NPCs.Mechoraum {
	public class Miniraum : ModNPC {
		public NPC spawnedBy;
		private Player player;

		private float angularVelocity;
		private float maxFlySpeed;
		private float chargeSpeed;
		private float acceleration;
		private float deceleration;

		private int timeDuringCharge;
		
		public override void SetStaticDefaults () {
			DisplayName.SetDefault ("Miniraum");
		}

		public override void SetDefaults () {
			npc.aiStyle = -1; // My own AI!
			npc.lifeMax = 1000;
			npc.damage = 50;
			npc.defense = 0;
			npc.knockBackResist = 0.5f;
			npc.width = 44;
			npc.height = 44;
			npc.value = 2500;
			npc.npcSlots = 1;
			npc.boss = false;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit4; // metal
			npc.DeathSound = SoundID.NPCDeath14; // mechanical explosion

			angularVelocity = 0.115f;
			maxFlySpeed = 25f;
			acceleration = 0.40f;
			deceleration = 0.962f;
			timeDuringCharge = 110;
			chargeSpeed = 46f;
			npc.ai [3] = 30f;
		}

		public override void ScaleExpertStats (int numPlayers, float bossLifeScale) {
			npc.lifeMax = (int) (npc.lifeMax * 0.75f * bossLifeScale);
			npc.damage = (int) (npc.damage * 1.25f);
		}

		public override bool? DrawHealthBar (byte hbPosition, ref float scale, ref Vector2 position) {
			scale = 0.625f;
			return null;
		}

		public override void NPCLoot () {
			for (int k = 0; k < 200; k ++) {
				if (Main.npc [k].type == mod.NPCType ("Mechoraum"))
					Main.npc [k].ai [1] --;
			}
		}

		public override void AI () {
			Target ();

			UpdateRotation ();
			FlyTowardNextLocation ();
			ChargeTowardPlayer ();
			ShootAtPlayer ();
		}

		void Target () {
			npc.TargetClosest (false);
			player = Main.player [npc.target];
		}

		void FlyTowardNextLocation () {
			npc.velocity += (player.Center - npc.Center).SafeNormalize (Vector2.Zero) * acceleration;
			if (npc.velocity.LengthSquared () > maxFlySpeed) {
				npc.velocity *= deceleration;
			}
		}

		void ChargeTowardPlayer () {
			npc.ai [2] --;
			if (npc.ai [2] > 0) return;
			npc.ai [2] = timeDuringCharge + Main.rand.Next (-8, 8);
			if (Main.rand.Next (2) != 0) return;
			npc.velocity = ((player.Center + new Vector2 (Main.rand.Next (-20, 20), Main.rand.Next (-20, 20))) - npc.Center).SafeNormalize (Vector2.UnitX) * (chargeSpeed + Main.rand.NextFloat (-3f, 3f));
		}

		void UpdateRotation () {
			npc.rotation += angularVelocity;
		}

		void ShootAtPlayer () {
			npc.ai [0] += 1f;
			if (npc.ai [0] < 20f) return;
			npc.ai [0] = 0f;
			if (Main.rand.Next (18) != 0) return;
			Vector2 delta = player.Center - npc.Center;
			if (delta.LengthSquared () < 57600f) return; // must be 240 pixels = 24 tiles away
            Vector2 direction = delta.SafeNormalize (Vector2.UnitX) * 16f;
			Projectile.NewProjectile (npc.Center.X, npc.Center.Y, direction.X, direction.Y, ProjectileID.DeathLaser, 20, 0, Main.myPlayer, 0f, 0f);
		}
	}
}