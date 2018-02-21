using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SnakeEnemy : MonoBehaviour {

	// Settings/properties:
	[HideInInspector]
	public float speed = 6f;
	private int maxHP = 2;

	private float frequency = 0.5f;
	private float amplitudeMult = 1f;

	// Object references
	private Rigidbody2D rb;

	// Other variables
	private int HP;


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
		//Vector2 playerPos = new Vector2 (0f, 0f);
		Vector3 mousePos3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 mousePos = new Vector2 (mousePos3.x, mousePos3.y);

		// Decide what direction to move in
		direction = mousePos - pos;
		Vector2 perp = new Vector2 (-direction.y, direction.x);

		float scale = Mathf.Abs((Time.time % (2*frequency)) - frequency) / frequency;
		float radians = Mathf.Lerp(-Mathf.PI/2f, Mathf.PI/2f, scale);
		
		direction = perp * Mathf.Sin (radians) * amplitudeMult + direction * Mathf.Cos(radians);

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
}
