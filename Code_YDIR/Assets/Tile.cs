using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	[SerializeField] LayerMask tileLayer;
	[SerializeField] GameObject[] neighbors = new GameObject[4];

	void Update () {
		ResetMouseOver ();
	}

	public void SetNeighbor (int arrayIndex, GameObject neighbor) {
		neighbors [arrayIndex] = neighbor;
	}

	public void MouseOver () {
		gameObject.GetComponent<MeshRenderer> ().enabled = true;
	}

	void ResetMouseOver () {
		if (gameObject.GetComponent<MeshRenderer> ().enabled == true) {
			gameObject.GetComponent<MeshRenderer> ().enabled = false;
		}
	}
}
