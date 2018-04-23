using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothZoom : MonoBehaviour {

	// Settings
	public float zoomTime;

	// Other variables
	private float targetSize;
	private float currentZoomVelo;

	// References
	private Camera cam;


	// Initialization
	void Awake () {
		cam = GetComponent<Camera> ();

		targetSize = cam.orthographicSize;
		currentZoomVelo = 0f;

		// Testing --- Right now, smooth zoom does not affect UI camera, but this has a really cool effect...
		//addToCameraSize(5f);
	}
	
	// Called once per frame
	void Update () {
		float newSize = Mathf.SmoothDamp (cam.orthographicSize, targetSize, ref currentZoomVelo, zoomTime);
		cam.orthographicSize = newSize;
	}
		
	// Update the target camera size
	public void newCameraSize (float newTarget) {
		targetSize = newTarget;
		currentZoomVelo = 0f;
	}

	// Add to the target camera size
	public void addToCameraSize (float addition) {
		newCameraSize (targetSize + addition);
	}
}
