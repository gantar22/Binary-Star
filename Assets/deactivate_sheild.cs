using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deactivate_sheild : MonoBehaviour {


	private Animator am;

	// Use this for initialization
	void Start () {
		am = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(GM.Instance.player.transform.position,transform.position) < 10) deactivate();
		if(Vector3.Distance(GM.Instance.player.transform.position,transform.position) > 20) activate();	
	}




	void deactivate(){
		am.SetTrigger("de");
		//gameObject.SetActive(false);
	}
	void activate(){
		am.SetTrigger("act");
		//gameObject.SetActive(false);
	}


}
