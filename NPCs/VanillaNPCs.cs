using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HMPlus.Lib;

namespace HMPlus.NPCs {
	public class VanillaNPCs : GlobalNPC {
		public override void NPCLoot (NPC npc) {
			if (npc.type == NPCID.WallofFlesh) {
				if (Main.expertMode == false) HMPlusLib.NPCDropModItem (mod, npc, "DemonHorn", 2);
			} else if (npc.type == NPCID.Plantera) {
				HMPlusLib.NPCDropModItem (mod, npc, "PlanteraBud", 1);
			} else if (npc.type == NPCID.DungeonSpirit) {
				/*if (npc.defense < 100) {
					HMPlusWorld.dungeonSpiritsKilled += 1;
					if (HMPlusWorld.dungeonSpiritsKilled % (HMPlusWorld.downedTheIllusion ? 48 : 12) == 0) {
						NPC.SpawnOnPlayer (0, mod.NPCType ("TheIllusion"));
					}
				}*/
			} else if (npc.type == NPCID.DD2Betsy) {
				if (Main.expertMode)
					HMPlusLib.NPCDropModItem (mod, npc, "DragonPortal", 1);
			}
		}
	}
}