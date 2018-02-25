using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WeightedEnemyPhysics : MonoBehaviour {

	// Settings/properties, set by enemy controller scripts
	//[SerializeField]
	private float drag = 0.4f;

	// Other variables
	[HideInInspector]
	public float maxSpeed;
	[HideInInspector]
	public Vector2 velocity, acceleration;

	// Object references
	private Rigidbody2D rb;


	// Initialize
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 pos = new Vector2 (transform.position.x, transform.position.y);

		/* if (acceleration.magnitude == 0f) {
			acceleration = velocity * drag;
		} */

		velocity += acceleration * Time.deltaTime;
		velocity = Vector2.ClampMagnitude (velocity, maxSpeed * Time.deltaTime);
		rb.MovePosition (pos + velocity);
	}
}
