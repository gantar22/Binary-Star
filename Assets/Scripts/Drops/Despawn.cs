using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : MonoBehaviour {

	// Settings/properties
	[SerializeField]
	private float despawnTime = 10f;
	private float warningTime = 1f, flashFreq = 5f;
	private float minAlpha = 0.3f, maxAlpha = 0.9f;

	// Other variables
	private float timer;
	private float alphaCounter;

	// References
	private SpriteRenderer thisSR;
	

	// Use this for initialization
	void Start () {
		thisSR = GetComponent<SpriteRenderer> ();
		timer = despawnTime;
		setAllColorScale (1);
		alphaCounter = Mathf.PI / -2f;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0f) {
			Destroy (gameObject);
		} else if (timer < warningTime) {
			alphaCounter += flashFreq * Time.deltaTime * (2f * Mathf.PI);
			float alphaScale = (Mathf.Sin (alphaCounter) + 1f) / 2f;
			float newAlpha = Mathf.Lerp (minAlpha, maxAlpha, alphaScale);

			setAllColorScale (newAlpha);
		}
	}

	// Scale the colors
	private void setAllColorScale(float scale) {
		Color newColor = new Color (scale, scale, scale, scale);
		thisSR.color = newColor;
	}
}
