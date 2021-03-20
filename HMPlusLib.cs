using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HMPlus.Lib {
	public static class HMPlusLib {
		public static int [] evilBrickIDs { get; private set; }
		public static int [] evilBarIDs { get; private set; }
		public static int [] ironBarIDs { get; private set; }
		public static int [] goldBarIDs { get; private set; }
		public static int [] tier3SwordIDs { get; private set; }

		public static void Init () {
			evilBrickIDs = new int [] { ItemID.DemoniteBrick, ItemID.CrimtaneBrick };
			evilBarIDs = new int [] { ItemID.DemoniteBar, ItemID.CrimtaneBar };
			ironBarIDs = new int [] { ItemID.IronBar, ItemID.LeadBar };
			goldBarIDs = new int [] { ItemID.GoldBar, ItemID.PlatinumBar };
			tier3SwordIDs = new int [] { ItemID.AdamantiteSword, ItemID.TitaniumSword };
		}

		public static bool NPCIsMechanicalBoss (Mod mod, NPC npc) {
			return npc.type == mod.NPCType ("Mechoraum") || npc.type == 134 || npc.type == 135 || npc.type == 136 || npc.type == 139 || (npc.type >= 125 && npc.type <= 131);
		}

		public static void NPCDropModItem (Mod mod, NPC npc, string itemName, int amount = 1) {
			Item.NewItem ((int) npc.position.X, (int) npc.position.Y, npc.width, npc.height, mod.ItemType (itemName), amount);
		}

		public static void NPCDropItem (Mod mod, NPC npc, int itemType, int amount = 1) {
			Item.NewItem ((int) npc.position.X, (int) npc.position.Y, npc.width, npc.height, itemType, amount);
		}

		public static bool NPCCannotReceiveDamage (NPC npc) {
			return npc == null || npc.immortal || !npc.active || npc.dontTakeDamage || npc.takenDamageMultiplier == 0f;
		}

		public static bool ModIsInstalled (string name) {
			return ModLoader.GetMod (name) != null;
        }
	}

	public class ColorCycle {
		public int index { get; private set; }
		public int length { get; private set; }
		public Color [] colors { get; private set; }

		public ColorCycle (Color [] colors) {
			index = -1;
			length = colors.Length;
			this.colors = colors;
		}

		public Color Next () {
			index = (index + 1) % length;
			return colors [index];
		}
	}
}