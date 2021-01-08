using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class turtle_script : MonoBehaviour {

	[SerializeField]
	XboxController _ctlr;
	[SerializeField]
	XboxButton _button;

	public GameObject ghost;

	private static turtle_script instance;

	public static bool unlocked, decoy, incr_dur, move, refl;

	public static float old_dur = 1.5f, new_dur = 3.0f;

	public static float dur = old_dur;

	public static bool currently_active;

	float cooldown;

	float max_cooldown = 2;


	// Use this for initialization
	void Start () {
		instance = this;

		currently_active = false;

	}
	
	// Update is called once per frame
	void Update () {
		if(!unlocked)
			return;

		if(cooldown > 0) {
			if(!currently_active) cooldown -= Time.deltaTime;
		} else if (!GM.Instance.player.GetComponentInChildren<Player_Fire>().cantFire) {
			if (XCI.GetButtonDown (_button, _ctlr)) {
				turtle();
			}
			#if UNITY_EDITOR
			else if (Input.GetKey (KeyCode.X)) { // KEYBOARD TESTING --- REMOVE
				turtle();
			}
			#endif
		}	
	}

	static void turtle(){
		instance.StartCoroutine(instance.go());
		instance.cooldown = instance.max_cooldown;
	}


	public static float meter(){
		return 1 - instance.cooldown / instance.max_cooldown;
	}

	IEnumerator go(){
		if(decoy)
		{
			GameObject g = Instantiate(ghost,transform.position,transform.root.rotation);
			g.GetComponentInChildren<self_destruct>().dur = dur + 0.5f;
			GM.Instance.decoy = g;
		}
		currently_active = true;
		GM.Instance.player.GetComponentInChildren<Player_Fire>().cantFire = true;
		for(int i = 0; i < transform.childCount;i++)
		{
			transform.GetChild(i).gameObject.SetActive(true);
		}

		yield return new WaitForSeconds(dur);

		currently_active = false;
		GM.Instance.player.GetComponentInChildren<Player_Fire>().cantFire = false;
		for(int i = 0; i < transform.childCount;i++)
		{
			transform.GetChild(i).gameObject.SetActive(false);
		}
		if(decoy){
			print((Vector2)GM.Instance.decoy.transform.position - GM.Instance.player_pos);
			GM.Instance.decoy = null;
		}
	}



	public static void upgrade_turtle(int total){
		unlocked = false;
		if(total > 0) unlocked = true;
	}

	public static void upgrade_decoy(int total){
		decoy = false;
		if(total > 0) decoy = true;
	} 

	public static void upgrade_dur(int total){
		incr_dur = false;
		if(total == 0) dur = old_dur;
		else {
			incr_dur = true;
			dur = new_dur;
		}
	}

	public static void upgrade_move(int total){
		move = false;
		if(total > 0) move = true;
	}

	public static void upgrade_reflect(int total){
		refl = false;
		if(total > 0) refl = true;
	}
}
