using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class self_destruct : MonoBehaviour {

	public float dur;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		dur -= Time.deltaTime;
		if(dur < 0){
			Destroy(transform.root.gameObject);
		}
	}
}
