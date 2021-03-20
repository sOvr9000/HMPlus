using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HMPlus.Lib;

namespace HMPlus.Items
{
	public class Sawblade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sawblade");
			Tooltip.SetDefault("Don't touch, or else...!");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 20;
			item.height = 20;
			item.useTime = 20;
			item.useAnimation = 15;
			item.useStyle = 4;
			item.value = 30000;
			item.rare = 1;
			item.UseSound = SoundID.Item1;
			item.consumable = true;
		}
		public override bool CanUseItem (Player player) {
			return Main.dayTime == false && Main.hardMode && NPC.AnyNPCs (mod.NPCType ("Mechoraum")) == false;
		}

		public override bool UseItem (Player player) {
			NPC.SpawnOnPlayer (player.whoAmI, mod.NPCType ("Mechoraum"));
			return true;
		}

		public override void AddRecipes () {
			foreach (int ironBarID in HMPlusLib.ironBarIDs) {
				ModRecipe recipe = new ModRecipe (mod);
				recipe.AddIngredient (ironBarID, 6);
				recipe.AddIngredient (547, 3); // Souls of Fright
				recipe.AddIngredient (548, 3); // Souls of Might
				recipe.AddIngredient (549, 3); // Souls of Sight
				recipe.AddTile (TileID.Anvils);
				recipe.SetResult (this);
				recipe.AddRecipe ();
			}
		}
	}
}
