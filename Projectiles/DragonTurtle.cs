using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using HMPlus.Lib;

namespace HMPlus.Projectiles {
	public class DragonTurtle : ModProjectile {
		public static DragonCurveData dragonCurveData;

		public override void SetStaticDefaults () {

		}

		public override void SetDefaults () {
			projectile.friendly = true;
			projectile.damage = 1;
			projectile.width = 4;
			projectile.height = 4;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 342;
			projectile.penetrate = 200;
			projectile.light = 0.5f;
			projectile.extraUpdates = 1;
			projectile.ignoreWater = true;

			projectile.velocity = Vector2.Zero;
		}

		public override void AI () {
			DragonCurveData dcd = dragonCurveData;
			if (dragonCurveData == null) return;
			Vector2 [] vs = dcd.NextVertices (12);
			if (vs.Length > 0) {
				Vector2 lastPos = projectile.Center;
				int r, g, b;
				HSV2RGB.HsvToRgb (360d * dcd.currentTerm * dcd.invNumVertices, 1d, 1d, out r, out g, out b);
				Color dustColor = new Color (r, g, b);
				foreach (Vector2 v in vs) {
					Dust.NewDust (v, 10, 10, mod.DustType ("DragonCurveDust"), 0f, 0f, 0, dustColor, 1f);
					lastPos = v;
				}
				projectile.Center = lastPos;
			} else {
				dragonCurveData = null;
			}
		}
	}
}
