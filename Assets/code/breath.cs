using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breath : MonoBehaviour {

	[SerializeField]
	float ydiff = .1f;
	[SerializeField]
	float xdiff = .1f;
	[SerializeField]
	float speed = 1;

	Vector3 scale;

	void Start(){
		scale = transform.localScale;
	} 
	
	void Update () {
		transform.localScale = scale + new Vector3(Mathf.Sin(Time.time * speed) * xdiff,Mathf.Cos(Time.time * speed) * ydiff,0);
	}
}
