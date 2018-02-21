using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FollowerEnemy : MonoBehaviour {

	// Settings/properties:
	private float speed = 3f;
	private int maxHP = 2;

	private float followRadius = 1f;

	// Object references
	private Rigidbody2D rb;
	public GameObject objToFollow;

	// Other variables
	private int HP;


	// Initialize
	void Start () {
		rb = GetComponent<Rigidbody2D> ();

		HP = maxHP;
	}

	// Called every frame
	void Update () {
		Vector2 direction = new Vector2 ();
		Vector2 pos = new Vector2 (transform.position.x, transform.position.y);

		// Get position of object to follow
		Vector2 followPos = new Vector2 (objToFollow.transform.position.x, objToFollow.transform.position.y);

		// Decide what direction to move in
		direction = followPos - pos;
		if (direction.magnitude < followRadius) {
			return;
		}

		// Normalize the velocity and set to desired speed
		Vector2 velocity = direction.normalized * speed * Time.deltaTime;
		rb.MovePosition (pos + velocity);
	}

	// Called when damage is taken
	private void gotHit(int dmg) {
		HP -= dmg;

		if (HP <= 0) {
			// DO SOMETHING ELSE WHEN ENEMY IS DESTROYED? - TODO
			Destroy (gameObject);
		}
	}
}
