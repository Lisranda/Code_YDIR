using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileOverlay : MonoBehaviour {
	MeshRenderer render;
	Color originalColor;

	void Awake () {
		render = GetComponent<MeshRenderer> ();
	}

	void OnEnable () {
		originalColor = render.material.color;
	}

	void Update () {
		SetColor (originalColor);
	}

	public void SetColor (Color color) {
		if (render.material.color != color)
			render.material.color = color;
	}

	public void SetOriginalColor (Color color) {
		originalColor = color;
	}
}
