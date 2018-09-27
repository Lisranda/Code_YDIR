using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
	Vector3 worldPosition;
	List<Node> connections = new List<Node>();

	public Node (Vector3 worldPosition) {
		this.worldPosition = worldPosition;
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
}
