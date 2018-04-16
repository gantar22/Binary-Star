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
	private float bounceScale = 0.7f;

	public bool noCollisions = true;

	// Other variables
	[HideInInspector]
	public float maxSpeed;
	[HideInInspector]
	public Vector2 velocity, acceleration;
	[HideInInspector]
	public bool clampPerp = false;
	[HideInInspector]
	public float minDegree, maxDegree;

	// Object references
	private Rigidbody2D rb;


	// Initialize
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Called once per frame
	void FixedUpdate () {
		Vector2 pos = transform.position;


		// Rotate towards direction of acceleration
		float currentAngle = transform.eulerAngles.z;
		currentAngle = normalDegrees (currentAngle);

		float targetAngle = Mathf.Atan2 (acceleration.y, acceleration.x) * Mathf.Rad2Deg;
		targetAngle = normalDegrees (targetAngle);

		float diff = targetAngle - currentAngle;
		diff = normalDegrees (diff);

		if (Mathf.Abs(diff) > Mathf.Abs(angleLeeway)) {
			float deltaTheta = Mathf.Pow(Mathf.Abs(diff), 0.6f) * Mathf.Sign(diff) * turnRate * Time.deltaTime;
			transform.Rotate(Vector3.forward * deltaTheta);
		}

		// If clampPerp is enabled, keep the angle within the min/max degrees
		if (clampPerp) {
			Vector3 lEA = transform.localEulerAngles;
			float newZ = angleClamp(lEA.z, minDegree, maxDegree);
			transform.localEulerAngles = new Vector3 (lEA.x, lEA.y, newZ); 
		}

		// Calculate new velocity
		velocity += acceleration * Time.deltaTime;
		velocity = Vector2.ClampMagnitude (velocity, maxSpeed * Time.deltaTime);

		rb.MovePosition (pos + velocity);
	}

	// Detect collisions with other enemies, or player (while invuln), to prevent stacking
	void OnTriggerStay2D(Collider2D col){
		if (!noCollisions) {
			return;
		}

		WeightedEnemyPhysics OtherWEP = col.gameObject.GetComponent<WeightedEnemyPhysics>(); 
		PlayerHP PHP = col.gameObject.GetComponent<PlayerHP> ();
		if (OtherWEP != null || PHP != null) {
			Vector2 OtherPos = col.transform.position;
			Vector2 diff = (OtherPos - (Vector2) transform.position).normalized;

			Vector2 projection = Vector2.Dot (diff, velocity) * diff;

			if ((diff + projection.normalized).magnitude > diff.magnitude) {
				velocity -= projection * (1f + bounceScale);
			}
		}
	}

	// Normalize the degrees to the range (-180, 180)
	public static float normalDegrees (float degrees) {
		while (degrees > 180f) {
			degrees -= 360f;
		} 
		while (degrees < -180f) {
			degrees += 360f;
		}
		return degrees;
	}

	// Clamp the degrees between two values, but pick the nearest clamp value
	public static float angleClamp (float value, float min, float max) {
		value = normalDegrees (value);
		min = normalDegrees (min);
		max = normalDegrees (max);

		if ((value >= min && value <= max) || (value <= min && value >= max)) {
			return value;
		}

		float minDiff = normalDegrees(value - min);
		float maxDiff = normalDegrees(value - max);

		if (Mathf.Abs(minDiff) > Mathf.Abs(maxDiff)) {
			return max;
		} else {
			return min;
		}
	}
}
