using System;
using Terraria;
using Terraria.ModLoader;
using HMPlus.Items;

namespace HMPlus.Prefixes {
	public class Ascending : ModPrefix {
		// see documentation for vanilla weights and more information
		// note: a weight of 0f can still be rolled. see CanRoll to exclude prefixes.
		// note: if you use PrefixCategory.Custom, actually use ChoosePrefix instead, see ExampleInstancedGlobalItem
		public override float RollChance (Item item) {
			return 0.5f;
		}

		// determines if it can roll at all.
		// use this to control if a prefixes can be rolled or not
		public override bool CanRoll (Item item) {
			return true;
		}

		// change your category this way, defaults to Custom
		public override PrefixCategory Category { get { return PrefixCategory.AnyWeapon; } }

		// Allow multiple prefix autoloading this way (permutations of the same prefix)
		public override bool Autoload (ref string name) {
			if (base.Autoload (ref name)) {
				mod.AddPrefix ("Ascending", new Ascending ());
			}
			return false;
		}

		public override void Apply (Item item) {
			item.GetGlobalItem<InstancedGlobalItem> ().ascending = 1;
			item.damage = (int) Math.Ceiling (item.damage * 1.18d);								// +18% damage
			item.useTime = Math.Max ((int) Math.Ceiling (item.useTime / 1.12d), 1);				// +12% speed
			item.crit += 8;																		// +8% crit chance
			item.knockBack *= 1.23f;															// +23% knockback
			item.value = (int) Math.Ceiling (item.value * 5.8d);								// +480% sell/reforge value
			item.rare += 3;																		// +3 tier (Legendary/Mythical/Unreal is +2)
			if (item.ranged) {
				item.shootSpeed *= 1.12f;														// +12% projectile velocity
			} else if (item.magic) {
				item.mana = (int) Math.Ceiling (item.mana / 1.12d);								// -12% mana cost
			} else if (item.melee) {
				item.Size *= 1.12f;																// +12% size
			}
		}

		public override void ModifyValue (ref float valueMult) {
			valueMult *= 1.18f;
		}
	}
}