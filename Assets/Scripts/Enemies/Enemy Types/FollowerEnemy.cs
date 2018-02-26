using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeightedEnemyPhysics))]
public class FollowerEnemy : MonoBehaviour {

	// Settings/properties:
	[SerializeField]
	private float followRadius = 10f;

	// Other variables
	private float maxSpeed, accelMag;

	// Object references
	public GameObject objToFollow;
	private WeightedEnemyPhysics WEP, leaderWEP;


	// Initialize
	void Start () {
		if (!objToFollow) {
			setAsLeader ();
			return;
		} else {
			maxSpeed = getLeaderSpeed ();
		}

		WEP = GetComponent<WeightedEnemyPhysics> ();
		leaderWEP = objToFollow.GetComponent<WeightedEnemyPhysics> ();
		WEP.maxSpeed = maxSpeed;
	}

	// Called every frame
	void Update () {
		if (!objToFollow) {
			setAsLeader ();
			return;
		} else {
			WEP.maxSpeed = getLeaderSpeed ();
			accelMag = getLeaderAccelMag ();
		}

		Vector2 direction = new Vector2 ();
		Vector2 pos = new Vector2 (transform.position.x, transform.position.y);

		// Get position of object to follow
		Vector2 followPos = new Vector2 (objToFollow.transform.position.x, objToFollow.transform.position.y);

		// Decide what direction to move in
		direction = followPos - pos;
		if (direction.magnitude < followRadius) {
			WEP.acceleration = Vector2.zero;
			WEP.velocity = Vector2.zero; // Not weighty...
			return;
		}

		// Normalize the velocity and set to desired speed
		WEP.acceleration = direction.normalized * accelMag;
		print ("Follower WEP maxSpeed: " + WEP.maxSpeed);
		print ("Follower WEP acceleration: " + WEP.acceleration.magnitude);
	}

	public float getLeaderSpeed() {
		if (!leaderWEP) {
			leaderWEP = getLeaderWEP ();
		}
		return leaderWEP.velocity.magnitude / Time.deltaTime;
	}

	public float getLeaderAccelMag() {
		if (!leaderWEP) {
			leaderWEP = getLeaderWEP ();
		}
		return leaderWEP.acceleration.magnitude;
	}

	public WeightedEnemyPhysics getLeaderWEP() {
		FollowerEnemy nextInChain = objToFollow.GetComponent<FollowerEnemy> ();
		if (nextInChain && nextInChain.enabled) {
			return nextInChain.getLeaderWEP ();
		} else {
			WeightedEnemyPhysics newLeadWEP = objToFollow.GetComponent<WeightedEnemyPhysics> ();
			if (newLeadWEP) {
				return newLeadWEP;
			} else {
				Debug.LogError ("No leader WeightedEnemyPhysics component found");
				return null;
			}
		}
	}

/* ========= TO USE FOLLOWERS FOR A NEW NON-WEIGHTED ENEMY TYPE, ADD IT TO THE FUNCTION BELOW ======== */

	// When follower is destroyed, enable new leader script
	private void setAsLeader() {
		if (GetComponent<NewBasicEnemy> ()) {
			GetComponent<NewBasicEnemy> ().enabled = true;
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
