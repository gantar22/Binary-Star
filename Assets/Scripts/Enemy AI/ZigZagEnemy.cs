using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ZigZagEnemy : MonoBehaviour {

	// Settings/properties:
	[HideInInspector]
	public float speed = 1.6f;
	private int maxHP = 3;

	private float minZgDelay = 0.5f;
	private float maxZgDelay = 3f;

	// Object references
	private Rigidbody2D rb;

	// Other variables
	private int HP;

	private bool zigOrZag;
	private float delay;


	// Initialize
	void Start () {
		rb = GetComponent<Rigidbody2D> ();

		HP = maxHP;
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
		Vector2 perpHalf = new Vector2 (-direction.y, direction.x) * zigZagRandom();
		direction = direction + perpHalf;

		// Normalize the velocity and set to desired speed
		Vector2 velocity = direction.normalized * speed * Time.deltaTime;
		rb.MovePosition (pos + velocity);
	}

	// Flips zigOrZag randomly in the range (given in settings), and returns 1 if zig and -1 if zag
	private float zigZagRandom() {
		if (delay > 0) {
			delay -= Time.deltaTime;
		} else {
			zigOrZag = !zigOrZag;
			delay = Random.Range (minZgDelay, maxZgDelay);
		}

		if (zigOrZag) {
			return 1f;
		} else {
			return -1f;
		}
	}

	// Called when damage is taken
	private void gotHit(int dmg) {
		HP -= dmg;

		if (HP <= 0) {
			// DO SOMETHING ELSE WHEN ENEMY IS DESTROYED? - TODO
			Destroy (gameObject);
		}
	}
}
