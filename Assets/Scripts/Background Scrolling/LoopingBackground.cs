using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LoopingBackground : MonoBehaviour {

	private float width;
	private float height;

	// Initialization
	void Start () {
		BoxCollider2D collider = GetComponent<BoxCollider2D> ();
		width = collider.size.x * transform.localScale.x;
		height = collider.size.y * transform.localScale.y;
	}
	
	// Called once per frame
	void Update () {
		if (transform.position.x < -width) {
			transform.position += new Vector3 (2f * width, 0, 0);
		} else if (transform.position.x > width) {
			transform.position += new Vector3 (-2f * width, 0, 0);
		} else if (transform.position.y < -height) {
			transform.position += new Vector3 (2f * height, 0, 0);
		} else if (transform.position.y > height) {
			transform.position += new Vector3 (-2f * height, 0, 0);
		}
	}
}
