using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class take_hit : MonoBehaviour {

	SpriteRenderer sp;

	// Use this for initialization
	void Start () {
		sp = GetComponent<SpriteRenderer>();

		
	}



	public void hit(){
		StartCoroutine("flash");

	}

	IEnumerator flash(){
		breath b;
		if(b = GetComponent<breath>()){
			b.enabled = false;
		}
			float duration = .25f;
			float elapsedTime = 0f;
			

			while(elapsedTime < duration){

				elapsedTime += Time.deltaTime;
				float percentComplete = elapsedTime / duration;

				sp.color = new Color(1,1 - percentComplete,1 - percentComplete,.9f);
				yield return null;

			}
			yield return new WaitForSeconds(.5f);

			duration = .25f;
			elapsedTime = 0f;
			

			while(elapsedTime < duration){

				elapsedTime += Time.deltaTime;
				float percentComplete = elapsedTime / duration;

				sp.color = new Color(1,1 - percentComplete,1 - percentComplete,.9f);
				yield return null;

			}

			if(b) b.enabled = true;
			sp.color = Color.white;
	}
}
