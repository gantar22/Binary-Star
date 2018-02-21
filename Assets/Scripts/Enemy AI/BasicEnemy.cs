using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BasicEnemy : MonoBehaviour {

	// Settings/properties:
	[HideInInspector]
	public float speed = 1.2f;
	private int maxHP = 2;

	// Other variables
	private int HP;

	// Object references
	private Rigidbody2D rb;


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
