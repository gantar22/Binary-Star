using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkEnemy : MonoBehaviour {

	// Settings/properties:
	[SerializeField]
	private float maxSpeed = 3.2f, accelMag = 0.1f, triggerRadius = 4f;
	[SerializeField]
	private float pauseDuration = 1.5f, flashPeriod = 0.25f;
	[SerializeField]
	private Sprite flashSprite;

	// Other variables
	private Sprite normalSprite;
	private bool triggered, normalState;
	private float triggerTimer;

	// Object references
	private WeightedEnemyPhysics WEP;
	private SpriteRenderer SR;


	// Initialize
	void Start () {
		WEP = GetComponent<WeightedEnemyPhysics> ();
		WEP.maxSpeed = maxSpeed;

		SR = GetComponent<SpriteRenderer> ();
		normalSprite = SR.sprite;

		triggered = false;
		normalState = true;
	}

	// Called every frame
	void Update () {
		// If triggered, flash and/or explode. Otherwise, move as normal
		if (triggered) {
			triggerTimer -= Time.deltaTime;

			// Swap to the correct sprite
			if ((pauseDuration - triggerTimer) % (flashPeriod) < flashPeriod / 2f) {
				swapSprite (false);
			} else {
				swapSprite (true);
			}

			// Check if trigger time is up
			if (triggerTimer <= 0f) {
				//fireworks.Explode ();  // Already explodes on death
				GetComponent<EnemyHP> ().die ();
			}

			return;
		}

		// If not triggered, move as towards player as normal
		Vector2 pos = transform.position;

		if (GM.Instance.player == null)
			return;
		Vector2 targetPos = GM.Instance.player.transform.position;

		// Decide what direction to move in
		Vector2 direction = targetPos - pos;

		// Normalize the velocity and set to desired speed
		WEP.acceleration = direction.normalized * accelMag;

		// Check if the fireworks should trigger
		Vector2 diff = transform.position - GM.Instance.player.transform.position;
		if (diff.magnitude < triggerRadius) {
			pauseThenExplode ();
		}
	}

	// Trigger the flashing, then explode after delay
	private void pauseThenExplode() {
		triggered = true;
		triggerTimer = pauseDuration;

		WEP.maxSpeed = 0f;
	}

	// Swap the sprite appropriately, if needed
	private void swapSprite(bool normal) {
		if (normal && !normalState) {
			normalState = true;
			SR.sprite = normalSprite;
			transform.localScale /= 1.25f;
		} else if (!normal && normalState) {
			normalState = false;
			SR.sprite = flashSprite;
			transform.localScale *= 1.25f;
		}
	}
}
