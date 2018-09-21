using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGridGenerator : MonoBehaviour {
	[SerializeField] LayerMask terrainLayer;

	void Start () {
		CreateGrid ();
		
	}

	void Update () {
		
	}

	void CreateGrid () {
		Vector3 startLocation = new Vector3 (0f, 25f, 0f);
		List<Vector3> toCheckLocations = new List<Vector3> ();
		List<Vector3> checkedLocations = new List<Vector3> ();
		toCheckLocations.Add (startLocation);

		while (toCheckLocations.Count > 0) {
			if (!Physics.Raycast (toCheckLocations [0], Vector3.down, 50f, terrainLayer)) {
				checkedLocations.Add (toCheckLocations [0]);
				toCheckLocations.Remove (toCheckLocations [0]);
				continue;
			}
			
			Tile tile = new Tile (toCheckLocations [0]);
						
			if (!checkedLocations.Contains (toCheckLocations [0] + Vector3.forward) && !toCheckLocations.Contains (toCheckLocations [0] + Vector3.forward))
				toCheckLocations.Add (toCheckLocations [0] + Vector3.forward);
			if (!checkedLocations.Contains (toCheckLocations [0] + Vector3.back) && !toCheckLocations.Contains (toCheckLocations [0] + Vector3.back))
				toCheckLocations.Add (toCheckLocations [0] + Vector3.back);
			if (!checkedLocations.Contains (toCheckLocations [0] + Vector3.right) && !toCheckLocations.Contains (toCheckLocations [0] + Vector3.right))
				toCheckLocations.Add (toCheckLocations [0] + Vector3.right);
			if (!checkedLocations.Contains (toCheckLocations [0] + Vector3.left) && !toCheckLocations.Contains (toCheckLocations [0] + Vector3.left))
				toCheckLocations.Add (toCheckLocations [0] + Vector3.left);
			
			checkedLocations.Add (toCheckLocations [0]);
			toCheckLocations.Remove (toCheckLocations [0]);
		}
	}
}
