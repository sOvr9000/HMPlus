using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace HMPlus.Items {
	public class InstancedGlobalItem : GlobalItem {
		public byte ascending;

		private static string saveOriginalOwner;

		public InstancedGlobalItem () {
			ascending = 0;
		}

		public override bool InstancePerEntity {
			get {
				return true;
			}
		}

		public override GlobalItem Clone (Item item, Item itemClone) {
			InstancedGlobalItem myClone = (InstancedGlobalItem) base.Clone (item, itemClone);
			myClone.ascending = ascending;
			return myClone;
		}

		public override int ChoosePrefix (Item item, UnifiedRandom rand) {
			if (item.accessory || item.damage > 0 && item.maxStack == 1 && rand.NextBool (30)) {
				return mod.PrefixType ("Ascending");
			}
			return -1;
		}

		public override void ModifyTooltips (Item item, List<TooltipLine> tooltips) {
			//if (!item.social && item.prefix > 0) {
			//	int awesomeBonus = ascending - Main.cpItem.GetGlobalItem<InstancedGlobalItem> ().ascending;
			//	if (awesomeBonus > 0) {
			//		TooltipLine line = new TooltipLine (mod, "Ascending", "+" + awesomeBonus + " awesomeness");
			//		line.isModifier = true;
			//		tooltips.Add (line);
			//	}
			//}
			//if (originalOwner.Length > 0) {
				//TooltipLine line = new TooltipLine (mod, "CraftedBy", "Crafted by: " + originalOwner);
				//line.overrideColor = Color.LimeGreen;
				//tooltips.Add (line);

				/*foreach (TooltipLine line2 in tooltips)
				{
					if (line2.mod == "Terraria" && line2.Name == "ItemName")
					{
						line2.text = originalOwner + "'s " + line2.text;
					}
				}*/
			//}
		}

		//public override void Load (Item item, TagCompound tag) {
		//	//originalOwner = tag.GetString ("originalOwner");
		//}

		//public override bool NeedsSaving (Item item) {
		//	//return originalOwner.Length > 0;
		//	return false;
		//}

		//public override TagCompound Save (Item item) {
		//	return new TagCompound {
		//		{"originalOwner", originalOwner}
		//	};
		//}

		//public override void OnCraft (Item item, Recipe recipe) {
		//	if (item.maxStack == 1)
		//		originalOwner = Main.LocalPlayer.name;
		//}

		//public override void NetSend (Item item, BinaryWriter writer) {
		//	writer.Write (originalOwner);
		//	writer.Write (ascending);
		//}

		//public override void NetReceive (Item item, BinaryReader reader) {
		//	originalOwner = reader.ReadString ();
		//	ascending = reader.ReadByte ();
		//}
	}
}