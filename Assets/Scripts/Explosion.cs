using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	// Settings/properties
	public float duration;
	public float sizeMultiplier;
	public Sprite[] sprites;

	private float shakeRadius = 0.006f;
	private float scaleRange = 0.2f;

	// Other variables
	private float timer;
	private float realRad;
	private Vector3 initPos;
	private Vector3 initScale;

	// Object references
	private SpriteRenderer sr;


	// Initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();

		Vector3 scale = transform.localScale;
		transform.localScale = new Vector3 (scale.x * sizeMultiplier, scale.y * sizeMultiplier, scale.z);
		initScale = transform.localScale;

		initPos = transform.position;
		realRad = shakeRadius * transform.localScale.x;
		timer = 0f;
	}
	
	// Called once per frame
	void Update () {
		// Manage timer and sprite swapping
		timer += Time.deltaTime;
		if (timer >= duration) {
			Destroy (gameObject);
		} else {
			int counter = Mathf.FloorToInt (timer / duration * sprites.Length);
			sr.sprite = sprites [counter];
		}

		// Shake a little bit every frame
		float x = initPos.x + Random.Range (-realRad, realRad);
		float y = initPos.y + Random.Range (-realRad, realRad);
		transform.position = new Vector3 (x, y, initPos.z); 

		// Randomize the scaling
		x = initScale.x * (1f + Random.Range(-scaleRange, scaleRange));
		y = initScale.y * (1f + Random.Range(-scaleRange, scaleRange));
		transform.localScale = new Vector3 (x, y, initScale.z);
	}
}
