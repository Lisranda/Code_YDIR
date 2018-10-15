using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Graph))]
public class Overlay : MonoBehaviour {
	[SerializeField] bool drawDebugOverlay;
	Graph graph;
	LayerMask terrainLayer;
	GameObject tilePrefab;
	GameObject debugOverlay;

	void Awake () {
		graph = FindObjectOfType<Graph> ();
		terrainLayer = graph.terrainLayer;
		tilePrefab = graph.tilePrefab;
	}

	void Update () {
		DrawDebugOverlay ();
	}

	public GameObject DrawOverlay (Node node, Color color) {
		RaycastHit hit;
		if (!Physics.Raycast (node.GetWorldPosition () + Vector3.up, Vector3.down, out hit, 10f, terrainLayer))
			return null;

		Vector3 gyOffset = new Vector3 (0f, 0.01f, 0f);
		GameObject tile = Instantiate (tilePrefab, node.GetWorldPosition () + gyOffset, Quaternion.identity);
		tile.transform.rotation = Quaternion.FromToRotation (tile.transform.up, hit.normal) * tile.transform.rotation;
		tile.name = node.GetWorldPosition ().ToString () + " " + node.GetConnections ().Length;

		tile.GetComponent<TileOverlay> ().SetOriginalColor (new Color (0.8f, 0.5f, 0.5f, 0.25f));
		if (node.IsWalkable ())
			tile.GetComponent<TileOverlay> ().SetOriginalColor (color);
		return tile;
	}

	void DrawDebugOverlay () {
		Destroy (debugOverlay);
		if (!drawDebugOverlay)
			return;

		debugOverlay = new GameObject ("Debug Overlay");
		debugOverlay.transform.SetParent (this.transform);

		foreach (Node n in graph.nodeList) {
			RaycastHit hit;
			if (!Physics.Raycast (n.GetWorldPosition () + Vector3.up, Vector3.down, out hit, 10f, terrainLayer))
				continue;

			Vector3 gyOffset = new Vector3 (0f, 0.01f, 0f);
			GameObject tile = Instantiate (tilePrefab, n.GetWorldPosition () + gyOffset, Quaternion.identity);
			tile.transform.SetParent (debugOverlay.transform);
			tile.transform.rotation = Quaternion.FromToRotation (tile.transform.up, hit.normal) * tile.transform.rotation;
			tile.name = n.GetWorldPosition ().ToString () + " " + n.GetConnections ().Length;

			tile.GetComponent<MeshRenderer> ().material.color = new Color (0.8f, 0.5f, 0.5f, 0.25f);
			if (n.IsWalkable ())
				tile.GetComponent<MeshRenderer> ().material.color = new Color (0.2f, 0.5f, 0.8f, 0.25f);
		}
	}
}
