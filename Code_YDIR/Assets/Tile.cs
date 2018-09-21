using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {
	public static List<Tile> tileList;

	Vector3 coords;

	public Tile (Vector3 coords){
		tileList.Add (this);
		this.coords = coords;
	}

	public Vector3 GetCoords () {
		return coords;
	}
}
