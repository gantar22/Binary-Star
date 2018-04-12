using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class bull2 : MonoBehaviour {

	public enum state {waiting = 0,charging,stunned,idle,targeting,anim}
	[SerializeField]
	float baseSpeed;
	[SerializeField]
	Transform shield_holder;
	[SerializeField]
	Transform sprite_holder;
	[SerializeField]
	TrailRenderer tr;

	private bool side; // false == left | true == right (Western bias my ass)
	private Vector3 targetpos;
	private float speed;
	[HideInInspector]
	public state _state;
	[HideInInspector]
	public int hp = 3;

	public spin ring;
	private ParticleSystem ps;

	private Transform pTrans;


	private Animator anim;

	bool left(bool dir){
		return !dir;
	}

	bool right(bool dir){
		return dir;
	}

	// Use this for initialization
	void Start () {
		Invoke("change",3);
		pTrans = GM.Instance.player.transform;
		anim = GetComponentInChildren<Animator>();
		ps = GetComponentInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		print(hp);
		if(!pTrans) return;

		switch(_state) { 
		    case state.waiting:
		    	anim.SetBool("stunned",false);
		    	anim.SetBool("charging",false);


		    	float y =  (.15f + Mathf.PingPong(Time.time * Mathf.Pow(speed,.5f) / 20, .7f));
		    	if(Mathf.Abs(transform.position.x - Camera.main.ViewportToWorldPoint(Vector3.right * (left(side) ? .1f : .9f)).x) >= 1f) y = .5f;
	    		targetpos = Camera.main.ViewportToWorldPoint(Vector3.right * (left(side) ? .1f : .9f) + Vector3.up * y);
	    		speed = baseSpeed;

		        break; 
		    case state.targeting:

		    	targetpos = Camera.main.ViewportToWorldPoint(Vector3.right * (left(side) ? .1f : .9f));
		    	targetpos = new Vector3(targetpos.x, pTrans.position.y,0);
	    		speed = baseSpeed / 2;

	    		if(Mathf.Abs(transform.position.y - pTrans.position.y) < .1f)
	    		{
	    			_state = state.stunned;
					anim.SetBool("charging",true);
	    			Invoke("charge",1.3f);
	    		}

		    	break;
		    case state.stunned: 

		   		shield_holder.gameObject.SetActive(false);		     	
		     	targetpos = transform.position;
		     	speed = 0;
		     	if(hp == 0) Invoke("unstun",.5f);

		        break;
		    case state.charging: 
		    	
		    	targetpos = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.right * (right(side) ? 0f : 1)).x,transform.position.y,0);
		    	speed += Time.deltaTime;

		    	if((left(side) && Camera.main.WorldToViewportPoint(transform.position).x > .9f) || (right(side) &&  Camera.main.WorldToViewportPoint(transform.position).x < .1f)) {
		    		side = !side;
		    		_state = state.waiting;
		    		Invoke("change",1f);
		    		speed = 0;
		    	}

		        break;
		    default:
		        break;
		}
		transform.localScale = (left(side) ? new Vector3(1,1,1) : new Vector3(-1,1,1)) * 5;
		transform.position += (targetpos - transform.position).normalized * speed * Time.deltaTime;
		transform.position = new Vector3(transform.position.x,transform.position.y,1);
	}


	void change(){
		if(_state != state.waiting) return;
		if(Random.value < .2f){
			_state = state.targeting;
		} else {
			Invoke("change",1.5f);
		}
	}

	void charge(){
		CancelInvoke("charge");
		//tr.enabled = true;
		_state = state.charging;
		speed = baseSpeed * 3f;
	}


	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.layer == 9 && _state == state.charging){
			anim.SetBool("stunned",true);
			anim.SetBool("charging",false);
			anim.SetTrigger("stun");
			_state = state.stunned;
			Invoke("unstun",3);
		}
	}

	void unstun(){
		CancelInvoke("unstun");
		shield_holder.gameObject.SetActive(true);
		hp = 3;
		_state = state.waiting;
		anim.SetBool("stunned",false);
		side = !side;
		Invoke("change",1f);
	}


	public void hit(Vector3 pos){
		shield_holder.GetComponent<Animator>().SetTrigger("shieldUp");
		//shield_holder.eulerAngles = Vector3.forward * Mathf.Atan2(pos.y - transform.position.y, pos.x - transform.position.x) * Mathf.Rad2Deg;
		shield_holder.right = pos - transform.position;
		if(right(side)) shield_holder.right = -1 * shield_holder.right;
		ps.Emit(2);
		Invoke("desheild",1);

	}


	void desheild(){
		CancelInvoke("desheild");
		shield_holder.GetComponent<Animator>().SetTrigger("shieldDown");
	}

}
