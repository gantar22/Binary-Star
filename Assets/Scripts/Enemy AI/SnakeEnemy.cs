using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SnakeEnemy : MonoBehaviour {

	// Settings/properties:
	[HideInInspector]
	public float speed = 7f;
	private float zipMult = 1.7f;
	private int maxHP = 2;

	private float frequency = 2f;
	private float amplitudeMult = 1f;

	private float minCircle = 3f;
	private float maxCircle = 5f;
	private float minZip = 1.2f;
	private float maxZip = 1.8f;

	// Other variables
	private int HP;

	private bool circling;
	private float delay;

	// Object references
	private Rigidbody2D rb;


	// Initialize
	void Start () {
		rb = GetComponent<Rigidbody2D> ();

		HP = maxHP;
		circling = true;
		delay = minCircle;
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
			direction = perp * Mathf.Cos (radians) * amplitudeMult + direction * Mathf.Sin(radians);
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

	private void controlDelay() {
		if (delay > 0) {
			delay -= Time.deltaTime;
		} else if (circling) {
			circling = !circling;
			frequency = frequency * zipMult;
			speed = speed * zipMult;
			delay = Random.Range (minZip, maxZip);
		} else {
			circling = !circling;
			frequency = frequency / zipMult;
			speed = speed / zipMult;
			delay = Random.Range (minCircle, maxCircle);
		}
	}
}
