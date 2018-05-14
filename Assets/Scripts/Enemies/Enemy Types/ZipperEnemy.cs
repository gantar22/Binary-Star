using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeightedEnemyPhysics))]
public class ZipperEnemy : MonoBehaviour {

	// Settings/properties:
	[SerializeField]
	private float maxSpeed = 4f, accelMag = 1f;

	[SerializeField]
	private float minPause = 1.2f, maxPause = 1.8f;
	[SerializeField]
	private float minZip = 1f, maxZip = 1f;

	// Other variables
	private bool paused;
	private float delay;
	private Vector2 fixedDirection;

	// Object references
	private WeightedEnemyPhysics WEP;


	// Initialize
	void Start () {
		WEP = GetComponent<WeightedEnemyPhysics> ();
		WEP.maxSpeed = maxSpeed;
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
		controlDelay (direction);
		direction = fixedDirection;

		if (!paused) {
			// Normalize the velocity and set to desired speed
			WEP.acceleration = direction.normalized * accelMag;
		} else {
			WEP.acceleration = Vector2.zero;
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
}
