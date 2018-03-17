using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootsAtPlayer : MonoBehaviour {

	// Settings
	[SerializeField]
	private GameObject _bullet;
	[SerializeField]
	private float _offset;
	[SerializeField, Range(.01f, 10)]
	private float _fire_rate;

	// Other variables
	private bool cool_down;

	// Object references
	private GameObject player;
	private WeightedEnemyPhysics WEP;


	// Initialization
	void Start () {
		player = GM.Instance.player;
		WEP = GetComponent<WeightedEnemyPhysics> ();
	}
	
	// Called once per frame
	void Update () {
		if (!cool_down) {
			fire ();
		}
	}

	void fire() {
		cool_down = true;
		Invoke("reload",1 / _fire_rate);

		// Use angle towards player instead of actual rotation of enemy
		// Alternatively, only allow the enemy to shoot if it is actually angled towards the player
		float a = transform.eulerAngles.z * 2 * Mathf.PI / 360 ;
		GameObject bul = Instantiate(_bullet,transform.position + new Vector3(Mathf.Cos(a),Mathf.Sin(a),0) * _offset,transform.rotation);
		bul.GetComponentInChildren<linear_travel>().setSpeed(WEP.velocity.magnitude);

	}

	void reload() {
		cool_down = false;
	}
}
