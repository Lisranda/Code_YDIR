using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable {
	Vector3 worldPosition;
	List<Node> connections = new List<Node>();
	bool isWalkable;
	public int hCost;
	public int gCost;
	public int gHops;
	public Node gParent;

	public int fCost { get { return gCost + hCost; } }

	public int CompareTo (System.Object obj) {
		Node node = (Node)obj;
		if (fCost == node.fCost)
			return hCost - node.hCost;
		return fCost - node.fCost;
	}

	public Node (Vector3 worldPosition, bool isWalkable = true) {
		this.worldPosition = worldPosition;
		this.isWalkable = isWalkable;
	}

	public Vector3 GetWorldPosition () {
		return worldPosition;
	}

	public void AddConnection (Node connectedNode) {
		connections.Add (connectedNode);
	}

	public Node[] GetConnections () {
		return connections.ToArray ();
	}

	public bool IsWalkable () {
		if (!isWalkable)
			return false;
		return true;
	}

	public void CheckWalkable () {
		Vector3 gyOffset = new Vector3 (0f, 0.5f, 0f);
//		if (Physics.Raycast (worldPosition, Vector3.up, 1f))
//			isWalkable = false;

		if (Physics.CheckCapsule (worldPosition + gyOffset, worldPosition + Vector3.up, 0.35f))
			isWalkable = false;
	}

	public void GenerateConnections () {
		connections.Clear ();
		foreach (Node n in Graph.nodeList) {
			Vector3 nPosition = n.GetWorldPosition ();
			Vector3 direction = nPosition - worldPosition;
			float yDiff = Mathf.Abs (worldPosition.y - nPosition.y);
			if (yDiff > 0.5f)
				continue;

			if (nPosition.x == worldPosition.x && nPosition.z == worldPosition.z + 1f) {
				RaycastHit hit;
				if (Physics.Raycast (worldPosition, direction, out hit, 1f)) {
					if (hit.transform.gameObject.GetComponent<Obstacle> () != null)
						continue;
				}
				AddConnection (n);
			}
			if (nPosition.x == worldPosition.x + 1f && nPosition.z == worldPosition.z + 1f) {
				RaycastHit hit;
				if (Physics.Raycast (worldPosition, direction, out hit, 1f)) {
					if (hit.transform.gameObject.GetComponent<Obstacle> () != null)
						continue;
				}
				AddConnection (n);
			}
			if (nPosition.x == worldPosition.x + 1f && nPosition.z == worldPosition.z) {
				RaycastHit hit;
				if (Physics.Raycast (worldPosition, direction, out hit, 1f)) {
					if (hit.transform.gameObject.GetComponent<Obstacle> () != null)
						continue;
				}
				AddConnection (n);
			}
			if (nPosition.x == worldPosition.x + 1f && nPosition.z == worldPosition.z - 1f) {
				RaycastHit hit;
				if (Physics.Raycast (worldPosition, direction, out hit, 1f)) {
					if (hit.transform.gameObject.GetComponent<Obstacle> () != null)
						continue;
				}
				AddConnection (n);
			}
			if (nPosition.x == worldPosition.x && nPosition.z == worldPosition.z - 1f) {
				RaycastHit hit;
				if (Physics.Raycast (worldPosition, direction, out hit, 1f)) {
					if (hit.transform.gameObject.GetComponent<Obstacle> () != null)
						continue;
				}
				AddConnection (n);
			}
			if (nPosition.x == worldPosition.x - 1f && nPosition.z == worldPosition.z - 1f) {
				RaycastHit hit;
				if (Physics.Raycast (worldPosition, direction, out hit, 1f)) {
					if (hit.transform.gameObject.GetComponent<Obstacle> () != null)
						continue;
				}
				AddConnection (n);
			}
			if (nPosition.x == worldPosition.x - 1f && nPosition.z == worldPosition.z) {
				RaycastHit hit;
				if (Physics.Raycast (worldPosition, direction, out hit, 1f)) {
					if (hit.transform.gameObject.GetComponent<Obstacle> () != null)
						continue;
				}
				AddConnection (n);
			}
			if (nPosition.x == worldPosition.x - 1f && nPosition.z == worldPosition.z + 1f) {
				RaycastHit hit;
				if (Physics.Raycast (worldPosition, direction, out hit, 1f)) {
					if (hit.transform.gameObject.GetComponent<Obstacle> () != null)
						continue;
				}
				AddConnection (n);
			}
		}
	}
}
