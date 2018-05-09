using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class sword_script : MonoBehaviour {

	[SerializeField]
	XboxController _ctlr;

	public static bool sword_enabled = false;
	private static float sword_dur = 3;
	public static bool sword_destroys_projectiles = false;
	private static float sword_range;
	private static bool sword_spins;


	public ParticleSystem ps_short;
	public ParticleSystem ps_long;
	private bool off_cooldown;
	private BoxCollider2D box;
	private float timer;
	private float cooldown_time = 20;

	// Use this for initialization
	void Start () {
		box = GetComponent<BoxCollider2D>();
		box.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(XCI.GetButton(XboxButton.X,_ctlr) && sword_enabled && off_cooldown){
			if(sword_range == 2){
				ps_short.Play();
			} else {
				ps_long.Play();
			}
			box.enabled = true;
			off_cooldown = false;
			timer = cooldown_time;
			box.size = new Vector2(sword_range,.5f);
			box.offset = new Vector2(sword_range / 2, 0);
			if(sword_spins) StartCoroutine(spin());
			Invoke("shut_down",sword_dur);
		}
		if(!box.enabled){
			timer -= Time.deltaTime;
		}
		if(timer < 0){
			off_cooldown = true;
		}
	}

	IEnumerator spin(){
		float timer = sword_dur;
		while(sword_dur > 0){
			transform.Rotate(Vector3.forward * Time.deltaTime * 10);
			yield return null;	
		}
		transform.eulerAngles = Vector3.zero;
		yield return null;

	}

	void shut_down(){
		box.enabled = false;
		ps_short.Stop();
		ps_long.Stop();
	}


	public float get_cooldown(){
		return 1 - timer / cooldown_time;
	}


	//------------------Upgrades--------------------//

	public static void upgrade_range(int i){
		if(i == 0){
			sword_range = 2;
		} else {
			sword_range = 4;
		}
	}
	public static void upgrade_enabled(int i){
		if(i == 0){
			sword_enabled = false;
		} else {
			sword_enabled = true;
		}
	}

	public static void upgrade_dur(int i){
		if(i == 0){
			sword_dur = 3;
		} else {
			sword_dur = 6;
		}
	}

	public static void upgrade_projectiles(int i){
		if(i == 0){
			sword_destroys_projectiles = false;
		} else {
			sword_destroys_projectiles = true;
		}
	}

	public static void upgrade_spins(int i){
		if(i == 0){
			sword_spins = false;
		} else {
			sword_spins = true;
		}
	}

}
