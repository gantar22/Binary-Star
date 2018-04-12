using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour {

	// Settings/properties:
	[SerializeField]
	private int maxHP = 3;
	[SerializeField]
	private float invulnTime = 3f, minFrequency = 2f, maxFrequency = 6f, minAlpha = 0.3f, maxAlpha = 0.9f;
	[SerializeField]
	private GameObject PlayerTurret;

	// Other variables
	public static bool invuln, checkColliders;
	private float invulnTimer;
	private float alphaCounter;

	// References
	private SpriteRenderer thisSR;
	private SpriteRenderer PlayerTurretSR;


	// Initialize
	void Start () {
		thisSR = GetComponent<SpriteRenderer> ();
		PlayerTurretSR = PlayerTurret.GetComponent<SpriteRenderer> ();

		GM.Instance.playerHP = maxHP;
		invuln = false;
		checkColliders = false;
	}
	
	// Called once per frame. Manages invuln timer and alpha
	void Update() {
		if (invuln) {
			//float frequency = Mathf.Lerp (minFrequency, maxFrequency, (invulnTime - invulnTimer) / invulnTime);
			float frequency = Mathf.Lerp (minFrequency, maxFrequency, Mathf.Pow((invulnTime - invulnTimer) / invulnTime, 2));
			alphaCounter += frequency * Time.deltaTime * (2f * Mathf.PI);
			float alphaScale = (Mathf.Sin (alphaCounter) + 1f) / 2f;
			float newAlpha = Mathf.Lerp (minAlpha, maxAlpha, alphaScale);

			invulnTimer -= Time.deltaTime;
			if (invulnTimer <= 0f) {
				invuln = false;
				setBothAlpha (1);
				checkColliders = true;
				Invoke ("stopCheck", 0.2f);
			} else {
				setBothAlpha (newAlpha);
			}
		}

		// Testing:
		// print (GM.Instance.playerHP);
	}


	// Check for collisions with enemies, and stay out of asteroids
	void OnTriggerEnter2D (Collider2D col){
		if (invuln) {
			return;
		}

		EnemyHP s = col.gameObject.GetComponent<EnemyHP>(); 
		if (s != null){
			gotHit();
			if (s.gameObject.GetComponent<Rock> () == null) {
				s.gotHit (1);
			}
		}

		// Bullet and HP drop collisions managed by BulletScript.cs and HPDrop.cs
	}

	// When invulnerability is over, check for anything currently in the collider
	void OnTriggerStay2D (Collider2D col) {
		if (checkColliders) {
			OnTriggerEnter2D (col);
		}
	}

	// Invoke this with a delay to stop checking colliders
	private void stopCheck () {
		checkColliders = false;
	}

	// Regain moreHP more HP, up to max, and return true. If already at max, return false
	public bool gainHP(int moreHP) {
		int currentHP = GM.Instance.playerHP;
		if (currentHP >= maxHP) {
			return false;
		} else {
			GM.Instance.playerHP = Mathf.Min (maxHP, currentHP + moreHP);
			return true;
		}
	}

	// Called when damage is taken
	public void gotHit(int dmg = 1) {
		if (invuln) {
			return;
		}

		GM.Instance.playerHP -= dmg;
		if (GM.Instance.playerHP <= 0) {
			die ();
		} else {
			invuln = true;
			invulnTimer = invulnTime;
			alphaCounter = Mathf.PI / -2f;
		}
	}

	// Player runs out of health or dies by other means
	public void die() {
		// TODO - What happens when the player dies/loses?

		CameraShakeScript CSS = Camera.main.GetComponent<CameraShakeScript> ();
		if(CSS != null){
			CSS.activate(.1f,.1f);
		}

		Destroy (gameObject.transform.parent.gameObject);
	}

	// Set the alpha of both body and pilot sprites
	private void setBothAlpha(float alpha) {
		Color prev = thisSR.color;
		prev.a = alpha;
		thisSR.color = prev;

		prev = PlayerTurretSR.color;
		prev.a = alpha;
		PlayerTurretSR.color = prev;
	}
}
