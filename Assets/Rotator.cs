using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class Rotator : MonoBehaviour {
	[SerializeField]
	float turnRadius;
	[SerializeField]
	XboxController ctlr;

	private float targetTheta;
	private float deltaTheta;
	private Vector2 joy;
	private float prevTargetTheta = 0;
	
	// Update is called once per frame
	void Update () {
		
		keyboard();

		joy += new Vector2(XCI.GetAxisRaw(XboxAxis.LeftStickX,ctlr),XCI.GetAxisRaw(XboxAxis.LeftStickY,ctlr));
		if (joy.x == 0 && joy.y == 0) return;
		if(joy.y == 0) targetTheta = 180 * joy.x * -1;
		targetTheta = joy.x == 0 ? 90 * joy.y : (360 / (2 * Mathf.PI)) * Mathf.Atan(joy.y / joy.x);

		if(joy.x < 0) targetTheta += 180;
		if(targetTheta < 0) targetTheta += 360;

		if(joy.x > 0) {
			if((transform.eulerAngles.z > 270 || transform.eulerAngles.z == 0)&& joy.y > 0){
				targetTheta += 360; 
			} else if(transform.eulerAngles.z < 180 && joy.y < 0){
				targetTheta -= 360;
			} 
		}

		//deltaTheta = (targetTheta - transform.eulerAngles.z) % 360;

		deltaTheta = Mathf.LerpAngle(transform.eulerAngles.z,targetTheta,Time.deltaTime * turnRadius);

		transform.Rotate(Vector3.forward * deltaTheta - transform.eulerAngles);//Mathf.Clamp(deltaTheta,-1 * turnRadius,turnRadius));
		
		prevTargetTheta = targetTheta;
	}

	float mod(float m,float n){
		return m % n < 0 ? m % n + n : m % n;
	}


	void keyboard(){
		if(ctlr == XboxController.First){
			joy.x = (Input.GetKey(KeyCode.D) ? 1 : 0) + (Input.GetKey(KeyCode.A) ? -1 : 0);
			joy.y = (Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S) ? -1 : 0);
		} else {
			joy.x = (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) + (Input.GetKey(KeyCode.LeftArrow) ? -1 : 0);
			joy.y = (Input.GetKey(KeyCode.UpArrow) ? 1 : 0)    + (Input.GetKey(KeyCode.DownArrow) ? -1 : 0);
		}
	}
}
