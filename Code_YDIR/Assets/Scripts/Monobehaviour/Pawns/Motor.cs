using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour {
	public void MoveOnGraph (Node node) {
		transform.position = node.GetWorldPosition ();
		//TODO: Fire event to refresh if all nodes are walkable.
	}
}
