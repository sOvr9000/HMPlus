using Terraria.ID;
using Terraria.ModLoader;
using HMPlus.Lib;

namespace HMPlus.Items
{
	public class MechanicalEyeSocket : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mechanical Eye Socket");
		}
		public override void SetDefaults()
		{
			item.maxStack = 99;
			item.width = 40;
			item.height = 40;
			item.useTime = 34;
			item.useAnimation = 34;
			item.useStyle = 0;
			item.value = 2000;
			item.rare = 5;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
		}
	}
}
