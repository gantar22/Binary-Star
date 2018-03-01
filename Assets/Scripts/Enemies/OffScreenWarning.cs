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


	// Called once per frame
	void Update () {
		Vector3 pos = transform.position;
		Vector3 enemyPos = enemy.transform.position;
		Camera cam = Camera.main;

		// Clamp position within the screen
		Vector3 camLimits = cam.ScreenToWorldPoint (new Vector3(cam.scaledPixelWidth, cam.scaledPixelHeight, 0));
		float x = Mathf.Clamp (enemyPos.x, -camLimits.x + radius, camLimits.x - radius);
		float y = Mathf.Clamp (enemyPos.y, -camLimits.y + radius, camLimits.y - radius);

		transform.position = new Vector3 (x, y, transform.position.z);

		// Point the arrow at the enemy

		/* float zRot = Mathf.Rad2Deg * Mathf.Atan2(enemyPos.x - pos.x, enemyPos.y - pos.y);

		arrow.transform.rotation = arrow.transform.rotation */

		// arrow.transform.LookAt (new Vector3 (enemyPos.x, enemyPos.y, 0));
	}
}
