using Terraria.ID;
using Terraria.ModLoader;
using HMPlus.Lib;

namespace HMPlus.Items
{
	public class BladeOfHell : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blade of Hell");
			Tooltip.SetDefault("Shoots fire balls from the Underworld!\nWielding this blade makes you tremble of some wicked terror...");
		}

		public override void SetDefaults()
		{
			item.damage = 63;
			item.melee = true;
			item.width = 50;
			item.height = 50;
			item.useTime = 34;
			item.useAnimation = 34;
			item.useStyle = 1;
			item.knockBack = 7.5f;
			item.value = 20000;
			item.rare = 4;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = true;
			item.shoot = 15; // Ball of Fire
			item.shootSpeed = 5.2f;
		}

		public override void AddRecipes()
		{
			foreach (int goldBarID in HMPlusLib.goldBarIDs) {
				foreach (int evilBrickID in HMPlusLib.evilBrickIDs) {
					ModRecipe recipe = new ModRecipe (mod);
					recipe.AddIngredient (121, 1); // Fiery Greatsword
					recipe.AddIngredient (mod.ItemType ("DemonHorn"), 2);
					recipe.AddIngredient (ItemID.HellstoneBar, 6);
					recipe.AddIngredient (goldBarID, 6);
					recipe.AddIngredient (evilBrickID, 6);
					recipe.AddTile (TileID.Anvils);
					recipe.SetResult (this);
					recipe.AddRecipe ();
				}
			}
		}
	}
}
