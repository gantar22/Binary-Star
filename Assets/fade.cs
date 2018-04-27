using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof(Renderer))]
public class fade : MonoBehaviour {

	float alpha = 1;
	float tarA = 1;
	bool back = true;
	Color[] _colors;
	Renderer r;

	// Use this for initialization
	void Start () {
		r = GetComponent<Renderer>();
		_colors = new Color[r.materials.Length];
		for(int i = 0; i < _colors.Length; i++){
			_colors[i] = r.materials[i].color;
			r.materials[i].SetFloat("_Mode",3);
		}
		
	}
	
	// Update is called once per frame
	void Update () { //Optimize This!!!!!
		if(back) alpha = Mathf.Lerp(alpha,1,Time.deltaTime * 2f);
		else     alpha = Mathf.Lerp(alpha,.5f,Time.deltaTime * 5f);		

	}


	public void fadeOut(){
		back = false;
		CancelInvoke("fadeBack");
		Invoke("fadeBack",.5f);
	}

	void fadeBack(){
		back = true;
	}
}
