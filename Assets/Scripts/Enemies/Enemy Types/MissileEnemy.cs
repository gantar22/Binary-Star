using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeightedEnemyPhysics))]
public class MissileEnemy : MonoBehaviour {

	// Settings/properties:
	[SerializeField]
	private float maxSpeed = 4f, accelMag = 5f;
	//private float diveSpeedMult = 1f;

	[SerializeField]
	private float circleTime = 4f, diveTime = 0.4f;

	// Other variables
	private bool clockwise;
	private bool circling;
	private float delay;

	// Object references
	private WeightedEnemyPhysics WEP;
	private ShootsAtPlayer SAP;


	// Initialize
	void Start () {
		SAP = GetComponent<ShootsAtPlayer> ();
		WEP = GetComponent<WeightedEnemyPhysics> ();
		WEP.maxSpeed = maxSpeed;

		clockwise = false;
		circling = false;
		delay = diveTime;
	}

	// Called every frame
	void Update () {
		Vector2 direction = new Vector2 ();
		Vector2 pos = new Vector2 (transform.position.x, transform.position.y);

		// Get player position
		if (GM.Instance.player == null) return;
		Vector2 targetPos = GM.Instance.player_pos;

		// Decide what direction to move in
		direction = targetPos - pos;
		controlDelay ();
		if (circling) {
			Vector2 perp = new Vector2 (-direction.y, direction.x);
			if (clockwise) {
				direction = perp;
			} else {
				direction = -perp;
			}
		}

		// Normalize the velocity and set to desired speed
		WEP.acceleration = direction.normalized * accelMag;
	}

	// Control the delay between circling and closing in
	private void controlDelay() {
		if (delay > 0) {
			delay -= Time.deltaTime;
		} else if (circling) {
			circling = !circling;
			delay = diveTime;
			SAP.shootingEnabled = false;
		} else {
			circling = !circling;
			clockwise = !clockwise;
			delay = circleTime;
			Invoke ("enableShooting", 0.5f);
		}
	}

	// Invoke this with short delay to enable shooting again
	private void enableShooting() {
		SAP.shootingEnabled = true;
	}
}
