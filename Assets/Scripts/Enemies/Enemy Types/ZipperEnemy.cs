using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeightedEnemyPhysics))]
public class ZipperEnemy : MonoBehaviour {

	// Settings/properties:
	//[SerializeField]
	private float maxSpeed = 4f, accelMag = 1f;

	private float minPause = 1.2f;
	private float maxPause = 1.8f;
	private float minZip = 1f;
	private float maxZip = 1f;

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

		// Get player position - TODO
		//Vector2 targetPos = new Vector2 (0f, 0f);
		Vector3 mousePos3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 targetPos = new Vector2 (mousePos3.x, mousePos3.y);

		// Decide what direction to move in
		direction = targetPos - pos;
		controlDelay (direction);
		direction = fixedDirection;

		if (!paused) {
			// Normalize the velocity and set to desired speed
			WEP.acceleration = direction.normalized * accelMag;
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
