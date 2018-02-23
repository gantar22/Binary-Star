using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerMove : MonoBehaviour {

	[SerializeField]
	XboxController ctlr;
	[SerializeField]
	float _speed = 20;
	[SerializeField]
	bool _lockedInCamera;
	[SerializeField]
	bool drag;

	private Vector2 joy;
	private bool stunned;
	[HideInInspector]
	public Vector2 velo;





	// Update is called once per frame
	void Update () {
		keyboard();

		joy += new Vector2(XCI.GetAxisRaw(XboxAxis.LeftStickX,ctlr),XCI.GetAxisRaw(XboxAxis.LeftStickY,ctlr));
		if(!stunned) move();
		
	}

	void move(){
		if(!drag){
			if ((velo.x * joy.x < 0 && velo.y * joy.y < 0) || 
				((velo.x * joy.x < 0 && velo.y * joy.y == 0) || 
					(velo.x * joy.x == 0 && velo.y * joy.y < 0)))
			{
				stun(.1f);
				velo = Vector2.zero;
				print("!");
			} else if(joy.magnitude < .8f){
				velo = Vector2.Lerp(velo,joy * _speed,20 * Time.deltaTime);	
			} else {
				velo = Vector2.Lerp(velo,joy * _speed,(velo.magnitude < .8f ? 2 : 8) * Time.deltaTime);	
			}
		} else {
			velo = joy * _speed;
		}


		if(velo.magnitude < .2f) velo = Vector2.zero;

		transform.root.Translate(velo * Time.deltaTime,Space.World);
		if(_lockedInCamera && not_percent(Camera.main.WorldToViewportPoint(transform.root.position).x)){
			transform.root.Translate(Vector2.right * velo.x * Time.deltaTime * -1,Space.World);
		}
		if(_lockedInCamera && not_percent(Camera.main.WorldToViewportPoint(transform.root.position).y)){
			transform.root.Translate(Vector2.up * velo.y * Time.deltaTime * -1,Space.World);
		}

		


	}

	static bool not_percent(float x){
		return x < -0 || x > 1;
	}

	void stun(float t){
		stunned = true;
		Invoke("unstun",t);
	}

	void unstun(){
		stunned = false;
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
