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
	private float last_fire;

//TODO heat_per_shoot / (heat_decay * max_heat) * .5f = firerate for hold

	private float heat;


	private bool cool_down;
	private int bullets_fired;

	[HideInInspector]
	public bool cantFire;

	// Upgrade properties
	private static int multishotCount = 1;
	private static int ricochet = 0;
	private static int cooldownReduction = 0;
	private static int widthUpgrades = 0;


	void Start(){
		last_fire = Time.time;

		// Testing
		//UpgradeMultishot(7);
	}
	
	// Update is called once per frame
	void Update () {
		heat -= heat_decay * Time.deltaTime;
		if(cool_down) heat -= heat_decay * Time.deltaTime * 2;
		if(heat < 0) reload();
		if((XCI.GetButtonDown(_button,_ctlr) || Input.GetKeyDown(KeyCode.Space)) && !cool_down){
			fire();
		}	
		if((XCI.GetButton(_button,_ctlr) || Input.GetKey(KeyCode.Space)) && !cool_down){
			if(Time.time - last_fire > (heat_decay * max_heat * .1f)) fire();
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
		last_fire = Time.time;
		music_manager.Instance.shot();
		GetComponentInChildren<ParticleSystem>().Play();

		CameraShakeScript CSS = Camera.main.GetComponent<CameraShakeScript> ();
		if(CSS != null){
			CSS.activate(.01f,.05f); //this feels bad
		}


		
		heat += heat_per_shoot;
		if(heat > max_heat) cool_down = true;
		
		/* float a = transform.eulerAngles.z * 2 * Mathf.PI / 360 ;
		GameObject bul = Instantiate(_bullet,transform.position + new Vector3(Mathf.Cos(a),Mathf.Sin(a),0) * _offset,transform.rotation);
		bul.GetComponentInChildren<ObjT>().id = bullets_fired++;
		bul.GetComponentInChildren<linear_travel>().setSpeed(transform.root.gameObject.GetComponentInChildren<PlayerMove>().velo.magnitude); */

		float totalDegrees = 0;
		float degIncr = 0;
		for (int i = 0; i < multishotCount - 1; i++) {
			totalDegrees += Mathf.Max (20 - i * 2, 0);
			degIncr = totalDegrees / (multishotCount - 1);
		}
		totalDegrees *= Mathf.Deg2Rad;
		degIncr *= Mathf.Deg2Rad;

		float a = transform.eulerAngles.z * Mathf.Deg2Rad;
		for (int i = 0; i < multishotCount; i++) {
			float angleDiffInRad = (totalDegrees / -2f) + degIncr * i;
			float localA = a - angleDiffInRad;

			Vector3 offset = new Vector3 (Mathf.Cos (localA), Mathf.Sin (localA), 0) * _offset;
			spawnBullet (transform.position + offset, angleDiffInRad * Mathf.Rad2Deg);
		}
	}

	void spawnBullet (Vector3 pos, float angleDiffDegrees) {
		GameObject bul = Instantiate(_bullet, pos, transform.rotation);
		bul.transform.Rotate(new Vector3(0, 0, angleDiffDegrees));

		bul.GetComponentInChildren<linear_travel>().setSpeed(transform.root.gameObject.GetComponentInChildren<PlayerMove>().velo.magnitude);
		bul.GetComponentInChildren<ObjT>().id = bullets_fired++;
	}

	void reload(){
		heat = 0;
		cool_down = false;
	}

	// ===== UPGRADES =====

	public static void UpgradeMultishot (int total) {
		multishotCount = total + 1;
	}
}
