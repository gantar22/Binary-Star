using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour {

	// Called once per frame
	void Update () {
		Vector2 velo = ScrollManager.scrollVelo;
		transform.position += new Vector3(velo.x, velo.y, 0) * Time.deltaTime;
	}
}
