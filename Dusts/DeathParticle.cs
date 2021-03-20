using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HMPlus.Dusts {
	public class DeathParticle : ModDust {
		public override void OnSpawn (Dust dust) {
			dust.color = new Color (211, 25, 253);
			dust.alpha = 1;
			dust.scale = 1.4f;
			dust.noGravity = true;
			dust.noLight = true;
			dust.frame = new Rectangle (0, 0, 10, 10);
		}

		public override bool Update (Dust dust) {
			dust.position += dust.velocity;
			dust.rotation += dust.velocity.X * 0.15f;
			dust.velocity *= 0.97f;
			dust.scale *= (95 + Main.rand.Next (7)) * 0.01f;
			Lighting.AddLight (dust.position, 0.35f * dust.scale, 0.04f * dust.scale, 0.4f * dust.scale);
			if (dust.scale <= 0.2f) {
				dust.active = false;
			}
			return false;
		}
	}
}