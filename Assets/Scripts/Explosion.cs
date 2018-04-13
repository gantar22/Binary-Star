using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	// Settings/properties
	public float duration;
	public Sprite[] sprites;

	private float shakeRadius = 0.006f;

	// Other variables
	private float timer;
	private float realRad;
	private Vector2 initPos;

	// Object references
	private SpriteRenderer sr;


	// Initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
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
		transform.position = new Vector3 (x, y, 0);
	}
}
