using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
	void Update () {
		if (Input.GetKeyDown ("space")) {
			Debug.Log ("Space");
		}

		if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("L Click");
		}

		if (Input.GetMouseButtonDown (1)) {
			Debug.Log ("R Click");
		}
	}
}
