using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HMPlus.Items
{
	[AutoloadEquip (EquipType.Shield)]
	public class EarthenEye : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Earthen Eye");
			Tooltip.SetDefault("+7% damage\n+6% critical strike chance\n+40 max life\nIncreased life regeneration");
		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 28;
			item.value = 20000;
			item.rare = 7;
			item.accessory = true;
			item.defense = 6;
			item.lifeRegen = 3;
		}

		public override void UpdateAccessory (Player player, bool hideVisual) {
			player.statLifeMax2 += 40;
			player.meleeDamage *= 1.07f;
			player.thrownDamage *= 1.07f;
			player.rangedDamage *= 1.07f;
			player.magicDamage *= 1.07f;
			player.minionDamage *= 1.07f;
			player.meleeCrit += 6;
			player.thrownCrit += 6;
			player.rangedCrit += 6;
			player.magicCrit += 6;
			player.GetModPlayer<HMPlusPlayer> (mod).earthenEye = true;
		}

		public override void AddRecipes () {
			ModRecipe recipe = new ModRecipe (mod);
			recipe.AddIngredient (mod.ItemType ("MechanicalEyeSocket"), 1);
			recipe.AddIngredient (mod.ItemType ("PlanteraBud"), 1);
			recipe.AddIngredient (ItemID.LifeFruit, 8);
			recipe.AddIngredient (ItemID.JungleGrassSeeds, 3);
			recipe.AddIngredient (ItemID.SoulofLight, 5);
			recipe.needWater = true;
			recipe.SetResult (this);
			recipe.AddRecipe ();
		}
	}
}
