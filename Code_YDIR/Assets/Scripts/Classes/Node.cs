using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
	Vector3 worldPosition;
	List<Node> connections = new List<Node>();
	bool isWalkable;

	public Node (Vector3 worldPosition) {
		this.worldPosition = worldPosition;
		this.isWalkable = true;
	}

	public Node (Vector3 worldPosition, bool isWalkable) {
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

	public bool CheckWalkable () {
		if (!isWalkable)
			return false;
		return true;
	}
}
