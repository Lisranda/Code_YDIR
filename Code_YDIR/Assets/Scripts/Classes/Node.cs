using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable {
	Graph graph;
	Vector3 worldPosition;
	List<Node> connections = new List<Node>();
	bool isWalkable;
	public float hCost;
	public float gCost;
	public int gHops;
	public Node gParent;

	public float fCost { get { return gCost + hCost; } }

	public int CompareTo (System.Object obj) {
		Node node = (Node)obj;
		if (fCost == node.fCost)
			return (int)(hCost - node.hCost);
		return (int)(fCost - node.fCost);
	}

	public Node (Graph graph, Vector3 worldPosition, bool isWalkable = true) {
		this.graph = graph;
		this.worldPosition = worldPosition;
		this.isWalkable = isWalkable;
	}

	public Vector3 GetWorldPosition () {
		return worldPosition;
	}

	public Node[] GetConnections () {
		return connections.ToArray ();
	}

	public bool IsWalkable () {
		return isWalkable;
	}

	public void CheckWalkable () {
		isWalkable = true;
		Vector3 gyOffset = new Vector3 (0f, 0.5f, 0f);
//		if (Physics.Raycast (worldPosition, Vector3.up, 1f))
//			isWalkable = false;

		if (Physics.CheckCapsule (worldPosition + gyOffset, worldPosition + Vector3.up, 0.35f))
			isWalkable = false;
	}

	public void GenerateConnections () {
		connections.Clear ();
		foreach (Node n in graph.nodeList) {
			Vector3 nPosition = n.GetWorldPosition ();
			Vector3 direction = nPosition - worldPosition;
			float yDiff = Mathf.Abs (worldPosition.y - nPosition.y);
			if (yDiff > 0.5f)
				continue;

			if (!((nPosition.x == worldPosition.x && nPosition.z == worldPosition.z + 1f) ||
				(nPosition.x == worldPosition.x + 1f && nPosition.z == worldPosition.z + 1f) ||
				(nPosition.x == worldPosition.x + 1f && nPosition.z == worldPosition.z) ||
				(nPosition.x == worldPosition.x + 1f && nPosition.z == worldPosition.z - 1f) ||
				(nPosition.x == worldPosition.x && nPosition.z == worldPosition.z - 1f) ||
				(nPosition.x == worldPosition.x - 1f && nPosition.z == worldPosition.z - 1f) ||
				(nPosition.x == worldPosition.x - 1f && nPosition.z == worldPosition.z) ||
				(nPosition.x == worldPosition.x - 1f && nPosition.z == worldPosition.z + 1f))) {
				continue;
			}

			RaycastHit hit;
			if (Physics.Raycast (worldPosition, direction, out hit, 1f)) {
				if (hit.transform.gameObject.GetComponent<Obstacle> () != null)
					continue;
			}

			connections.Add (n);
		}
	}
}
