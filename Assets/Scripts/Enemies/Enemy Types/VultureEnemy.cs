
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeightedEnemyPhysics))]
public class VultureEnemy : MonoBehaviour {

	// Settings/properties:
	[SerializeField]
	private int fakeDives = 2;

	[SerializeField]
	private float maxSpeed = 3.5f, accelMag = 5f;

	[SerializeField]
	private float diveSpeedMult = 1.7f;

	[SerializeField]
	private float frequency = 2f;

	[SerializeField]
	private float minCircleTime = 3f, maxCircleTime = 5f;
	[SerializeField]
	private float minDiveTime = 0.8f, maxDiveTime = 1.2f;

	// Other variables
	private bool circling;
	private float delay;
	private int divesSoFar;

	// Object references
	private WeightedEnemyPhysics WEP;


	// Initialize
	void Start () {
		WEP = GetComponent<WeightedEnemyPhysics> ();
		WEP.maxSpeed = maxSpeed;

		circling = true;
		delay = minCircleTime;
		divesSoFar = 0;
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
			direction = perp;
		}

		// Normalize the velocity and set to desired speed
		WEP.acceleration = direction.normalized * accelMag;
	}

	private void controlDelay() {
		if (divesSoFar > fakeDives) {
			return;
		}

		if (delay > 0) {
			delay -= Time.deltaTime;
		} else if (circling) {
			// Start to dive
			divesSoFar++;
			circling = false;
			frequency = frequency * diveSpeedMult;
			WEP.maxSpeed = maxSpeed * diveSpeedMult;
			delay = Random.Range (minDiveTime, maxDiveTime);
		} else {
			// Start circling again
			circling = true;
			frequency = frequency / diveSpeedMult;
			WEP.maxSpeed = maxSpeed;
			delay = Random.Range (minCircleTime, maxCircleTime);
		}
	}
}
