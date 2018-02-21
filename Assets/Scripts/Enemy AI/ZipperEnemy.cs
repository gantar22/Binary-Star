using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ZipperEnemy : MonoBehaviour {

	// Settings/properties:
	[HideInInspector]
	public float speed = 3.5f;
	private int maxHP = 3;

	private float minPause = 0.8f;
	private float maxPause = 2f;
	private float minZip = 0.5f;
	private float maxZip = 1f;

	// Object references
	private Rigidbody2D rb;

	// Other variables
	private int HP;

	private bool paused;
	private float delay;
	private Vector2 fixedDirection;

	// Initialize
	void Start () {
		rb = GetComponent<Rigidbody2D> ();

		HP = maxHP;
	}

	// Called every frame
	void Update () {
		Vector2 direction = new Vector2 ();
		Vector2 pos = new Vector2 (transform.position.x, transform.position.y);

		// Get player position - TODO
		Vector2 playerPos = new Vector2 (0f, 0f);

		// Decide what direction to move in
		direction = playerPos - pos;
		controlDelay (direction);
		direction = fixedDirection;

		if (!paused) {
			// Normalize the velocity and set to desired speed
			Vector2 velocity = direction.normalized * speed * Time.deltaTime;
			rb.MovePosition (pos + velocity);
		}
	}

	private void controlDelay(Vector2 newDirection) {
		if (delay > 0) {
			delay -= Time.deltaTime;
		} else if (paused) {
			paused = !paused;
			fixedDirection = newDirection;
			delay = Random.Range (minZip, maxZip);
		} else {
			paused = !paused;
			delay = Random.Range (minPause, maxPause);
		}
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
