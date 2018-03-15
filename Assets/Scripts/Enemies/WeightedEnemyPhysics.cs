using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WeightedEnemyPhysics : MonoBehaviour {

	// Settings/properties
	//[SerializeField]
	//private float drag = 0.4f;
	[SerializeField]
	private float turnRate = 15f;
	[SerializeField]
	private float angleLeeway = 3f;

	// Other variables
	[HideInInspector]
	public float maxSpeed;
	[HideInInspector]
	public Vector2 velocity, acceleration;

	// Object references
	private Rigidbody2D rb;


	// Initialize
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Called once per frame
	void Update () {
		Vector2 pos = transform.position;


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
			//diff = diff / Mathf.Abs (diff);
			float deltaTheta = Mathf.Pow(Mathf.Abs(diff), 0.6f) * Mathf.Sign(diff) * turnRate * Time.deltaTime;
			//transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, newAngle));
			transform.Rotate(Vector3.forward * deltaTheta);
		}


		// Calculate new velocity
		velocity += acceleration * Time.deltaTime;
		velocity = Vector2.ClampMagnitude (velocity, maxSpeed * Time.deltaTime);
		rb.MovePosition (pos + velocity);
	}
}
