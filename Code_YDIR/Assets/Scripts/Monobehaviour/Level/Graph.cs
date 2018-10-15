using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {
	public LayerMask terrainLayer;
	public LayerMask tileGrid;
	public GameObject tilePrefab;
	GameObject terrainContainer;
	public List<Node> nodeList = new List<Node>();

	#region MONOBEHAVIOUR

	void Awake () {
		terrainContainer = FindObjectOfType<TerrainContainer> ().gameObject;
	}

	void Start () {
		GenerateGraph ();
	}

	#endregion

	#region UTILITY

	public Node GetNodeFromWorldSpace (Vector3 worldPosition) {
		foreach (Node n in nodeList) {
			if ((worldPosition.x >= n.GetWorldPosition ().x - 0.5f && worldPosition.x <= n.GetWorldPosition ().x + 0.5f)
			    && (worldPosition.z >= n.GetWorldPosition ().z - 0.5f && worldPosition.z <= n.GetWorldPosition ().z + 0.5f)
			    && (worldPosition.y >= n.GetWorldPosition ().y - 0.5f && worldPosition.y <= n.GetWorldPosition ().y + 0.5f))
				return n;
		}
		return null;
	}

	#endregion

	void GenerateGraph () {
		GenerateNodes (GetTerrainMembers ());
		ConnectGraph ();
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

					Node node = new Node (this, new Vector3 (testNode.x, hit.point.y, testNode.z));
					node.CheckWalkable ();
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
		
	void ConnectGraph () {
		foreach (Node n in nodeList) {
			n.GenerateConnections ();
		}
	}
}