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
	private static int widthBoost = 0;




	void Start(){
		last_fire = Time.time;

		// Testing
		//UpgradeMultishot(7);
		//bullet_range.upgradeRange(5);
		//UpgradeBulletWidth(10);
		//UpgradeFireRate(10);
		//UpgradeRicochet(2);
	}
	
	// Update is called once per frame
	void Update () {
		heat -= getHeat_decay() * Time.deltaTime;
		if(cool_down) heat -= getHeat_decay() * Time.deltaTime * 2;
		if(heat < 0) reload();
		if((XCI.GetButtonDown(_button,_ctlr) || Input.GetKeyDown(KeyCode.Space)) && !cool_down && !PauseManager.paused){
			basicFire();
		}	

		if((XCI.GetButton(_button,_ctlr) || Input.GetKey(KeyCode.Space)) && !cool_down && !PauseManager.paused){
			if(Time.time - last_fire > fastestFireInterval() * 0.5f) basicFire();
		}
	}


	public float GetHeat(){
		return heat / max_heat;
	}

	public void heat_refund(){
		heat -= getHeat_per_shoot();
	}


	void shot_sound(){
		music_manager.play_by_name("shot");
	}

	// Fires a bullet according to basic Player_Fire mechanics
	void basicFire() {
		if (cantFire) {
			music_manager.play_by_name("error");
			return;
		}

		last_fire = Time.time;

		heat += getHeat_per_shoot();
		if(heat > max_heat) cool_down = true;

		fire ();
	}

	// Fires a bullet. Can be called by any means
	public void fire(){
		for(int i = 0; i < multishotCount;i++){
			Invoke("shot_sound",.06f * i);
		}
		
		GetComponentInChildren<ParticleSystem>().Play();

		CameraShakeScript CSS = Camera.main.GetComponent<CameraShakeScript> ();
		if(CSS != null){
			CSS.activate(.01f,.05f); //this feels bad
		}

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
		bul.GetComponentInChildren<BulletScript> ().ricochetsLeft = ricochet;

		// Scale up the scale.x of the bullet based on widthBoost upgrade
		Vector3 expandedScale = bul.transform.localScale;
		BoxCollider2D boxCol = bul.GetComponentInChildren<BoxCollider2D> ();
		for (int i = 0; i < widthBoost; i++) {
			expandedScale += Vector3.up * (1.5f + expandedScale.y) * 0.2f;
		}
		bul.transform.localScale = expandedScale;
	}

	void reload(){
		heat = 0;
		cool_down = false;
	}

	// Calculate the fastest sustainble fire interval
	public float fastestFireInterval() {
		return getHeat_per_shoot () / getHeat_decay ();
	}

	// Access the real, upgraded heat per shot and heat decay
	private float getHeat_per_shoot() {
		float multiplier = Mathf.Pow(0.9f, cooldownReduction);
		return heat_per_shoot * multiplier;
	}

	private float getHeat_decay() {
		float multiplier = Mathf.Pow (1.1f, cooldownReduction);
		return heat_decay * multiplier;
	}

	// ===== UPGRADES =====

	public static void UpgradeMultishot (int total) {
		multishotCount = total * 2 + 1;
	}

	public static void UpgradeBulletWidth (int total) {
		widthBoost = total;
	}

	public static void UpgradeFireRate (int total) {
		cooldownReduction = total;
	}

	public static void UpgradeRicochet (int total) {
		ricochet = total;
	}



}

