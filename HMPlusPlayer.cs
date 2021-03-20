using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameInput;

namespace HMPlus {
	public class HMPlusPlayer : ModPlayer {
		public bool earthenEye = false;
		public bool overwhelmed = false;

        public override void ResetEffects () {
			earthenEye = false;
			overwhelmed = false;
        }

		public override void UpdateDead () {
			earthenEye = false;
			overwhelmed = false;
		}

		public override void UpdateBadLifeRegen () {
			// ...
			if (overwhelmed) {
				player.lifeRegenTime = 0;
				player.lifeRegen = -121;
			}
		}

		public override void PostUpdateBuffs () {
			
		}

		public override void PostUpdateEquips () {
			
		}

		public override bool PreHurt (bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit,
			ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) {
			return base.PreHurt (pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
		}

		public override void Hurt (bool pvp, bool quiet, double damage, int hitDirection, bool crit) {
			
		}

		public override bool PreKill (double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) {
			return true;
		}

		public override void DrawEffects (PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright) {
			if (earthenEye) {
				// :D
			}
		}
	}
}
