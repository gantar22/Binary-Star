using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seeking_missile : MonoBehaviour {

	// Settings
	[SerializeField]
	private float maxSpeed = 1f, accelMag = 1f, turnRate = 1f;

	// Properties
	private float angleLeeway = 3f;

	// Other variables
	private Vector2 velocity;

	// References
	private Rigidbody2D rb;


	// Initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Called once per frame
	void Update () {
		// Calculate acceleration (i.e. NewBasicEnemy Update)
		Vector2 pos = transform.position;
		if (GM.Instance.player == null) return;
		Vector2 targetPos = GM.Instance.player.transform.position;

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

		if (Mathf.Abs(diff) > Mathf.Abs(angleLeeway)) {
			float deltaTheta = Mathf.Pow(Mathf.Abs(diff), 0.6f) * Mathf.Sign(diff) * turnRate * Time.deltaTime;
			transform.Rotate(Vector3.forward * deltaTheta);
		}
			
		// Calculate new velocity
		velocity += acceleration * Time.deltaTime;
		velocity = Vector2.ClampMagnitude (velocity, maxSpeed * Time.deltaTime);
		rb.MovePosition (pos + velocity);
	}
}
