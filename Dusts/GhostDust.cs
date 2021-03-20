using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HMPlus.Dusts {
	public class GhostDust : ModDust {
		private int frameY = 0;

		public override void OnSpawn (Dust dust) {
			dust.noGravity = true;
			dust.frame = new Rectangle (0, 0, 10, 10);
		}

		public override bool Update (Dust dust) {
			dust.position += dust.velocity;
			dust.rotation += dust.velocity.X * 0.15f;
			dust.velocity *= 0.97f;
			dust.scale *= 0.9735f; // takes 60 frames to get to 0.2f (log_0.9735 (0.2) = 60...)
			Lighting.AddLight (dust.position, dust.color.R * 0.004f, dust.color.G * 0.004f, dust.color.B * 0.004f);
			if (dust.scale <= 0.2f) {
				dust.active = false;
			}
			return false;
		}
	}
}