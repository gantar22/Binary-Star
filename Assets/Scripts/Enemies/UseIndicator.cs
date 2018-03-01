using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseIndicator : MonoBehaviour {

	// Settings
	[SerializeField]
	private float extraScreenRadius;

	// Object references
	private OffScreenWarning indicator;
	private OffScreenWarning indicatorPrefab;

	// Other variables
	private float leftLimit;
	private float rightLimit;
	private float botLimit;
	private float topLimit;


	void Start () {
		indicatorPrefab = EnemyIdentifier.Instance.OffScreenWarningPrefab;

		// Finding screen limits
		Camera cam = Camera.main;
		Vector3 camLimits = cam.ScreenToWorldPoint (new Vector3(cam.scaledPixelWidth, cam.scaledPixelHeight, 0));
		leftLimit = -camLimits.x - extraScreenRadius;
		rightLimit = camLimits.x + extraScreenRadius;
		botLimit = -camLimits.y - extraScreenRadius;
		topLimit = camLimits.y + extraScreenRadius;
	}

	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		bool inScreen = (pos.x > leftLimit && pos.x < rightLimit && pos.y > botLimit && pos.y < topLimit);

		if (indicator && inScreen) {
			Destroy (indicator.gameObject);
		} else if (!indicator && !inScreen) {
			indicator = Instantiate (indicatorPrefab);
			indicator.enemy = gameObject;
		}
	}
}
