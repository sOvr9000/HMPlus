using Terraria;
using Terraria.ModLoader;
using HMPlus.NPCs;

namespace HMPlus.Buffs {
	public class Overwhelmed : ModBuff {
		public override void SetDefaults () {
			DisplayName.SetDefault ("Overwhelmed");
			Description.SetDefault ("You are turning into a roaming, mad creature of the dungeon.");
			Main.debuff [Type] = true;
		}

		public override void Update (Player player, ref int buffIndex) {
			player.GetModPlayer<HMPlusPlayer> (mod).overwhelmed = true;
		}
	}
}
