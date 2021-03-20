using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HMPlus.Dusts {
	public class DragonCurveDust : ModDust {
		public override void OnSpawn (Dust dust) {
			dust.noGravity = true;
			dust.frame = new Rectangle (0, 0, 10, 10);
			dust.scale = 0.9f;
			dust.alpha = -300;
        }

		public override bool Update (Dust dust) {
			if (dust.alpha < 0) {
				dust.alpha++;
			} else {
				dust.alpha += 3;
			}
			float brightness = (1f - dust.alpha * 0.003921f) * 0.003f;
            Lighting.AddLight (dust.position, dust.color.R * brightness, dust.color.G * brightness, dust.color.B * brightness);
			if (dust.alpha > 255) {
				dust.active = false;
			}
			return false;
		}
	}
}