using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjT), typeof(disableifoob))]
public class seeking_missile : MonoBehaviour {

	// Settings
	[SerializeField]
	private float maxSpeed = 1f, accelMag = 0.2f, turnRate = 1f;

	// Properties
	private float angleLeeway = 3f;

	// Other variables
	private Vector2 velocity;
	private bool seekEnemies;
	private GameObject targetObj;

	// References
	private Rigidbody2D rb;
	private disableifoob DifOOB;


	// Check which missile type (player vs enemy)
	void Awake () {
		if (GetComponent<ObjT> ().typ == ObjT.obj.enemy_bullet) {
			seekEnemies = false;
			targetObj = GM.Instance.player;
		} else if (GetComponent<ObjT> ().typ == ObjT.obj.player_bullet) {
			seekEnemies = true;
			targetObj = null;
		} else {
			Debug.LogError ("seeking missile ObjT identifier has wrong type assigned");
		}

		DifOOB = GetComponent<disableifoob> ();
	}


	// Initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Called once per frame
	void Update () {
		if (seekEnemies && targetObj == null) {
			findEnemyTarget ();
		}

		Vector2 pos = transform.position;

		if (targetObj != null) {
			DifOOB.enabled = false;

			// Calculate acceleration (i.e. NewBasicEnemy Update)
			Vector2 targetPos = targetObj.transform.position;
		
			Vector2 direction = targetPos - pos;
			Vector2 acceleration = direction.normalized * accelMag;
				
				
			// Calculate rotation and then translate (i.e. WEP Update)
				
			// Rotate towards direction of acceleration
			float currentAngle = transform.eulerAngles.z;
			if (currentAngle > 180f) {
				currentAngle -= 360f;
			} else if (currentAngle < -180f) {
				currentAngle += 360f;
			}

			float targetAngle = Mathf.Atan2 (acceleration.y, acceleration.x) * Mathf.Rad2Deg;
			if (targetAngle > 180f) {
				targetAngle -= 360f;
			} else if (targetAngle < -180f) {
				targetAngle += 360f;
			}

			float diff = targetAngle - currentAngle;
			if (diff > 180f) {
				diff = diff - 360f;
			} else if (diff < -180f) {
				diff = diff + 360f;
			}

			if (Mathf.Abs (diff) > Mathf.Abs (angleLeeway)) {
				float deltaTheta = Mathf.Pow (Mathf.Abs (diff), 0.6f) * Mathf.Sign (diff) * turnRate * Time.deltaTime;
				transform.Rotate (Vector3.forward * deltaTheta);
			}
				
			// Calculate new velocity
			velocity += acceleration * Time.deltaTime;
			velocity = Vector2.ClampMagnitude (velocity, maxSpeed * Time.deltaTime);
		} else {
			DifOOB.enabled = true;
		}

		// Move the rigidbody according to velocity
		rb.MovePosition (pos + velocity);
	}

	// Find the nearest enemy and start seeking it
	private void findEnemyTarget() {
		GameObject newTarget = null;
		float minDist = int.MaxValue;
		foreach (GameObject enemy in GM.Instance.enemies) {
			float distToEnemy = (transform.position - enemy.transform.position).magnitude;
			if (distToEnemy < minDist) {
				newTarget = enemy;
				minDist = distToEnemy;
			}
		}

		targetObj = newTarget;
	}

	// Public method to set the velocity
	public void setVelo (Vector2 direction) {
		velocity = direction;
		Vector2.ClampMagnitude (velocity, maxSpeed * 0.5f * Time.deltaTime);
	}
}
