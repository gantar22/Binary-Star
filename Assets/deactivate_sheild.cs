using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deactivate_sheild : MonoBehaviour {


	private Animator am;

	// Use this for initialization
	void Start () {
		am = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnTriggerExit2D(Collider2D col){
		if(col.gameObject.GetComponent<PlayerHP>() && Time.time > .1f){
			Invoke("deactivate",.01f);

		}
	}

	void deactivate(){
		am.SetTrigger("de");
		//gameObject.SetActive(false);
	}


}
