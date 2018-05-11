using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeightedEnemyPhysics))]
public class bull4 : MonoBehaviour {

	// Settings/properties:
	[SerializeField]
	private float maxSpeed = 3.2f, accelMag = 0.1f;

	Animator am;

	// Object references
	private WeightedEnemyPhysics WEP;


	// Initialize
	void Start () {
		WEP = GetComponent<WeightedEnemyPhysics> ();
		WEP.maxSpeed = maxSpeed;
		am = GetComponentInChildren<Animator>();
		StartCoroutine(checkCharge());
	}

	// Called every frame
	void Update () {
		Vector2 pos = transform.position;

		if (GM.Instance.player == null) return;
		Vector2 targetPos = GM.Instance.player.transform.position;

		// Decide what direction to move in
		Vector2 direction = targetPos - pos;


		// Normalize the velocity and set to desired speed
		if(direction.magnitude > 10 && !am.GetBool("charging") )
			WEP.acceleration = direction.normalized * accelMag;
		else WEP.acceleration = direction.normalized;


	}

	IEnumerator checkCharge(){
		/* yield return new WaitForSeconds(0);
		while(true){
			yield return new WaitForSeconds(3);
			if(!am.GetBool("charging") && Random.value > .66f){
				StartCoroutine(charge());
			}
		} */

		while (true) {
			yield return new WaitUntil (() => !am.GetBool ("charging"));

			float waitTime = Random.Range (5, 10);
			yield return new WaitForSeconds (waitTime);
			StartCoroutine (charge ());
		}
	}

	IEnumerator charge(){
		transform.GetChild(1).gameObject.SetActive(false);
		am.SetBool("charging",true);
		float oldturnrate = WEP.turnRate;
		WEP.maxSpeed = 0;
		WEP.turnRate = 20;
		yield return new WaitForSeconds(1);
		yield return new WaitUntil(() => !am.GetCurrentAnimatorStateInfo(0).IsTag("wait"));
		WEP.turnRate = 0;
		WEP.velocity = transform.right * 40;
		WEP.maxSpeed = 100;
		yield return new WaitForSeconds(1);
		am.SetBool("charging",false);
		WEP.maxSpeed = maxSpeed;
		WEP.turnRate = oldturnrate;
		transform.GetChild(1).gameObject.SetActive(true);
		yield return null;
	}
}
