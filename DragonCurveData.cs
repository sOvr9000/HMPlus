using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace HMPlus {
	public class DragonCurveData {
		public int currentTerm;
		public int iterations;
		public int numVertices;
		public double invNumVertices;
		public Vector2 [] vertices;
		public Vector2 startPoint;
		public Vector2 endPoint;

		public bool hasCompletedCycle = true;

		public DragonCurveData () {

		}

		public Vector2 GetVertex (int term) {
			if (term >= numVertices) {
				term = term % numVertices;
			}
			return vertices [term];
		}

		public Vector2 NextVertex () {
			if (++currentTerm >= numVertices - 1) {
				hasCompletedCycle = true;
				Projectiles.DragonTurtle.dragonCurveData = null;
				ErrorLogger.Log ("Reached end: currentTerm = " + currentTerm + " and numVertices = " + numVertices);
			} else {
				if (currentTerm >= 0) {
					hasCompletedCycle = false;
				}
			}
			return GetVertex (currentTerm);
		}

		public Vector2 [] NextVertices (int amount) {
			if (amount + currentTerm >= numVertices) amount = numVertices - 1 - currentTerm;
			if (amount == 0) {
				hasCompletedCycle = true;
				return new Vector2 [0];
			}
			Vector2 [] array = new Vector2 [amount];
			for (int n = 0; n < amount; n ++) {
				array [n] = NextVertex ();
			}
			return array;
		}

		public void ToBeginning () {
			hasCompletedCycle = true;
			currentTerm = -1;
		}

		/// <summary>
		/// Return whether the curve was successfully generated or not.
		/// </summary>
		/// <param name="vertices"></param>
		/// <returns></returns>
		public bool Generate (Vector2 startPoint, int iterations, double angle = 0d, int scale = 10) {
			if (iterations < 1) return false;
			if (scale == 0) return false;
			angle += MathHelper.PiOver4 * iterations;
			currentTerm = -1;
			this.iterations = iterations;
			numVertices = (int) System.Math.Pow (2, iterations) - 1;
			invNumVertices = 1d / numVertices;
			hasCompletedCycle = false;
			this.startPoint = startPoint;
			int x = (int) startPoint.X;
			int y = (int) startPoint.Y;
			int xi = (int) (scale * System.Math.Cos (angle));
			int yi = (int) (scale * System.Math.Sin (angle));

			vertices = new Vector2 [numVertices];
			vertices [0] = startPoint;

			int n = 0;
			while (++n < numVertices) {
				if ((((n & -n) << 1) & n) == 0) {
					// turn right
					int temp = yi;
					yi = -xi;
					xi = temp;
				} else {
					// turn left
					int temp = xi;
					xi = -yi;
					yi = temp;
				}
				x += xi;
				y -= yi;
				vertices [n] = new Vector2 (x, y);
			}

			endPoint = vertices [n - 1];

			ToBeginning ();

			return true;
		}
	}
}
