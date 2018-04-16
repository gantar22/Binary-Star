using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class pitch_randomizer : MonoBehaviour {

	[SerializeField]
	float variation = .1f;
	[SerializeField]
	float mid = 1;


	void OnEnable(){
		GetComponent<AudioSource>().pitch = Random.value * variation + mid - .5f * variation;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
