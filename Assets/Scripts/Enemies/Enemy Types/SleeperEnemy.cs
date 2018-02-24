using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SleeperEnemy : MonoBehaviour {

	// Settings/properties:
	[HideInInspector]
	public float speed = 4.5f;
	private float chaseSpeed = 7f;

	private float wakeRadius = 5f;
	private float frequency = 1f;
	private float inwardBoost = 0.4f;

	// Other variables

	private bool chasing;

	// Object references
	private Rigidbody2D rb;


	// Initialize
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		
		chasing = false;
	}

	// Called every frame
	void Update () {
		Vector2 direction = new Vector2 ();
		Vector2 pos = new Vector2 (transform.position.x, transform.position.y);

		// Get player position - TODO
		//Vector2 targetPos = new Vector2 (0f, 0f);
		Vector3 mousePos3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 targetPos = new Vector2 (mousePos3.x, mousePos3.y);

		// Decide what direction to move in
		direction = targetPos - pos;
		Vector2 perp = new Vector2 (-direction.y, direction.x);

		float period = 1f / frequency;
		float radians = (Time.time % (period)) / period * 2 * Mathf.PI;

		checkForChase (targetPos, pos);
		if (!chasing) {
			direction = perp * Mathf.Cos (radians) + direction * Mathf.Sin (radians) + direction * inwardBoost;
		}

		// Normalize the velocity and set to desired speed
		Vector2 velocity = direction.normalized * speed * Time.deltaTime;
		rb.MovePosition (pos + velocity);
	}

	// Check if the player is within the wake radius
	private void checkForChase(Vector2 targetPos, Vector2 pos) {
		if (chasing) {
			return;
		}
		if ((targetPos - pos).magnitude <= wakeRadius) {
			chasing = true;
			speed = chaseSpeed;
		}
	}
}
