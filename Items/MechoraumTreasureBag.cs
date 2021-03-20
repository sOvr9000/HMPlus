using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HMPlus.Lib;

namespace HMPlus.Items
{
	public class MechoraumTreasureBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
		}

		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.consumable = true;
			item.width = 32;
			item.height = 32;
			item.rare = 5;
			item.expert = true;
			bossBagNPC = mod.NPCType ("Mechoraum");
		}

		public override bool CanRightClick () { return true; }

		public override void OpenBossBag (Player player) {
			player.TryGettingDevArmor ();
			player.QuickSpawnItem (ItemID.HallowedBar, Main.rand.Next (20, 35));
			player.QuickSpawnItem (ItemID.MechanicalEye, Main.rand.Next (1, 3));
			player.QuickSpawnItem (ItemID.MechanicalWorm, Main.rand.Next (1, 3));
			player.QuickSpawnItem (ItemID.MechanicalSkull, Main.rand.Next (1, 3));
			player.QuickSpawnItem (mod.ItemType ("EvilSawtooth"), 8);
			player.QuickSpawnItem (mod.ItemType ("MechanicalEyeSocket"), 1);
		}
	}
}
