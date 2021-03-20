using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HMPlus.Lib;

namespace HMPlus.Items
{
	public class Darkspace : ModItem
	{
		public override void SetStaticDefaults ()
		{
			DisplayName.SetDefault("Darkspace");
			Tooltip.SetDefault("Casts a lifeless emission of darkness\n'It channels through the void of the cosmos.'");
		}

		public override void SetDefaults ()
		{
			item.damage = 90;
			item.melee = true;
			item.width = 64;
			item.height = 64;
			item.useTime = 40;
			item.useAnimation = 40;
			item.useStyle = 1;
			item.knockBack = 4.0f;
			item.value = 30000;
			item.rare = 8;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = true;
			item.shoot = mod.ProjectileType ("DarknessEmission");
			item.shootSpeed = 12f;
		}

		public override void AddRecipes ()
		{
			foreach (int tier3SwordID in HMPlusLib.tier3SwordIDs) {
				foreach (int evilBarID in HMPlusLib.evilBarIDs) {
					ModRecipe recipe = new ModRecipe (mod);
					recipe.AddIngredient (evilBarID, 18);
					recipe.AddIngredient (tier3SwordID, 1);
					recipe.AddIngredient (ItemID.SpectreBar, 12);
					recipe.AddTile (TileID.DemonAltar);
					recipe.SetResult (this);
					recipe.AddRecipe ();
				}
			}
		}
	}
}
