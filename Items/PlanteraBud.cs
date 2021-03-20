using Terraria.ID;
using Terraria.ModLoader;
using HMPlus.Lib;

namespace HMPlus.Items
{
	public class PlanteraBud : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plantera Bud");
			Tooltip.SetDefault("'Five bucks if you put it in his salad!'");
		}
		public override void SetDefaults()
		{
			item.maxStack = 99;
			item.width = 40;
			item.height = 40;
			item.value = 8000;
			item.rare = 7;
		}
	}
}
