using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {
	Graph graph;
	Overlay overlay;
	public int turnCounter = 1;
	public List<Pawn> pawns = new List<Pawn> ();
	public Pawn activePawn;
	List<GameObject> overlays = new List<GameObject> ();

	void Awake () {
		graph = FindObjectOfType<Graph> ();
		overlay = FindObjectOfType<Overlay> ();
	}

	void Update () {
		if (Input.GetKeyDown ("space"))
			EndTurn ();

		if (Input.GetMouseButtonDown (0)) {
			Click ();
		}

		if (Input.GetMouseButtonDown (1)) {
			RightClick ();
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

	void RightClick () {
		if (activePawn == null)
			return;
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (!Physics.Raycast (ray, out hit))
			return;
		if (hit.transform.GetComponent<TileOverlay> () == null)
			return;
		Node n = graph.GetNodeFromWorldSpace (hit.transform.position);
		MovePawn (activePawn, n);
		if (n.gCost <= activePawn.movementRange)
			activePawn.numActions--;
		else
			activePawn.numActions -= 2;
		SelectPawn (activePawn);
	}

	void MovePawn (Pawn pawn, Node node) {
		pawn.transform.position = node.GetWorldPosition ();
		foreach (Node n in graph.nodeList) {
			n.CheckWalkable ();
		}
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
		List<Node> inRange = Pathfinding.Dijkstra.GetNodesInRange (graph, graph.GetNodeFromWorldSpace (p.transform.position), p.movementRange * p.numActions);
		HighlightNodes (p, inRange);
	}

	void HighlightNodes (Pawn p, List<Node> inRange) {		
		foreach (Node n in inRange) {
			Color color;
			if (p.numActions == 2) {
				color = n.gCost <= p.movementRange ? new Color (0.2f, 0.5f, 0.8f, 0.25f) : new Color (0.2f, 0.8f, 0.5f, 0.25f);
			} else {
				color = new Color (0.2f, 0.8f, 0.5f, 0.25f);
			}
			overlays.Add (overlay.DrawOverlay (n, color));
		}
	}

	void EndTurn () {
		foreach (Pawn p in pawns) {
			p.numActions = 2;
		}
		foreach (GameObject go in overlays) {
			Destroy (go);
		}
		overlays.Clear ();
		SelectPawn (activePawn);
		turnCounter++;
		Debug.Log ("Turn: " + turnCounter);
	}
}
