using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeightedEnemyPhysics))]
public class FollowerEnemy : MonoBehaviour {

	// Settings/properties:
	//[SerializeField]
	private float maxSpeed = 2f, accelMag = 0.1f;

	private float followRadius = 0.8f;

	// Other variables

	// Object references
	private WeightedEnemyPhysics WEP;
	public GameObject objToFollow;


	// Initialize
	void Start () {
		if (!objToFollow) {
			setAsLeader ();
		} else {
			maxSpeed = getLeaderSpeed ();
		}

		WEP = GetComponent<WeightedEnemyPhysics> ();
		WEP.maxSpeed = maxSpeed;
	}

	// Called every frame
	void Update () {
		if (!objToFollow) {
			setAsLeader ();
			return;
		}

		Vector2 direction = new Vector2 ();
		Vector2 pos = new Vector2 (transform.position.x, transform.position.y);

		// Get position of object to follow
		Vector2 followPos = new Vector2 (objToFollow.transform.position.x, objToFollow.transform.position.y);

		// Decide what direction to move in
		direction = followPos - pos;
		if (direction.magnitude < followRadius) {
			return;
		}

		// Normalize the velocity and set to desired speed
		WEP.acceleration = direction.normalized * accelMag;
	}

	// ========= TO USE FOLLOWERS FOR A NEW NON-WEIGHTED ENEMY TYPE, ADD IT TO THE TWO FUNCTIONS BELOW ========

	// For now, followers are just really fast, so this is not used
	public float getLeaderSpeed() {
		WeightedEnemyPhysics WEPtoFollow = objToFollow.GetComponent<WeightedEnemyPhysics> ();
		if (WEPtoFollow) {
			return WEPtoFollow.velocity.magnitude;
		} /* else if (objToFollow.GetComponent<SnakeEnemy> ()) {
			return objToFollow.GetComponent<SnakeEnemy> ().speed;
		} */ else {
			Debug.LogError ("No leader speed found");
			return 0f;
		}
	}

	// When follower is destroyed, enable new leader script
	private void setAsLeader() {
		if (GetComponent<BasicEnemy> ()) {
			GetComponent<BasicEnemy> ().enabled = true;
		} else if (GetComponent<ZigZagEnemy> ()) {
			GetComponent<ZigZagEnemy> ().enabled = true;
		} else if (GetComponent<ZipperEnemy> ()) {
			GetComponent<ZipperEnemy> ().enabled = true;
		} else if (GetComponent<SnakeEnemy> ()) {
			GetComponent<SnakeEnemy> ().enabled = true;
		} else {
			Debug.LogError ("Follower enemy lost leader and itself has no backup script");
		}

		this.enabled = false;
	}
}
