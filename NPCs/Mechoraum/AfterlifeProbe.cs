using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HMPlus.Lib;

namespace HMPlus.NPCs.Mechoraum {
	public class AfterlifeProbe : ModNPC {
		private Player player;

		public override void SetStaticDefaults () {
			DisplayName.SetDefault ("Afterlife Probe");
		}

		public override void SetDefaults () {
			npc.CloneDefaults (NPCID.Probe);
			npc.alpha = 80;
		}

		public override bool PreNPCLoot () {
			return false; // don't drop hearts
		}
	}
}