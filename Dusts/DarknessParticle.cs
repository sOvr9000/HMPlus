using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HMPlus.Projectiles;
using HMPlus.Items;

namespace HMPlus.Dusts {
	public class DarknessParticle : ModDust {
		int frame;
		int frameY;

		public override void OnSpawn (Dust dust) {
			dust.noGravity = true;
			dust.noLight = true;
			dust.frame = new Rectangle (0, 0, 10, 10);

			frame = 0;
			frameY = 0;
		}

		public override bool Update (Dust dust) {
			dust.velocity += Main.rand.NextVector2Unit () * 0.0167f;
			Lighting.AddLight (dust.position, 0.4f * dust.scale, 1.6f * dust.scale, 2.0f * dust.scale);
			if (++ frame % 3 == 0) {
				frameY += 10;
				if (frameY >= 50) {
					frameY = 0;
				}
				dust.frame.Y = frameY;
				dust.alpha += 3;
				if (dust.alpha >= 255) {
					dust.active = false;
				}
			}
			return false;
		}
	}
}