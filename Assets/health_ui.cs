using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health_ui : MonoBehaviour {


	
	public RectTransform box;
	public GameObject hp_prefab;
	Vector3 init_pos;
	private int old_hp;
	GameObject[] hp_objs = new GameObject[0];

	// Use this for initialization
	void Start () {
		init_pos = box.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (!GM.Instance.player) {
			return;
		}

		int hp = GM.Instance.player.GetComponentInChildren<PlayerHP>().getHP();
		box.position = init_pos + Vector3.right * (hp - 1) * 9;
		box.sizeDelta = new Vector2(100 + 50 * (hp - 1),100);


		if(old_hp != hp){
			for(int i = 0; i < hp_objs.Length;i++){
				Destroy(hp_objs[i]);
			}
			hp_objs = new GameObject[hp];
			for(int i = 0; i < hp; i++){
				hp_objs[i] = Instantiate(hp_prefab,transform);
				hp_objs[i].transform.position = init_pos + Vector3.left * hp_objs[0].GetComponent<RectTransform>().rect.width * .6f;
				hp_objs[i].transform.position += Vector3.right * hp_objs[0].GetComponent<RectTransform>().rect.width * .6f * i;


			}
		}


		old_hp = hp;
	}
}
