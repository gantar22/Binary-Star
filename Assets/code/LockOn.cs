using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using System.Linq;

public class LockOn : MonoBehaviour {

	[SerializeField]
	XboxController ctlr;
	[SerializeField]
	GameObject _target;


	private float step_size = 1f; //in degrees
	private List<GameObject> enemies;
	private float hand = 1;

	void Start(){
		if(_target.GetComponent<PlayerMove>() == null){
			print("the reticle doesn't have a movement script");
			//Destroy(this,.1f);
		}
	}

	// Update is called once per frame
	void Update () {
		if(_target == null){
			Destroy(gameObject);
		}
		if(XCI.GetAxis(XboxAxis.LeftStickX,ctlr) != 0 || XCI.GetAxis(XboxAxis.LeftStickY,ctlr) != 0) {
			_target.transform.parent = null;
			_target.GetComponent<PlayerMove>().enabled = true;
		}
		if(XCI.GetButtonUp(XboxButton.LeftBumper ,ctlr)){
			hand = 1f;
			attachOld(step_size);
		}
		if(XCI.GetButtonUp(XboxButton.RightBumper,ctlr)){
			hand = -1f;
			attachOld(step_size);
			}
		
	}



	void attach(){
		
		if(_target.transform.parent != null){
			attachOld(step_size);
		} else {
			enemies = GM.Instance.enemies;
			enemies.Sort( (g1,g2) => compareDist(g1,g2) );
			if(enemies.Count > 0)
				setTarget(enemies[0].transform);
			else
				print("no targets");
		}

	}


	int compareDist(GameObject g1,GameObject g2){
		float dis1 = Mathf.Abs((_target.transform.position - g1.transform.position).sqrMagnitude);
		float dis2 = Mathf.Abs((_target.transform.position - g2.transform.position).sqrMagnitude);
		if (dis1 > dis2){
			return 1;
		} else if(dis1 == dis2) {
			return 0;
		} else {
			return -1;
		}
	}



	void attachOld(float h){ //TODO parallelize in coroutine

		RaycastHit2D r = new RaycastHit2D();
		float z = transform.eulerAngles.z;
		float dis = Camera.main.orthographicSize * 5;
		Vector2 dir;

		float degToRad = (2 * Mathf.PI) / 360;

	
		for(float i = 0; Mathf.Abs(i) < 360 && !r; i += (h * hand))
		{
			dir = new Vector2(Mathf.Cos((z + i) * degToRad),Mathf.Sin((z + i) * degToRad));

			r = Physics2D.Raycast(transform.position,dir,dis,1 << 8);
			/*if(r && r.transform == _target.transform.parent) {
				r = Physics2D.Raycast(r.transform.position,dir,dis,1 << 8);
			} */
			if (r && r.transform != _target.transform.parent){
				setTarget(r.transform);
			} else{
				r = new RaycastHit2D();
			}
		}

		
	}


	void setTarget(Transform t){
				_target.GetComponent<PlayerMove>().enabled = false;
				_target.transform.position = t.position;
				_target.transform.parent   = t;
	}



}
