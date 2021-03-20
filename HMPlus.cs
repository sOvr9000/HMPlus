using System;
using Terraria.ModLoader;
using HMPlus.Lib;

namespace HMPlus
{
	class HMPlus : Mod
	{
		public HMPlus()
		{
			HMPlusLib.Init ();
			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}

		public override void PostSetupContent () {
			Mod bossChecklist = ModLoader.GetMod ("BossChecklist");
			if (bossChecklist != null) {
				bossChecklist.Call ("AddBossWithInfo", "Mechoraum", 9.1f, (Func<bool>) (() => HMPlusWorld.downedMechoraum), "Use a [i:" + ItemType ("Sawblade") + "]");
				bossChecklist.Call ("AddBossWithInfo", "The Illusion", 10.6f, (Func<bool>) (() => HMPlusWorld.downedTheIllusion), "Use an [i:" + ItemType ("EctoplasmicDistortion") + "]");
			}
		}
	}
}
