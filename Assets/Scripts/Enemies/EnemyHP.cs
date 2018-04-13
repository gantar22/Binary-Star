using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExplosionType {None, Small, Medium, Large};

public class EnemyHP : MonoBehaviour {

	// Settings/properties:
	[SerializeField]
	private int maxHP = 2;
	[SerializeField]
	private bool guaranteeDrop = false, diesToBomb = true;
	[SerializeField]
	private ExplosionType explosionType;

	// Other variables
	private int HP;


	// Initialize
	void Start () {
		HP = maxHP;
		GM.Instance.Spawn (gameObject);
	}

	// Called when damage is taken
	public void gotHit(int dmg) {
		UnParentOnDestroy retScript;
		if(special()) HP -= dmg;
		if (HP <= 0) {
			die ();
		} else { //hit animation
			take_hit th;
			if(th = GetComponentInChildren<take_hit>()){
				th.hit();
			}
		}

	}

	// Called when the player bomb explosion collides
	public void hitByBomb() {
		if (diesToBomb) {
			Invoke("die",Random.value * .5f);
		}
	}

	// Called when this enemy should die
	public void die() {
		music_manager.Instance.die();
		// Make sure reticle is unparented
		UnParentOnDestroy retScript;
		if((retScript = GetComponentInChildren<UnParentOnDestroy>()) != null){
			retScript.gameObject.transform.parent = null;
		}

		// Camera shake
		CameraShakeScript CSS = Camera.main.GetComponent<CameraShakeScript> ();
		if(CSS != null){
			CSS.activate(.3f,.3f);
		}

		playExplosion ();

		// Spawn drops
		if(GetComponent<bull2>()){
			for(int i = 0; i < 10;i++) {
				DropManager.Instance.SpawnDrop((Vector3)(Random.insideUnitCircle) * transform.localScale.x + transform.position);
			}
		} else if (guaranteeDrop) {
			DropManager.Instance.SpawnDrop (transform.position);
		} else {
			DropManager.Instance.MaybeDrop (maxHP, transform.position);
		}

		// If this is an asteroid (rock), blow up the whole thing
		Rock rock = GetComponent<Rock>();
		if (rock != null) {
			rock.asteroid.killTurrets ();
		}

		// If this has fireworks enabled, explode with bullets!
		Fireworks boom = GetComponent<Fireworks>();
		if (boom != null && boom.enabled) {
			boom.Explode ();
		}

		GM.Instance.Died (gameObject);
		Destroy (gameObject);
	}

	// Spawn an explosion animation
	private float playExplosion() {
		if (explosionType == ExplosionType.None) {
			return 0f;
		}

		Explosion newExplosion = Instantiate (PrefabManager.Instance.explosion, transform.position, transform.rotation);
		if (explosionType == ExplosionType.Small) {
			newExplosion.transform.localScale = new Vector3 (6f, 6f, 1f);
		} else if (explosionType == ExplosionType.Medium) {
			newExplosion.transform.localScale = new Vector3 (12f, 12f, 1f);
		} else if (explosionType == ExplosionType.Large) {
			newExplosion.transform.localScale = new Vector3 (18f, 18f, 1f);
		}

		return PrefabManager.Instance.explosion.duration;
	}

	private bool special(){ //special script checks for taking damage
		bull2 b;
		if((b = GetComponent<bull2>())){
			if(b._state != bull2.state.stunned) return false;
			if(transform.childCount > 0 && transform.GetChild(0).GetComponent<Animator>()) transform.GetChild(0).GetComponent<Animator>().SetTrigger("hit");
			if(b.ring) b.ring.speed += 7;
			return (--b.hp == 0);
		}

		SleeperEnemy se;
		if(se = GetComponent<SleeperEnemy>()){
			se.hit();
			return true;
		}
		return true;
	}
}
