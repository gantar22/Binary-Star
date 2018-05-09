using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class y_exists : MonoBehaviour {


	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < transform.childCount;i++){
			transform.GetChild(i).gameObject.SetActive(YButtonManager.abilityIsUnlocked());
		}
	}
}
