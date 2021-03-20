using Terraria.ID;
using Terraria.ModLoader;
using HMPlus.Lib;

namespace HMPlus.Items
{
	public class EvilSawtooth : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Evil Sawtooth");
			Tooltip.SetDefault("If a giant sawblade is going to attack you, you might as well scavenge its parts!");
		}
		public override void SetDefaults()
		{
			item.maxStack = 99;
			item.width = 29;
			item.height = 40;
			item.value = 2000;
			item.rare = 5;
		}
	}
}
