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
	private float angleLeeway = 3f;
	private float bounceScale = 0.7f;

	public bool noCollisions = true;

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

	// Detect collisions with other enemies to prevent stacking
	void OnTriggerStay2D(Collider2D col){
		if (!noCollisions) {
			return;
		}

		WeightedEnemyPhysics OtherWEP = col.gameObject.GetComponent<WeightedEnemyPhysics>(); 
		if (OtherWEP != null){
			Vector2 OtherPos = col.transform.position;
			Vector2 diff = (OtherPos - (Vector2) transform.position).normalized;

			Vector2 projection = Vector2.Dot (diff, velocity) * diff;

			if ((diff + projection.normalized).magnitude > diff.magnitude) {
				velocity -= projection * (1f + bounceScale);
			}
		}
	}
}
