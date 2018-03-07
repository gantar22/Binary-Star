using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeightedEnemyPhysics))]
public class SyncCircleEnemy : MonoBehaviour {

	// Settings/properties:
	[SerializeField]
	private float maxSpeed = 4f, accelMag = 5f;
	//private float diveSpeedMult = 1f;

	[SerializeField]
	private float circleTime = 2f, diveTime = 0.4f;

	// Other variables
	private bool clockwise;
	private bool circling;
	private float delay;

	// Object references
	private WeightedEnemyPhysics WEP;


	// Initialize
	void Start () {
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
		Vector2 targetPos = GM.Instance.player.transform.position;

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
		} else {
			circling = !circling;
			clockwise = !clockwise;
			delay = circleTime;
		}
	}

}
