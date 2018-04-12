using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireworks : MonoBehaviour {

	// Settings/properties
	public GameObject BulletPrefab;
	public float offset;
	public float bulletCount;


	// Spawn bullets all around the enemy
	public void Explode () {
		float degrees = transform.eulerAngles.z * Mathf.Deg2Rad;
		float incrBy = 2f * Mathf.PI / bulletCount;
		for (int i = 0; i < bulletCount; i++) {
			float y = Mathf.Cos (degrees);
			float x = Mathf.Sin (degrees);

			// Instantiate the bullet at the correct position
			Vector2 displacement = (new Vector2(x, y)).normalized * offset;
			Vector3 newPos = transform.position + ((Vector3) displacement);
			GameObject newBullet = Instantiate (BulletPrefab, newPos, transform.rotation);

			// Aim it in the correct direction
			float targetAngle = Mathf.Atan2 (displacement.y, displacement.x) * Mathf.Rad2Deg;
			float currentAngle = transform.eulerAngles.z;
			float diff = targetAngle - currentAngle;
			newBullet.transform.Rotate (Vector3.forward * diff);

			// Increment the degrees for the next bullet to spawn
			degrees += incrBy;
		}
	}
}
