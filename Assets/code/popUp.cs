using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popUp : MonoBehaviour {

    float multiple;

	void Awake(){
        multiple = transform.localScale.x;
    	StartCoroutine(PopUp());
    }

    IEnumerator PopUp(){
    	float tp = 0; //time passed
    	float dur = .3f; //duration
    	while(tp < dur){
    		tp += Time.deltaTime;
    		transform.localScale = Vector3.Lerp(Vector3.zero, multiple * Vector3.one * 1.3f,tp / dur);

    		yield return null;
    	}
    	tp = 0;
    	dur = .1f;
    	while(tp < dur){
    		tp += Time.deltaTime;
    		transform.localScale = Vector3.Lerp(multiple * Vector3.one * 1.3f, multiple * Vector3.one,tp / dur);
    		yield return null;
    	}

    	transform.localScale = Vector3.one * multiple;
    
    }
}
