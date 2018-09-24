using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOver : MonoBehaviour {
	[SerializeField] LayerMask tileLayer;

	void Start () {
		
	}

	void LateUpdate () {
		OverTile ();
	}

	void OverTile () {
		GameObject go = RayToTile ();

		if (go == null)
			return;

		go.GetComponent<Tile> ().MouseOver ();
	}

	GameObject RayToTile () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 100f, tileLayer)) {
			if (hit.transform.gameObject.GetComponent<Tile> () == null)
				return null;

			return hit.transform.gameObject;			
		}

		return null;
	}
}
