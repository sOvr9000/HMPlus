using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HMPlus.Lib;

namespace HMPlus.Items
{
	public class SplitSecond : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Split Second");
			Tooltip.SetDefault("Shoots the revealed light\nThe revealed light unleashes multiple homing obstructed lights.\nHoming obstructed lights sometimes ignore NPC immunity frames.\n'It will be over soon.'");
		}

		public override void SetDefaults()
		{
			item.damage = HMPlusLib.ModIsInstalled ("CalamityMod") || HMPlusLib.ModIsInstalled ("ThoriumMod") || HMPlusLib.ModIsInstalled ("SacredTools") ? 90 : 120;
			item.melee = true;
			item.width = 100;
			item.height = 100;
			item.useTime = 70;
			item.useAnimation = 60;
			item.useStyle = 1;
			item.knockBack = 7.0f;
			item.value = 300000;
			item.rare = 10;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = true;
			item.shoot = mod.ProjectileType ("RevealedLight");
			item.shootSpeed = 5.6f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe (mod);
			recipe.AddIngredient (ItemID.BrokenHeroSword, 1);
			recipe.AddIngredient (mod.ItemType ("TrueBladeOfHell"), 1);
			recipe.AddIngredient (ItemID.Meowmere, 1);
			recipe.AddTile (412); // Ancient Manipulator
			recipe.SetResult (this);
			recipe.AddRecipe ();

			recipe = new ModRecipe (mod);
			recipe.AddIngredient (ItemID.BrokenHeroSword, 1);
			recipe.AddIngredient (mod.ItemType ("TrueBladeOfHell"), 1);
			recipe.AddIngredient (ItemID.StarWrath, 1);
			recipe.AddTile (412); // Ancient Manipulator
			recipe.SetResult (this);
			recipe.AddRecipe ();
		}
	}
}
