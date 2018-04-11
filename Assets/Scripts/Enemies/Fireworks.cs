using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireworks : MonoBehaviour {

	// Settings/properties
	public GameObject BulletPrefab;
	public float offset;
	public float bulletCount;

	public void Explode () {
		float degrees = 0f;
		float incrBy = 2f * Mathf.PI / bulletCount;
		for (int i = 0; i < bulletCount; i++) {
			float y = Mathf.Cos (degrees);
			float x = Mathf.Sin (degrees);

			Vector2 displacement = (new Vector2(x, y)).normalized * offset;
			Vector3 newPos = transform.position + ((Vector3) displacement);
			GameObject newBullet = Instantiate (BulletPrefab);
			newBullet.transform.position = newPos;

			degrees += incrBy;
		}
	}
}
