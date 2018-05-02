using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Extension {

	public static Vector2 Rotate (this Vector2 v, float degrees) {
		float sin = Mathf.Sin (degrees * Mathf.Deg2Rad);
		float cos = Mathf.Cos (degrees * Mathf.Deg2Rad);
	
		float oldX = v.x;
		float oldY = v.y;
		v.x = (cos * oldX) - (sin * oldY);
		v.y = (sin * oldX) + (cos * oldY);
		return v;
	}
	
}
