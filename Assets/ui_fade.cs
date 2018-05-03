using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_fade : MonoBehaviour {

	RectTransform rect;
	fade[] fades;
	private CanvasScaler cs;
	private Canvas canvas;
	public float top_border = 1;
	public float bottom_border = 0;
	public float right_border = 1;
	public float left_border = 0;

	// Use this for initialization
	void Start () {
		fades = GetComponentsInChildren<fade>();
		rect = GetComponent<RectTransform>();
		cs = GetComponentInParent<CanvasScaler>();
		canvas = GetComponentInParent<Canvas>();
	}
	
	// Update is called once per frame
	void Update () {

		Vector2 pos = GM.Instance.player_screen_loc();
		if(((pos.y > top_border && pos.x > right_border) || (pos.y > top_border && pos.x < left_border))
			|| ((pos.y < bottom_border && pos.x > right_border) || (pos.y < bottom_border && pos.x < left_border)) ) {
			foreach(Transform t in transform){
				Text tex;
				if(tex = t.GetComponent<Text>())
					tex.color = Color.Lerp(tex.color,Color.white * .5f,Time.deltaTime * 10);
				Image image;
				if(image = t.GetComponent<Image>())
					image.color = Color.Lerp(image.color,Color.white * .5f,Time.deltaTime * 10);
			}
			Image im;
			if(im = GetComponent<Image>())
				im.color = Color.Lerp(im.color,Color.white * .5f,Time.deltaTime * 10);

		} else {
			foreach(Transform t in transform){
				Text tex;
				if(tex = t.GetComponent<Text>())
					tex.color += Color.white * Time.deltaTime;
				Image image;
				if(image = t.GetComponent<Image>())
					image.color += Color.white * Time.deltaTime;
			}
			Image im;
			if(im = GetComponent<Image>())
				im.color += Color.white * Time.deltaTime;
		}
	}


	Rect rect_to_screen(RectTransform trans)
    {
        Vector2 size = Vector2.Scale(trans.rect.size, trans.lossyScale);
        Rect rect = new Rect(trans.position.x, Screen.height - trans.position.y, size.x, size.y);
        rect.x -= (trans.pivot.x * size.x);
        rect.y -= ((1.0f - trans.pivot.y) * size.y);
        float width = Camera.main.orthographicSize;
        float height = width * Camera.main.aspect;
        rect.x /= width + .5f;
        rect.y /= height + .5f;
        rect.width /= width;
        rect.height /= height;
        return rect;
    }



    Rect GetWorldRect (RectTransform rt, Vector2 scale) {
  
        // Convert the rectangle to world corners and grab the top left
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        Vector3 topLeft = corners[0];
  
        // Rescale the size appropriately based on the current Canvas scale
        Vector2 scaledSize = new Vector2(scale.x * rt.rect.size.x, scale.y * rt.rect.size.y);
        return new Rect(topLeft, scaledSize);
  
    }
}
