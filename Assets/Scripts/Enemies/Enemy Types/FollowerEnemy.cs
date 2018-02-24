using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FollowerEnemy : MonoBehaviour {

	// Settings/properties:
	[HideInInspector]
	public float speed = 2f;

	private float followRadius = 0.8f;

	// Other variables

	// Object references
	private Rigidbody2D rb;
	public GameObject objToFollow;


	// Initialize
	void Start () {
		rb = GetComponent<Rigidbody2D> ();

		if (!objToFollow) {
			setAsLeader ();
		} else {
			speed = getLeaderSpeed ();
		}
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
		Vector2 velocity = direction.normalized * speed * Time.deltaTime;
		rb.MovePosition (pos + velocity);
	}

	// ========= TO USE FOLLOWERS FOR A NEW ENEMY TYPE, ADD IT TO THE TWO FUNCTIONS BELOW ========

	// For now, followers are just really fast, so this is not used
	public float getLeaderSpeed() {
		FollowerEnemy followFollower = objToFollow.GetComponent<FollowerEnemy> ();
		if (followFollower && followFollower.enabled) {
			return followFollower.getLeaderSpeed ();
		} else if (objToFollow.GetComponent<BasicEnemy> ()) {
			return objToFollow.GetComponent<BasicEnemy> ().speed;
		} else if (objToFollow.GetComponent<ZigZagEnemy> ()) {
			return objToFollow.GetComponent<ZigZagEnemy> ().speed;
		} else if (objToFollow.GetComponent<ZipperEnemy> ()) {
			return objToFollow.GetComponent<ZipperEnemy> ().speed;
		} else if (objToFollow.GetComponent<SnakeEnemy> ()) {
			return objToFollow.GetComponent<SnakeEnemy> ().speed;
		} else {
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
