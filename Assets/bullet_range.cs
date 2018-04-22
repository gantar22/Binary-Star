using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_range : MonoBehaviour {
	private Vector3 startPos;
	public float range = 5;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(startPos,transform.position) > range){
			transform.parent.gameObject.SetActive(false);
		}
		
	}
}
