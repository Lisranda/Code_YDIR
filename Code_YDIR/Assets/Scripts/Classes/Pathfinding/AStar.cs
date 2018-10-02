using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding {	
	public static class AStar {
		public static List<Node> FindPath (Node start, Node goal) {
			HeapMin<Node> openSet = new HeapMin<Node> ();
			HashSet<Node> closedSet = new HashSet<Node> ();
			
			openSet.Add (start);
			
			while (openSet.Count > 0) {
				Node current = openSet.Remove ();
				closedSet.Add (current);
				
				if (current == goal) {
					return RetracePath (start, goal);
				}
				
				foreach (Node n in current.GetConnections ()) {
					if (closedSet.Contains (n) || !n.IsWalkable ())
						continue;
					
					int newCost = current.gCost + GetDistance (current, n);
					bool inOpen = openSet.Contains (n);
					if (newCost < n.gCost || !inOpen) {
						n.gCost = newCost;
						n.hCost = GetDistance (n, goal);
						n.gParent = current;
						
						if (!inOpen)
							openSet.Add (n);
					}
				}
			}
			return null;
		}
		
		static int GetDistance (Node a, Node b) {
			return Mathf.CeilToInt(Mathf.Abs (Vector3.Distance (a.GetWorldPosition (), b.GetWorldPosition ())));
		}
		
		static List<Node> RetracePath (Node start, Node end) {
			List<Node> path = new List<Node> ();
			Node current = end;
			
			while (current != start) {
				path.Add (current);
				current = current.gParent;
			}
			path.Reverse ();
			return path;
		}
	}
}
