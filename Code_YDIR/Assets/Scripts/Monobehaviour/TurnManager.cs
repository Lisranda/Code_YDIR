using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {
	public Graph graph;
	public int turnCounter = 1;
	public List<Pawn> pawns = new List<Pawn> ();
	public Pawn activePawn;
	List<GameObject> overlays = new List<GameObject> ();

	void Update () {
		if (Input.GetKeyDown ("space"))
			EndTurn ();

		if (Input.GetMouseButtonDown (0)) {
			Click ();
		}
	}

	void LateUpdate () {
		if (activePawn != null) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (!Physics.Raycast (ray, out hit))
				return;
			if (hit.transform.GetComponent<TileOverlay> () == null)
				return;
			hit.transform.GetComponent<TileOverlay> ().SetColor (Color.red);
			DrawPath (graph.GetNodeFromWorldSpace (activePawn.transform.position), graph.GetNodeFromWorldSpace (hit.transform.position));
		}
	}

	void Click () {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (!Physics.Raycast (ray, out hit))
			return;
		if (hit.transform.GetComponent<Pawn> () == null)
			return;
		if (hit.transform.GetComponent<Pawn> () == activePawn)
			return;
		SelectPawn (hit.transform.GetComponent<Pawn> ());
	}

	void DrawPath (Node start, Node end) {
		List<Node> path = Pathfinding.Dijkstra.GetPath (start, end);
		foreach (Node n in path) {
			Debug.DrawLine (n.gParent.GetWorldPosition (), n.GetWorldPosition ());
		}
	}

	void SelectPawn (Pawn p) {
		foreach (GameObject go in overlays) {
			Destroy (go);
		}
		overlays.Clear ();

		activePawn = p;
		Camera.main.transform.position = new Vector3 (p.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
		List<Node> inRange = Pathfinding.Dijkstra.GetNodesInRange (graph.GetNodeFromWorldSpace (p.transform.position), p.movementRange * p.numActions);
		foreach (Node n in inRange) {
			Color color = n.gCost <= p.movementRange ? new Color (0.2f, 0.5f, 0.8f, 0.25f) : new Color (0.2f, 0.8f, 0.5f, 0.25f);
			overlays.Add (graph.DrawOverlay (n, color));
		}
	}

	void EndTurn () {
		foreach (Pawn p in pawns) {
			p.numActions = 2;
		}
		turnCounter++;
		Debug.Log ("Turn: " + turnCounter);
	}
}
