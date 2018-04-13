using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeightedEnemyPhysics))]
public class SleeperEnemy : MonoBehaviour {

	// Settings/properties:
	[SerializeField]
	private float circleSpeed = 0.1f, chaseSpeed = 11f, accelMag = 1f; 

	[SerializeField]
	private float wakeRadius = 15f, frequency = 0.3f;

	[SerializeField]
	private Sprite AsleepSprite;
	[SerializeField]
	private Sprite AwakeSprite;

	// Other variables
	private bool chasing;

	// Object references
	private WeightedEnemyPhysics WEP;
	private SpriteRenderer SR;
	private ScrollingObject SO;


	// Initialize
	void Start () {
		WEP = GetComponent<WeightedEnemyPhysics> ();
		WEP.maxSpeed = circleSpeed;
		SR = GetComponentInChildren<SpriteRenderer> ();
		SR.sprite = AsleepSprite;
		SO = GetComponent<ScrollingObject> ();
		SO.enabled = true;

		chasing = false;
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

		float period = 1f / frequency;
		float radians = (Time.time % (period)) / period * 2 * Mathf.PI;

		checkForChase (targetPos, pos);
		if (!chasing) {
			direction = Vector2.up * Mathf.Cos (radians) + Vector2.right * Mathf.Sin (radians);
		}

		// Normalize the velocity and set to desired speed
		WEP.acceleration = direction.normalized * accelMag;
	}

	// Check if the player is within the wake radius
	private void checkForChase(Vector2 targetPos, Vector2 pos) {
		if (chasing && (targetPos - pos).magnitude > wakeRadius) {
			chasing = false;
			SO.enabled = true;
			SR.sprite = AsleepSprite;
			WEP.maxSpeed = circleSpeed;
		} else if (!chasing && (targetPos - pos).magnitude <= wakeRadius) {
			chasing = true;
			SO.enabled = false;
			SR.sprite = AwakeSprite;
			WEP.maxSpeed = chaseSpeed;
		}
	}
}
