using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HMPlus.Lib;

namespace HMPlus.Items
{
	public class Razorfell : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Razorfell");
			Tooltip.SetDefault("Warning! Evil razor may try to shave your head.");
		}

		public override void SetDefaults()
		{
			item.damage = 79;
			item.thrown = true;
			item.width = 32;
			item.height = 32;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.knockBack = 4.5f;
			item.value = 10000;
			item.rare = 6;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = true;
			item.shoot = mod.ProjectileType ("RazorfellProjectile");
			item.shootSpeed = 10f;
		}

		public override void AddRecipes () {
			ModRecipe recipe = new ModRecipe (mod);
			recipe.AddIngredient (mod.ItemType ("MechanicalEyeSocket"), 1);
			recipe.AddIngredient (mod.ItemType ("EvilSawtooth"), 8);
			recipe.AddTile (TileID.Anvils);
			recipe.SetResult (this);
			recipe.AddRecipe ();
		}
	}
}
