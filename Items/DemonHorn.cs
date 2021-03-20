using Terraria.ID;
using Terraria.ModLoader;
using HMPlus.Lib;

namespace HMPlus.Items
{
	public class DemonHorn : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Demon Horn");
			Tooltip.SetDefault("This came off of the Wall of Flesh");
		}
		public override void SetDefaults()
		{
			item.maxStack = 99;
			item.width = 50;
			item.height = 50;
			item.value = 7500;
			item.rare = 4;
		}
	}
}
