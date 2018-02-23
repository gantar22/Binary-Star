using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SyncCircleEnemy : MonoBehaviour {

	// Settings/properties:
	[HideInInspector]
	public float speed = 4f;
	private float zipMult = 1f;
	private int maxHP = 2;

	private float circleTime = 2f;
	private float zipTime = 0.4f;

	// Other variables
	private int HP;

	private bool clockwise;
	private bool circling;
	private float delay;

	// Object references
	private Rigidbody2D rb;


	// Initialize
	void Start () {
		rb = GetComponent<Rigidbody2D> ();

		HP = maxHP;
		clockwise = false;
		circling = false;
		delay = zipTime;
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
			delay = zipTime;
		} else {
			circling = !circling;
			clockwise = !clockwise;
			delay = circleTime;
		}
	}

}
