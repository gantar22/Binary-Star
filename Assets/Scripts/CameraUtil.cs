using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUtil {

	// Move a Transform to the closest position within the Main camera
	public static void keepInCamBounds(Transform trans) {
		float viewport_x_0 = Camera.main.ViewportToWorldPoint (Vector3.zero).x;
		while (trans.position.x < viewport_x_0) {
			trans.Translate (Vector2.right * 0.001f, Space.World);
		}

		float viewport_x_1 = Camera.main.ViewportToWorldPoint (Vector3.one).x;
		while (trans.position.x > viewport_x_1) {
			trans.Translate (Vector2.right * -0.001f, Space.World);
		}

		float viewport_y_0 = Camera.main.ViewportToWorldPoint (Vector3.zero).y;
		while (trans.position.y < viewport_y_0) {
			trans.Translate (Vector2.up * 0.001f, Space.World);
		}

		float viewport_y_1 = Camera.main.ViewportToWorldPoint (Vector3.one).y;
		while (trans.position.y > viewport_y_1) {
			trans.Translate (Vector2.up * -0.001f, Space.World);
		}

		/* while (Camera.main.WorldToViewportPoint (trans.position).x < 0) {
		} */
	}

	// Returns true iff the position is in the camera bounds
	public static bool inCamBounds(Vector3 pos) {
		if (not_percent (Camera.main.WorldToViewportPoint (pos).x) ||
		    not_percent (Camera.main.WorldToViewportPoint (pos).y)) {
			return false;
		}
		return true;
	}

	static bool not_percent(float x){
		return x < -0 || x > 1;
	}
}
