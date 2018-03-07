using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeightedEnemyPhysics))]
public class SleeperEnemy : MonoBehaviour {

	// Settings/properties:
	[SerializeField]
	private float circleSpeed = 4.5f, chaseSpeed = 7f, accelMag = 0.1f;

	[SerializeField]
	private float wakeRadius = 5f, frequency = 1f, inwardBoost = 0.4f;

	// Other variables

	private bool chasing;

	// Object references
	private WeightedEnemyPhysics WEP;


	// Initialize
	void Start () {
		WEP = GetComponent<WeightedEnemyPhysics> ();
		WEP.maxSpeed = circleSpeed;

		chasing = false;
	}

	// Called every frame
	void Update () {
		Vector2 direction = new Vector2 ();
		Vector2 pos = new Vector2 (transform.position.x, transform.position.y);

		// Get player position
		if (GM.Instance.player == null) return;
		Vector2 targetPos = GM.Instance.player.transform.position;

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
		WEP.acceleration = direction.normalized * accelMag;
	}

	// Check if the player is within the wake radius
	private void checkForChase(Vector2 targetPos, Vector2 pos) {
		if (chasing && (targetPos - pos).magnitude > wakeRadius) {
			chasing = false;
			WEP.maxSpeed = circleSpeed;
		} else if (!chasing && (targetPos - pos).magnitude <= wakeRadius) {
			chasing = true;
			WEP.maxSpeed = chaseSpeed;
		}
	}
}
