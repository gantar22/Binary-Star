using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeightedEnemyPhysics))]
public class SnakeEnemy : MonoBehaviour {

	// Settings/properties:
	[SerializeField]
	private float maxSpeed = 3.5f, accelMag = 5f;

	[SerializeField]
	private float diveSpeedMult = 1.7f;

	[SerializeField]
	private float frequency = 2f, amplitudeMult = 1f;

	[SerializeField]
	private float minCircleTime = 3f, maxCircleTime = 5f;
	[SerializeField]
	private float minDiveTime = 0.8f, maxDiveTime = 1.2f;

	// Other variables

	private bool circling;
	private float delay;

	// Object references
	private WeightedEnemyPhysics WEP;


	// Initialize
	void Start () {
		WEP = GetComponent<WeightedEnemyPhysics> ();
		WEP.maxSpeed = maxSpeed;

		circling = true;
		delay = minCircleTime;
	}

	// Called every frame
	void Update () {
		Vector2 direction = new Vector2 ();
		Vector2 pos = new Vector2 (transform.position.x, transform.position.y);

		// Get player position - TODO
		//Vector2 playerPos = new Vector2 (0f, 0f);
		Vector3 mousePos3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 mousePos = new Vector2 (mousePos3.x, mousePos3.y);

		// Decide what direction to move in
		direction = mousePos - pos;
		Vector2 perp = new Vector2 (-direction.y, direction.x);

		float period = 1f / frequency;
		float scale = Mathf.Abs((Time.time % (2*period)) - period) / period;
		float radians = Mathf.Lerp(-Mathf.PI/2f, Mathf.PI/2f, scale);

		controlDelay ();
		if (!circling) {
			direction = perp * Mathf.Sin (radians) * amplitudeMult + direction * Mathf.Cos (radians);
		} else {
			direction = perp * Mathf.Cos (radians) + direction * Mathf.Sin(radians) * amplitudeMult;
		}

		// Normalize the velocity and set to desired speed
		WEP.acceleration = direction.normalized * accelMag;
	}

	private void controlDelay() {
		if (delay > 0) {
			delay -= Time.deltaTime;
		} else if (circling) {
			circling = !circling;
			frequency = frequency * diveSpeedMult;
			WEP.maxSpeed = maxSpeed * diveSpeedMult;
			delay = Random.Range (minDiveTime, maxDiveTime);
		} else {
			circling = !circling;
			frequency = frequency / diveSpeedMult;
			WEP.maxSpeed = maxSpeed;
			delay = Random.Range (minCircleTime, maxCircleTime);
		}
	}
}
