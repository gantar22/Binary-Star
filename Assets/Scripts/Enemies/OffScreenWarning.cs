using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffScreenWarning : MonoBehaviour {

	// Settings
	[SerializeField]
	private float radius;

	// Object references
	public GameObject enemy;
	public GameObject arrow;

	void Start () {
		Update ();
	}

	// Called once per frame
	void Update () {
		//deal with enemies beign dead
		if(enemy == null || arrow == null || gameObject == null){
			Destroy(gameObject);
			return;
		}


		// Get references
		Vector3 pos = transform.position;
		Vector3 enemyPos = enemy.transform.position;
		Camera cam = Camera.main;

		// Clamp position within the screen
		Vector3 camLimits = cam.ScreenToWorldPoint (new Vector3(cam.scaledPixelWidth, cam.scaledPixelHeight, 0));
		float x = Mathf.Clamp (enemyPos.x, -camLimits.x + radius, camLimits.x - radius);
		float y = Mathf.Clamp (enemyPos.y, -camLimits.y + radius, camLimits.y - radius);

		transform.position = new Vector3 (x, y, transform.position.z);

		// Point the arrow at the enemy
		Vector3 direction = (enemyPos - pos).normalized;
		float rot_z = Mathf.Rad2Deg * Mathf.Atan2 (direction.y, direction.x);
		arrow.transform.rotation = Quaternion.Euler (0f, 0f, rot_z);
	}
}
