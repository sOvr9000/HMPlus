using System.IO;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace HMPlus {
	public class HMPlusWorld : ModWorld {
		private const int saveVersion = 0;

		public static bool downedMechoraum = false;
		public static bool downedTheIllusion = false;

		public static int dungeonSpiritsKilled = 0;

		public override void Initialize () {
			downedMechoraum = false;
			dungeonSpiritsKilled = 0;
		}

		public override TagCompound Save () {
			var downed = new List<string> ();
			if (downedMechoraum) downed.Add ("mechoraum");

			return new TagCompound {
				{"downed", downed},
				{"dungeonSpiritsKilled", dungeonSpiritsKilled}
			};
		}

		public override void Load (TagCompound tag) {
			var downed = tag.GetList<string> ("downed");
			downedMechoraum = downed.Contains ("mechoraum");
			dungeonSpiritsKilled = tag.GetInt ("dungeonSpiritsKilled");
		}

		public override void LoadLegacy (BinaryReader reader) {
			int loadVersion = reader.ReadInt32 ();
			if (loadVersion == 0) {
				BitsByte flags = reader.ReadByte ();
				downedMechoraum = flags [0];
			} else {
				ErrorLogger.Log ("HMPlusMod: Unknown loadVersion: " + loadVersion);
			}
		}

		public override void ModifyWorldGenTasks (List<GenPass> tasks, ref float totalWeight) {
			int ShiniesIndex = tasks.FindIndex (genpass => genpass.Name.Equals ("Shinies"));
			if (ShiniesIndex != -1) {
				tasks.Insert (ShiniesIndex + 1, new PassLegacy ("Example Mod Ores", delegate (GenerationProgress progress) {
					progress.Message = "Example Mod Ores";

					for (int k = 0; k < (int) ((double) (Main.maxTilesX * Main.maxTilesY) * 6E-05); k++) {
						WorldGen.TileRunner (WorldGen.genRand.Next (0, Main.maxTilesX), WorldGen.genRand.Next ((int) WorldGen.worldSurfaceLow, Main.maxTilesY), (double) WorldGen.genRand.Next (3, 6), WorldGen.genRand.Next (2, 6), mod.TileType ("ExampleBlock"), false, 0f, 0f, false, true);
					}
				}));
			}

			int TrapsIndex = tasks.FindIndex (genpass => genpass.Name.Equals ("Traps"));
			if (TrapsIndex != -1) {
				tasks.Insert (TrapsIndex + 1, new PassLegacy ("Example Mod Traps", delegate (GenerationProgress progress) {
					progress.Message = "Example Mod Traps";
					// Computers are fast, so WorldGen code sometimes looks stupid.
					// Here, we want to place a bunch of tiles in the world, so we just repeat until success. It might be useful to keep track of attempts and check for attempts > maxattempts so you don't have infinite loops. 
					// The WorldGen.PlaceTile method returns a bool, but it is useless. Instead, we check the tile after calling it and if it is the desired tile, we know we succeeded.
					for (int k = 0; k < (int) ((double) (Main.maxTilesX * Main.maxTilesY) * 6E-05); k++) {
						bool placeSuccessful = false;
						Tile tile;
						int tileToPlace = mod.TileType<Tiles.ExampleCutTileTile> ();
						while (!placeSuccessful) {
							int x = WorldGen.genRand.Next (0, Main.maxTilesX);
							int y = WorldGen.genRand.Next (0, Main.maxTilesY);
							WorldGen.PlaceTile (x, y, tileToPlace);
							tile = Main.tile [x, y];
							placeSuccessful = tile.active () && tile.type == tileToPlace;
						}
					}
				}));
			}

			int LivingTreesIndex = tasks.FindIndex (genpass => genpass.Name.Equals ("Living Trees"));
			if (LivingTreesIndex != -1) {
				tasks.Insert (LivingTreesIndex + 1, new PassLegacy ("Post Terrain", delegate (GenerationProgress progress) {
					progress.Message = "What is it Lassie, did Timmy fall down a well?";
					MakeWells ();
				}));
			}
		}

		public override void NetSend (BinaryWriter writer) {
			BitsByte flags = new BitsByte ();
			flags [0] = downedMechoraum;
			writer.Write (flags);
		}

		public override void NetReceive (BinaryReader reader) {
			BitsByte flags = reader.ReadByte ();
			downedMechoraum = flags [0];
		}
	}
}