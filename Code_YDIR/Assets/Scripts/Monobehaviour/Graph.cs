using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {
	[SerializeField] LayerMask terrainLayer;
	[SerializeField] GameObject tilePrefab;
	GameObject terrainContainer;
	[SerializeField] bool drawOverlay = false;
	List<Node> nodeList = new List<Node>();

	void Awake () {
		terrainContainer = transform.parent.Find ("Terrain").gameObject;
	}

	void Start () {
		GenerateGraph ();
	}

	void GenerateGraph () {
		GenerateNodes (GetTerrainMembers ());
		ConnectGraph ();

		if (drawOverlay)
			DebugDrawOverlay ();
	}

	public Node GetNodeFromWorldSpace (Vector3 worldPosition) {
		foreach (Node n in nodeList) {
			if ((worldPosition.x >= n.GetWorldPosition ().x - 0.5f && worldPosition.x <= n.GetWorldPosition ().x + 0.5f)
			    && (worldPosition.z >= n.GetWorldPosition ().z - 0.5f && worldPosition.z <= n.GetWorldPosition ().z + 0.5f)
			    && (worldPosition.y >= n.GetWorldPosition ().y - 0.5f && worldPosition.y <= n.GetWorldPosition ().y + 0.5f))
				return n;
		}
		return null;
	}

	GameObject[] GetTerrainMembers () {
		List<GameObject> membersGO = new List<GameObject> ();
		Transform[] membersT = terrainContainer.GetComponentsInChildren<Transform> ();

		foreach (Transform t in membersT) {
			if (t.gameObject.GetComponent<MeshRenderer> () == null)
				continue;

			membersGO.Add (t.gameObject);
		}
		return membersGO.ToArray ();
	}

	void GenerateNodes (GameObject[] terrainMembers) {
		foreach (GameObject go in terrainMembers) {
			MeshRenderer render = go.GetComponent<MeshRenderer> ();

			float sizeX = render.bounds.size.x;
			float sizeY = render.bounds.size.y;
			float sizeZ = render.bounds.size.z;

			float posX = go.transform.position.x;
			float posY = go.transform.position.y;
			float posZ = go.transform.position.z;

			if (sizeX < 1 || sizeZ < 1)
				continue;

			for (int x = 0; x < (int)sizeX; x++) {
				for (int z = 0; z < (int)sizeZ; z++) {
					float nodeX = posX - (sizeX * 0.5f) + (float)x + 0.5f;
					float nodeY = posY + (sizeY * 0.5f);
					float nodeZ = posZ - (sizeZ * 0.5f) + (float)z + 0.5f;
					Vector3 testNode = new Vector3 (nodeX, nodeY, nodeZ);

					RaycastHit hit;
					if (!Physics.Raycast (testNode + Vector3.up, Vector3.down, out hit, 10f, terrainLayer))
						continue;

					if (hit.transform.gameObject != go)
						continue;

					if (!TestBounds (testNode + Vector3.up, go))
						continue;

					Node node = new Node (new Vector3 (testNode.x, hit.point.y, testNode.z), go);
					nodeList.Add (node);
				}
			}
		}
	}

	bool TestBounds (Vector3 testNode, GameObject testGameObject) {
		Vector3[] boundsTests = new Vector3[4];

		boundsTests [0] = testNode + new Vector3 (0.5f, 0f, 0.5f);
		boundsTests [1] = testNode + new Vector3 (-0.5f, 0f, 0.5f);
		boundsTests [2] = testNode + new Vector3 (-0.5f, 0f, -0.5f);
		boundsTests [3] = testNode + new Vector3 (0.5f, 0f, -0.5f);

		foreach (Vector3 bound in boundsTests) {
			RaycastHit hit;
			if (!Physics.Raycast (bound, Vector3.down, out hit, 10f, terrainLayer))
				return false;
//			if (hit.transform.gameObject != testGameObject)
//				return false;
		}
		return true;
	}

	void DebugDrawOverlay () {		
		foreach (Node n in nodeList) {
			RaycastHit hit;
			if (!Physics.Raycast (n.GetWorldPosition () + Vector3.up, Vector3.down, out hit, 10f, terrainLayer))
				continue;

			Vector3 gyOffset = new Vector3 (0f, 0.01f, 0f);
			GameObject tile = Instantiate (tilePrefab, n.GetWorldPosition () + gyOffset, Quaternion.identity);
			tile.transform.rotation = Quaternion.FromToRotation (tile.transform.up, hit.normal) * tile.transform.rotation;
			tile.name = n.GetWorldPosition ().ToString () + " " + n.GetConnections ().Length;
		}
	}
		
	void ConnectGraph () {
		foreach (Node n in nodeList) {
			GenerateConnections (n);
		}
	}

	void GenerateConnections (Node node) {
		Vector3 nodePosition = node.GetWorldPosition ();

		foreach (Node n in nodeList) {
			Vector3 nPosition = n.GetWorldPosition ();
			float yDiff = Mathf.Abs (nodePosition.y - nPosition.y);
			if (yDiff > 0.5f)
				continue;

			if (nPosition.x == nodePosition.x && nPosition.z == nodePosition.z + 1f)
				node.AddConnection (n);
			if (nPosition.x == nodePosition.x + 1f && nPosition.z == nodePosition.z + 1f)
				node.AddConnection (n);
			if (nPosition.x == nodePosition.x + 1f && nPosition.z == nodePosition.z)
				node.AddConnection (n);
			if (nPosition.x == nodePosition.x + 1f && nPosition.z == nodePosition.z - 1f)
				node.AddConnection (n);
			if (nPosition.x == nodePosition.x && nPosition.z == nodePosition.z - 1f)
				node.AddConnection (n);
			if (nPosition.x == nodePosition.x - 1f && nPosition.z == nodePosition.z - 1f)
				node.AddConnection (n);
			if (nPosition.x == nodePosition.x - 1f && nPosition.z == nodePosition.z)
				node.AddConnection (n);
			if (nPosition.x == nodePosition.x - 1f && nPosition.z == nodePosition.z + 1f)
				node.AddConnection (n);
		}
	}
}
