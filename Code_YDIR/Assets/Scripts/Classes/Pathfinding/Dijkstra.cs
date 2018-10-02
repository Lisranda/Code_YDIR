using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding {	
	public static class Dijkstra {
		static void Explore (Node start, int range) {
			HeapMin<Node> openSet = new HeapMin<Node> ();
			HashSet<Node> closedSet = new HashSet<Node> ();

			openSet.Add (start);

			while (openSet.Count > 0) {
				Node current = openSet.Remove ();
				closedSet.Add (current);

				if (!(current.gCost < range))
					continue;

				foreach (Node n in current.GetConnections ()) {
					if (closedSet.Contains (n) || !n.IsWalkable ())
						continue;

					int newCost = current.gCost + GetDistance (current, n);
					int newHops = current.gHops + 1;
					bool inOpen = openSet.Contains (n);
					if (newCost < n.gCost || !inOpen) {
						n.gCost = newCost;
						n.gHops = newHops;
						n.gParent = current;

						if (!inOpen)
							openSet.Add (n);
					}
				}
			}
		}

		static int GetDistance (Node a, Node b) {
			Vector3 aP = new Vector3 (a.GetWorldPosition ().x, 0f, a.GetWorldPosition ().z);
			Vector3 bP = new Vector3 (b.GetWorldPosition ().x, 0f, b.GetWorldPosition ().z);
			return Mathf.CeilToInt (Mathf.Abs (Vector3.Distance (aP, bP)));
		}

		public static List<Node> GetNodesInRange (Node start, int range) {
			Reset ();
			List<Node> inRange = new List<Node> ();
			Explore (start, range);
			foreach (Node n in Graph.nodeList) {
				if (n.gCost > 0 && n.gCost <= range)
					inRange.Add (n);
			}
			return inRange;
		}

		public static List<Node> GetPath (Node start, Node end) {
			List<Node> path = new List<Node> ();
			Node current = end;

			while (current != start) {
				path.Add (current);
				current = current.gParent;
			}
			path.Reverse ();
			return path;
		}

		static void Reset () {
			foreach (Node n in Graph.nodeList) {
				n.gCost = 0;
				n.gHops = 0;
				n.hCost = 0;
				n.gParent = null;
			}
		}
	}
}