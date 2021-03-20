using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HMPlus.Lib;

namespace HMPlus.Items
{
	public class TrueBladeOfHell : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Blade of Hell");
			Tooltip.SetDefault("You can feel it take a deathly grasp on your soul.\nShoots the obstructed light hidden within the sword\nThe projectile will burn enemies for two seconds and cause extra damage to the mechanical bosses.");
		}

		public override void SetDefaults()
		{
			item.damage = 121;
			item.melee = true;
			item.width = 46;
			item.height = 46;
			item.useTime = 22;
			item.useAnimation = 22;
			item.useStyle = 1;
			item.knockBack = 7.2f;
			item.value = 200000;
			item.rare = 6;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = true;
			item.shoot = mod.ProjectileType ("ObstructedLight");
			item.shootSpeed = 4.3f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe (mod);
			recipe.AddIngredient (ItemID.BrokenHeroSword, 1); // Really a broken demon sword
			recipe.AddIngredient (mod.ItemType ("BladeOfHell"), 1);
			recipe.AddIngredient (ItemID.DarkLance, 1);
			recipe.AddIngredient (112, 1); // Flower of Fire
			recipe.AddIngredient (ItemID.Sunfury, 1);
			recipe.AddIngredient (ItemID.Flamelash, 1);
			recipe.AddIngredient (ItemID.HellwingBow, 1);
			recipe.AddIngredient (521, 5); // Souls of Night
			recipe.AddTile (336); // Living Fire Blocks
			recipe.SetResult (this);
			recipe.AddRecipe ();
		}

		public override void MeleeEffects (Player player, Rectangle hitbox) {
			
		}
	}
}
