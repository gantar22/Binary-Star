using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class bomb_det : MonoBehaviour {

	public float cooldown = 60;
	bool canfire;
	public float timer;

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
			//Invoke("act",cooldown);
			timer = cooldown;
		}
		timer -= Time.deltaTime;
		if(timer < 0) act();
	}

	public bool addCharge(int c){
		bool r = timer > 0;
		timer -= 10 * c;
		return r;
	}

	void act(){
		canfire = true;
	}
}
