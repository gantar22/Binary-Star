using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class bomb_det : MonoBehaviour {

	float cooldown = 60;
	bool canfire;

	void Start(){
		canfire = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(XCI.GetButton(XboxButton.LeftStick,XboxController.First) && XCI.GetButton(XboxButton.LeftStick,XboxController.Second) && canfire){
			/*GameObject[] allObjects = Object.FindObjectsOfType<GameObject>() ;
			foreach(GameObject go in allObjects){
				if (go.activeInHierarchy && go.GetComponent<EnemyHP>())
				go.GetComponent<EnemyHP>().hitByBomb();
			}*/

			canfire = false;
			foreach(GameObject go in GM.Instance.enemies){
				if(go.GetComponent<EnemyHP>()) go.GetComponent<EnemyHP>().hitByBomb();
			}
			CameraShakeScript CSS = Camera.main.GetComponent<CameraShakeScript> ();
			if(CSS != null){
				CSS.activate(.5f,.5f); 
			}
			Invoke("act",cooldown);
		}
	}

	void act(){
		canfire = true;
	}
}
