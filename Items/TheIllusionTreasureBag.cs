using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HMPlus.Lib;

namespace HMPlus.Items
{
	public class TheIllusionTreasureBag : ModItem
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
			item.rare = 8;
			item.expert = true;
			bossBagNPC = mod.NPCType ("TheIllusion");
		}

		public override bool CanRightClick () { return true; }

		public override void OpenBossBag (Player player) {
			player.TryGettingDevArmor ();
			player.QuickSpawnItem (ItemID.SpectreBar, Main.rand.Next (7) + 6);
			player.QuickSpawnItem (ItemID.SpectreMask, 1);
			player.QuickSpawnItem (ItemID.SpectreHood, 1);
			player.QuickSpawnItem (ItemID.SpectreRobe, 1);
			player.QuickSpawnItem (ItemID.SpectrePants, 1);
			player.QuickSpawnItem (ItemID.SpectreStaff, 1);
			player.QuickSpawnItem (ItemID.WispinaBottle, 1);

			Mod calamityMod = ModLoader.GetMod ("CalamityMod");
			if (calamityMod != null) player.QuickSpawnItem (calamityMod.ItemType ("SpectreRifle"), 1);

			Mod fargowiltas = ModLoader.GetMod ("Fargowiltas");
			if (fargowiltas != null) player.QuickSpawnItem (ItemID.UnholyTrident, 1);
		}
	}
}
