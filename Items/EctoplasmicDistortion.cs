using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HMPlus.Items
{
	public class EctoplasmicDistortion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ectoplasmic Distortion");
			Tooltip.SetDefault("It phases through your hands...");
		}

		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 32;
			item.height = 32;
			item.useTime = 12;
			item.useAnimation = 10;
			item.useStyle = 4;
			item.value = 60000;
			item.rare = 1;
			item.UseSound = SoundID.Item1;
			item.consumable = true;
		}

		public override bool CanUseItem (Player player) {
			return Main.hardMode && player.ZoneDungeon && NPC.AnyNPCs (mod.NPCType ("TheIllusion")) == false;
		}

		public override bool UseItem (Player player) {
			NPC.SpawnOnPlayer (player.whoAmI, mod.NPCType ("TheIllusion"));
			return true;
		}

		public override void AddRecipes () {
			ModRecipe recipe = new ModRecipe (mod);
			recipe.AddIngredient (ItemID.Ectoplasm, 14);
			recipe.AddTile (TileID.Anvils);
			recipe.SetResult (this);
			recipe.AddRecipe ();
		}
	}
}
