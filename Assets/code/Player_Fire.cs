using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class Player_Fire : MonoBehaviour {

	[SerializeField]
	XboxController _ctlr;
	[SerializeField]
	XboxButton _button;
	[SerializeField]
	GameObject _bullet;

	[SerializeField]
	float _offset;
	[SerializeField]
	[Range(.1f,10)]
	float _fire_rate; //per second
	[SerializeField]
	float max_heat = 5;
	[SerializeField]
	float heat_per_shoot = 1;
	[SerializeField]
	float heat_decay = 1;

//TODO heat_per_shoot / (heat_decay * max_heat) * .5f = firerate for hold

	private float heat;


	private bool cool_down;
	private int bullets_fired;

	[HideInInspector]
	public bool cantFire;

	
	// Update is called once per frame
	void Update () {
		heat -= heat_decay * Time.deltaTime;
		if(cool_down) heat -= heat_decay * Time.deltaTime * 2;
		if(heat < 0) reload();
		if((XCI.GetButton(_button,_ctlr) || Input.GetKeyDown(KeyCode.Space)) && !cool_down){
			fire();
		}		
	}


	public float GetHeat(){
		return heat / max_heat;
	}

	public void heat_refund(){
		heat -= heat_per_shoot;
	}


	void fire(){
		if (cantFire) {
			// Play "click" noise here
			return;
		}

		music_manager.Instance.shot();
		GetComponentInChildren<ParticleSystem>().Play();

		CameraShakeScript CSS = Camera.main.GetComponent<CameraShakeScript> ();
		if(CSS != null){
			//CSS.activate(.2f,.05f); this feels bad
		}


		
		heat += heat_per_shoot;
		if(heat > max_heat) cool_down = true;
		
		float a = transform.eulerAngles.z * 2 * Mathf.PI / 360 ;
		GameObject bul = Instantiate(_bullet,transform.position + new Vector3(Mathf.Cos(a),Mathf.Sin(a),0) * _offset,transform.rotation);
		bul.GetComponentInChildren<ObjT>().id = bullets_fired++;
		bul.GetComponentInChildren<linear_travel>().setSpeed(transform.root.gameObject.GetComponentInChildren<PlayerMove>().velo.magnitude);

	}

	void reload(){
		heat = 0;
		cool_down = false;
	}
}
