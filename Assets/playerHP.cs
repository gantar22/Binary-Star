using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class playerHP : MonoBehaviour {
	// Settings/properties:
	[SerializeField]
	private int maxHP = 1;

	// Other variables
	private int HP;

	// Object references


	// Initialize
	void Start () {
		HP = maxHP;
	}

	void OnTriggerEnter2D(Collider2D other) {
		
		if(other.gameObject.layer == 8)
			HP -= 1;
		

		if (HP <= 0) {
			

			if(Camera.main.GetComponent<CameraShakeScript>() != null){
				Camera.main.GetComponent<CameraShakeScript>().activate(.1f,.1f);
			}
			Destroy (transform.root.gameObject);
		}
	}
}
