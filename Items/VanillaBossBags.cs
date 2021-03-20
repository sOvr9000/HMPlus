using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HMPlus.Items {
	public class VanillaBossBags : GlobalItem {
		public override void OpenVanillaBag (string context, Player player, int arg) {
			if (context == "bossBag" && arg == ItemID.WallOfFleshBossBag) {
				player.QuickSpawnItem (mod.ItemType ("DemonHorn"), 2);
			}
		}
	}
}
