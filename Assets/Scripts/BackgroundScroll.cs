using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum direction {left, right, up, down, still};

public class BackgroundScroll : MonoBehaviour {

	// Settings
	private static float accelTime = 1f;

	// Object references
	private Rigidbody2D rb;

	// Other variables
	private static Vector2 targetVelo;
	private static Vector2 velo;
	private static float timer;
	private static int updateCounter;


	// Initialize
	void Start () {
		rb = GetComponent<Rigidbody2D> ();

		// Testing:
		//setBgScrollSpeed(direction.up, 2f);
	}
	
	// Called once per frame
	void Update () {
		veloUpdate ();

		rb.velocity = velo;
	}

	// Change the scroll direction
	public static void setBgScrollSpeed(direction dir, float speed) {
		Vector2 veloDir = Vector2.zero;
		if (dir == direction.down) {
			veloDir = Vector2.down;
		} else if (dir == direction.up) {
			veloDir = Vector2.up;
		} else if (dir == direction.left) {
			veloDir = Vector2.left;
		} else if (dir == direction.right) {
			veloDir = Vector2.right;
		}

		targetVelo = veloDir * speed;
		timer = 0f;
	}

	// Called by every BackgroundScroll update (should be 4 running)
	private static void veloUpdate() {
		updateCounter++;
		if (updateCounter % 4 != 1) {
			return;
		}
		updateCounter = 1;

		if (timer < accelTime) {
			timer += Time.deltaTime;
		} else {
			timer = accelTime;
		}

		velo = Vector2.Lerp (Vector2.zero, targetVelo, timer/accelTime);
	}
}
