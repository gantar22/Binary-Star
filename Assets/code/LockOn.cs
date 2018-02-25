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


	private float step_size = .1f; //in degrees
	private List<Transform> enemies;
	private List<Transform> touched;

	void Start(){
		if(_target.GetComponent<PlayerMove>() == null){
			print("the reticle doesn't have a movement script");
			Destroy(this,.1f);
		}
	}

	// Update is called once per frame
	void Update () {
		if(XCI.GetAxis(XboxAxis.LeftStickX,ctlr) != 0 || XCI.GetAxis(XboxAxis.LeftStickY,ctlr) != 0) {
			_target.transform.parent = null;
			_target.GetComponent<PlayerMove>().enabled = true;
		}
		if(XCI.GetButtonUp(XboxButton.X,ctlr)){
			attach();
		}
		if(XCI.GetButtonUp(XboxButton.B,ctlr)){
			attach();
		}
		
	}



	void attach(){
		
		/*GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>() ;
		foreach(object go in allObjects){
			if (go.activeInHierarchy && go.GetComponent<ObjT>() != null && go.GetComponent<ObjT>().typ == obj.enemy_character){
				enemies.add(go.transform);
			}
		}*/

		


	}




	void attachOld(float h){
		
		RaycastHit2D r = new RaycastHit2D();
		float z = transform.eulerAngles.z;
		float dis = Camera.main.orthographicSize * Mathf.Pow(2,.5f);
		Vector2 dir;
	
		for(float i = 0; Mathf.Abs(i) < 360 && !r; i += h){
			dir = new Vector2(Mathf.Cos(z + i),Mathf.Sin(z + i));
			r = Physics2D.Raycast(transform.position,dir,dis,1 << 8);
			if(r && r.transform == _target.transform.parent) {
				r = Physics2D.Raycast(r.transform.position,dir,dis,1 << 8);
			}
			if (r){
				_target.GetComponent<PlayerMove>().enabled = false;
				_target.transform.position = r.transform.position;
				_target.transform.parent   = r.transform;
			}
		}
		
	}



}
