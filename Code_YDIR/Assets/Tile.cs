using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	[SerializeField] Tile[] neighbors = new Tile[4];
}
