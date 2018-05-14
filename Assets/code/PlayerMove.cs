using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public enum gear {brake, normal, boost}

public class PlayerMove : MonoBehaviour {

	[SerializeField]
	XboxController ctlr;
	[SerializeField]
	float _speed = 20;
	[SerializeField]
	bool _lockedInCamera;
	[SerializeField]
	bool drag;
	[SerializeField]
	float max_heat = 5;

	// Base sprint heat values
	private static float baseHeat_per_sec = 5;
	private static float baseHeat_decay = 1;
	// Real, current values
	private static float heat_per_sec;
	private static float heat_decay;

	private static float dash_length = 20;
	private static float dash_width = 1;
	private static float dash_cooldown = 8;
	private static bool  dash_enabled = false;
	public  static bool  dash_kill = false;




	private bool dash_off_cooldown = false;
	private float dash_timer;

	private float heat;
	private bool cooldown;

	private Vector2 joy;
	private bool stunned;
	[HideInInspector]
	public Vector2 velo;
	private gear _gear = gear.normal; 
	private float _eSpeed; //effective speed
	[SerializeField]
	private float _boostFactor = 2.5f;

	private Rigidbody2D rb;
	private PlayerHP PHP;

	private Bounds colliding_bounds = new Bounds(Vector3.zero,Vector3.zero);



	// Initialization
	void Start () {
		if (heat_per_sec == 0f || heat_decay == 0f) {
			heat_per_sec = baseHeat_per_sec;
			heat_decay = baseHeat_decay;
		}
		rb = GetComponent<Rigidbody2D>();
		PHP = GetComponent<PlayerHP> ();
	}

	// Update is called once per frame
	void Update () {
		joy = new Vector2(0,0);
		#if UNITY_EDITOR
		keyboard();
		#endif

		joy += new Vector2(XCI.GetAxisRaw(XboxAxis.LeftStickX,ctlr),XCI.GetAxisRaw(XboxAxis.LeftStickY,ctlr));

			//only apply to player
		if(PHP && (XCI.GetButton(XboxButton.A,ctlr)
			#if UNITY_EDITOR
			|| Input.GetKey(KeyCode.LeftShift)
			#endif
		) && !cooldown) _gear = gear.boost;
		else _gear = gear.normal;
		
		if(dash_enabled && PHP && (XCI.GetButton(XboxButton.X,ctlr)
			#if UNITY_EDITOR
			|| Input.GetKeyDown(KeyCode.F)
			#endif
		) && !PlayerHP.invuln && dash_off_cooldown){
			StartCoroutine(dash(dash_length,dash_width));
		}


		if(!stunned && (!turtle_script.currently_active || turtle_script.move)) move();

		dash_timer = Mathf.Clamp(dash_timer - Time.deltaTime, -1, dash_timer);
		if(dash_timer < 0)	dash_off_cooldown = true;

		CameraUtil.keepInCamBounds (transform.root);
	}

	public static float x_cooldown(){
		if(dash_enabled) return 1 - (GM.Instance.player.GetComponentInChildren<PlayerMove>().dash_timer / dash_cooldown);
		if(turtle_script.unlocked) return turtle_script.meter();
		else return 0;
	}

	public void resetDashCooldown() {
		dash_timer = 0;
	}

	// Upgrade sprint cooldown
	public static void UpgradeSprintCooldown (int total) {
		if (total == 0) {
			heat_per_sec = baseHeat_per_sec;
			heat_decay = baseHeat_decay;
		} else {
			heat_per_sec *= 0.9f;
			heat_decay *= 1.1f;
		}
	}


	// Upgrades

	public static void UpgradeDashWidth(int total) {
		if(total == 0) {
			PlayerMove.dash_width = 1;
		} else {
			PlayerMove.dash_width = 2.3f;
		}

	}

	public static void UpgradeDashCoolDown(int total){
		if(total == 0) {
			dash_cooldown = 8;
		} else {
			dash_cooldown = 3;
		}
	}

	public static void UpgradeDashEnabled(int total){
		if(total == 0){
			dash_enabled = false;
		} else {
			dash_enabled = true;
		}

	}

	public static void UpgradeDashLength(int total){
		if(total == 0){
			dash_length = 20;
		} else {
			dash_length = 50;

		}
	}

	public static void UpgradeDashKill(int total){
		dash_kill = total == 1;
	}

	void move(){
		heat -= heat_decay * Time.deltaTime;
		if(cooldown) heat -= heat_decay * Time.deltaTime * 2;
		if(heat < 0) reload();
		switch(_gear){
			case gear.brake:
				_eSpeed = 0;
				break;
			case gear.normal:
				_eSpeed = _speed;
				break;
			case gear.boost:
				if(!cooldown){
					heat += heat_per_sec * Time.deltaTime * (velo.magnitude > .1f ? 1 : 0);
					_eSpeed = _speed * _boostFactor;				
				}

				break;
			default:
				break;
		}
		if(heat > max_heat) cooldown = true;
		if(drag && _gear != gear.brake){
			if ((velo.x * joy.x < 0 && velo.y * joy.y < 0) || 
				((velo.x * joy.x < 0 && velo.y * joy.y == 0) || 
					(velo.x * joy.x == 0 && velo.y * joy.y < 0)))
			{
				stun(.1f);
				velo = Vector2.zero;
			} else if(joy.magnitude < .8f){
				velo = Vector2.Lerp(velo,joy * _eSpeed,20 * Time.deltaTime);	
			} else {
				velo = Vector2.Lerp(velo,joy * _eSpeed,(velo.magnitude < .8f ? 2 : 8) * Time.deltaTime);	
			}
		} else {
			velo = joy * _eSpeed;
		}


		if(velo.magnitude < .2f) velo = Vector2.zero;

		transform.root.Translate(velo * Time.deltaTime,Space.World);


		if(_lockedInCamera && not_percent(Camera.main.WorldToViewportPoint(transform.root.position).x)){
			transform.root.Translate(Vector2.right * velo.x * Time.deltaTime * -1,Space.World);
		}
		if(_lockedInCamera && not_percent(Camera.main.WorldToViewportPoint(transform.root.position).y)){
			transform.root.Translate(Vector2.up * velo.y * Time.deltaTime * -1,Space.World);
		}

		/* Causing the player body separation bug:
		if(not_percent(Camera.main.WorldToViewportPoint(transform.root.position).y)){
			transform.root.position = Vector3.zero;
		} */

		/*********************************************/


		if(colliding_bounds.size.x > 0){
				uncollide();



			colliding_bounds = new Bounds(Vector3.zero,Vector3.zero);
		}




		/*******************************************/
		
		// Make sure the player doesn't go on top of asteroids

		if (Asteroid.asteroids != null && PHP) {
			foreach (Asteroid ast in Asteroid.asteroids) {
				Vector2 rockPos = ast.rock.transform.position;
				Vector2 playerPos = transform.root.position;
				float rockRadius = ast.rock.circCollider.radius * ast.rock.transform.lossyScale.x;
				// Adjust this to adjust how close the player can get to asteroids:
				float playerRadius = 2f;
					
				Vector2 diff = playerPos - rockPos;
				if (diff.magnitude < rockRadius + playerRadius) {
					PHP.gotHit ();
						
					Vector2 newPos = diff.normalized * (rockRadius + playerRadius) + rockPos;
					transform.root.Translate ((Vector3)newPos - transform.root.position, Space.World); // Test this
				}
			}
		}
	}

	static bool not_percent(float x){
		return x < -0 || x > 1;
	}

	public void stun(float t){
		stunned = true;
		Invoke("unstun",t);
	}

	public void unstun(){
		stunned = false;
	}

	void reload(){
		heat = 0;
		cooldown = false;
	}


	public float GetHeat(){
		return heat / max_heat;
	}

	public void heat_refund(){
		heat -= heat_per_sec;
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






	IEnumerator dash(float dash_length,float dash_width){
			music_manager.play_by_name("dash");
			dash_off_cooldown = false;
			dash_timer = dash_cooldown;
			float dur = .1f;
			float timer = dur;
			float step_size = dash_length / dur;
			GetComponentInParent<squeeze>().enabled = false;
			PlayerHP.dashing = true;
			BoxCollider2D col = GetComponent<BoxCollider2D>();
			Vector2 old_size = col.size;
			col.size = new Vector2(old_size.x,dash_width);
			TrailRenderer trail = GetComponentInChildren<TrailRenderer>();
			float old_time = trail.time;
			float old_width = trail.startWidth;
			trail.time = 1;
			trail.widthMultiplier = dash_width * 10;
			while(timer > 0){

				timer -= Time.deltaTime;
				transform.root.position	+= transform.root.right * step_size * Time.deltaTime;
				if(not_percent(Camera.main.WorldToViewportPoint(transform.root.position).x)
					|| not_percent(Camera.main.WorldToViewportPoint(transform.root.position).y)){
					transform.root.position	+= transform.root.right * step_size * -Time.deltaTime * 1.15f;
				}
				yield return null;
			}

			col.size = old_size;
			GetComponentInParent<squeeze>().enabled = false;
			timer = .5f;
			while(timer > 0){
				timer -= Time.deltaTime;
				trail.time = Mathf.Lerp(trail.time, old_time,1 - 2 * timer);
				trail.widthMultiplier = Mathf.Lerp(trail.widthMultiplier,old_width,1 - 2 * timer);
				yield return null;
			}
			PlayerHP.dashing = false;
			yield return null;
	}


	public void activate_pika(){
		StartCoroutine(pika());
	}

	IEnumerator pika(){

		GetComponentInParent<squeeze>().enabled = false;
		TrailRenderer trail = GetComponentInChildren<TrailRenderer>();
		float old_time = trail.time;
		trail.time = 1;
		float old_width = trail.widthMultiplier;
		trail.widthMultiplier = 10;
		PlayerHP.dashing = true;
		BoxCollider2D col = GetComponent<BoxCollider2D>();
		Vector2 old_size = col.size;
		col.size = new Vector2(col.size.x * 3, col.size.y * 3);


		yield return new WaitForSeconds(6);

		PlayerHP.dashing = false;
		col.size = old_size;
		trail.time = old_time;
		trail.widthMultiplier = old_width;

		yield return  null;
	}




	void uncollide(){

			BoxCollider2D col = GetComponent<BoxCollider2D>();
			float bottom = colliding_bounds.min.y;
			float top    = colliding_bounds.max.y;
			float left   = colliding_bounds.min.x;
			float right  = colliding_bounds.max.x;

			Vector3 player_pos = transform.position;
			Transform pt = GM.Instance.player.transform;
			float dif_bottom = player_pos.y - bottom;
			float dif_top    = top - player_pos.y;
			float dif_left   = player_pos.x - left;
			float dif_right  = right - player_pos.x;

			float min_dis = Mathf.Min(new float[] {dif_bottom,dif_top,dif_left,dif_right});
			
			if(min_dis == dif_bottom){
				//transform.root.Translate(Vector2.up * velo.y * Time.deltaTime * -1,Space.World);
				//if(colliding_bounds.Intersects(col.bounds)){
					transform.root.position = new Vector3(transform.root.position.x,bottom - col.size.y * .51f * transform.lossyScale.y,0);
				//}
			}
			if(min_dis == dif_top){
				//transform.root.Translate(Vector2.up * velo.y * Time.deltaTime * -1,Space.World);
				//if(colliding_bounds.Intersects(col.bounds)){
					transform.root.position = new Vector3(transform.root.position.x,top + col.size.y * .51f * transform.lossyScale.y,0);
				//}
			}
			if(min_dis == dif_left){
				//transform.root.Translate(Vector2.right * velo.x * Time.deltaTime * -1,Space.World);
				//if(colliding_bounds.Intersects(col.bounds)){
					transform.root.position = new Vector3(left - col.size.x * .51f * transform.lossyScale.x,transform.root.position.y,0);
				//}
			}
			if(min_dis == dif_right){
				//transform.root.Translate(Vector2.right * velo.x * Time.deltaTime * -1,Space.World);
				//if(colliding_bounds.Intersects(col.bounds)){
					transform.root.position = new Vector3(right + col.size.x * .51f * transform.lossyScale.x,transform.root.position.y,0);
				//}
			}
			if(colliding_bounds.Intersects(col.bounds)) uncollide();
	}

	void OnTriggerStay2D(Collider2D other){
		if(other.gameObject.GetComponent<no_collide_with_player>()){
			colliding_bounds = other.bounds;
		}
	}
}
