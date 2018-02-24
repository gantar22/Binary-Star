using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WeightedEnemyPhysics : MonoBehaviour {

	// Settings/properties, set by enemy controller scripts
	[HideInInspector]
	public float maxSpeed;
	[HideInInspector]
	public Vector2 velocity, acceleration;

	// Other variables
	//private int HP; // Save HP here? Or in separate "EnemyHP" script?

	// Object references
	private Rigidbody2D rb;


	// Initialize
	void Start () {
		rb = GetComponent<Rigidbody2D> ();

		//HP = maxHP;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 pos = new Vector2 (transform.position.x, transform.position.y);


		velocity += acceleration * Time.deltaTime;
		velocity = Vector2.ClampMagnitude (velocity, maxSpeed * Time.deltaTime);
		rb.MovePosition (pos + velocity);
		print ("Actual speed: " + (velocity / Time.deltaTime).magnitude);
	}
}
