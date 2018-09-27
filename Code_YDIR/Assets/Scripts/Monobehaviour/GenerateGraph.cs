using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGraph : MonoBehaviour {
	[SerializeField] LayerMask terrainLayer;
	[SerializeField] GameObject tilePrefab;
	GameObject terrainContainer;
	[SerializeField] bool drawOverlay = false;

	void Awake () {
		Initialize ();
	}

	void Start () {
		Generate ();
	}

	void Initialize () {
		terrainContainer = transform.parent.Find ("Terrain").gameObject;
	}

	void Generate () {
		TestNodes (GetPossibleNodes (GetTerrainMembers ()));
		ConnectGraph ();

		if (drawOverlay)
			DebugDrawOverlay ();
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

	Vector3[] GetPossibleNodes (GameObject[] terrainMembers) {
		List<Vector3> possibleNodes = new List<Vector3> ();

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

					possibleNodes.Add (new Vector3 (nodeX, nodeY, nodeZ));
				}
			}
		}
		return possibleNodes.ToArray ();
	}

	void TestNodes (Vector3[] possibleNodes) {
		foreach (Vector3 testNode in possibleNodes) {
			Vector3 yOffset = new Vector3 (0f, 1f, 0f);

			RaycastHit hit;
			if (!Physics.Raycast (testNode + yOffset, Vector3.down, out hit, 10f, terrainLayer))
				continue;

			if (!TestBounds (testNode + yOffset))
				continue;

			Node node = new Node (new Vector3 (testNode.x, hit.point.y, testNode.z));
			NodeList.nodeList.Add (node);
		}
	}

	bool TestBounds (Vector3 testNode) {
		Vector3[] boundsTests = new Vector3[4];

		boundsTests [0] = testNode + new Vector3 (0.5f, 0f, 0.5f);
		boundsTests [1] = testNode + new Vector3 (-0.5f, 0f, 0.5f);
		boundsTests [2] = testNode + new Vector3 (-0.5f, 0f, -0.5f);
		boundsTests [3] = testNode + new Vector3 (0.5f, 0f, -0.5f);

		foreach (Vector3 bound in boundsTests) {
			if (!Physics.Raycast (bound, Vector3.down, 10f, terrainLayer))
				return false;
		}
		return true;
	}

	void DebugDrawOverlay () {		
		foreach (Node n in NodeList.nodeList) {
			Vector3 yOffset = new Vector3 (0f, 1f, 0f);
			RaycastHit hit;
			if (!Physics.Raycast (n.GetWorldPosition () + yOffset, Vector3.down, out hit, 10f, terrainLayer))
				continue;

			Vector3 gyOffset = new Vector3 (0f, 0.01f, 0f);
			GameObject tile = Instantiate (tilePrefab, n.GetWorldPosition () + gyOffset, Quaternion.identity);
			tile.transform.rotation = Quaternion.FromToRotation (tile.transform.up, hit.normal) * tile.transform.rotation;
			tile.name = n.GetWorldPosition ().ToString () + " " + n.GetConnections ().Length;
		}
	}

	void ConnectGraph () {
		foreach (Node n in NodeList.nodeList) {
			GenerateConnections (n);
		}
	}

	void GenerateConnections (Node node) {
		Vector3 nodePosition = node.GetWorldPosition ();

		foreach (Node n in NodeList.nodeList) {
			Vector3 nPosition = n.GetWorldPosition ();
			float yDiff = Mathf.Abs (nodePosition.y - nPosition.y);
			if (yDiff > 0.5f)
				continue;

			if (nPosition.x == nodePosition.x && nPosition.z == nodePosition.z + 1)
				node.AddConnection (n);
			if (nPosition.x == nodePosition.x + 1 && nPosition.z == nodePosition.z + 1)
				node.AddConnection (n);
			if (nPosition.x == nodePosition.x + 1 && nPosition.z == nodePosition.z)
				node.AddConnection (n);
			if (nPosition.x == nodePosition.x + 1 && nPosition.z == nodePosition.z - 1)
				node.AddConnection (n);
			if (nPosition.x == nodePosition.x && nPosition.z == nodePosition.z - 1)
				node.AddConnection (n);
			if (nPosition.x == nodePosition.x - 1 && nPosition.z == nodePosition.z - 1)
				node.AddConnection (n);
			if (nPosition.x == nodePosition.x - 1 && nPosition.z == nodePosition.z)
				node.AddConnection (n);
			if (nPosition.x == nodePosition.x - 1 && nPosition.z == nodePosition.z + 1)
				node.AddConnection (n);
		}
	}
}
