using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using HMPlus.Lib;

namespace HMPlus.Items
{
	public class DragonPortal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dragon Portal");
			Tooltip.SetDefault("Open the portal and let the beautiful dragon fly into this world.");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.useTime = 60;
			item.useAnimation = 56;
			item.useStyle = 4;
			item.value = 1;
			item.rare = 8;
			item.expert = true;
			item.consumable = true;
		}

		public override bool CanUseItem (Player player) {
			return true;
		}

		public override bool UseItem (Player player) {
			if (Projectiles.DragonTurtle.dragonCurveData != null) return false;
			Vector2 delta = Main.MouseWorld - player.Center;
			double angle = Math.Atan2 (-delta.Y, delta.X);
			ErrorLogger.Log ("angle = " + Math.Round (MathHelper.ToDegrees ((float) angle), 1) + " degrees");
			DragonCurveData dragonCurveData = new DragonCurveData ();
            dragonCurveData.Generate (player.Center, 12, angle, 12);
			Projectiles.DragonTurtle.dragonCurveData = dragonCurveData;
			Projectile.NewProjectile (dragonCurveData.startPoint, Vector2.Zero, mod.ProjectileType ("DragonTurtle"), 1, 0f, player.whoAmI, 0, 0);
			return true;
		}
	}
}
