using UnityEngine;
using System.Collections;

public class CameraShakeScript : MonoBehaviour {

	private bool running;
	private Vector3 vel;
	[SerializeField]
	private float maxSpeed = 5;


	// Use this for initialization
	void Start () {
	running = false;
	}
	
	IEnumerator shake(float mag, float dur){
			float elapsedTime = 0f;
			Vector3 originalPos = new Vector3(transform.position.x,transform.position.y,transform.position.z);
			int steps = 1000;
			float stepTime = 0;
			float stepSize = dur / steps;
			float percentComplete = elapsedTime / dur;
			float damper = 1.0f;

			float PosX = Random.value * 2.0f - 1.0f;
			float PosY = Random.value * 2.0f - 1.0f;

			PosX *= mag * damper * damper;
			PosY *= mag * damper * damper;
			Vector3 target = new Vector3(originalPos.x + PosX, originalPos.y  + PosY,originalPos.z);

			while(elapsedTime < dur){

				elapsedTime += Time.deltaTime;
				stepTime += Time.deltaTime;
				if(stepTime > stepSize){
					stepTime = 0;
					percentComplete = elapsedTime / dur;
					damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f,1.0f);

					PosX = Random.value * 2.0f - 1.0f;
					PosY = Random.value * 2.0f - 1.0f;

					PosX *= mag * damper * damper;
					PosY *= mag * damper * damper;
					target = new Vector3(originalPos.x + PosX, originalPos.y  + PosY,originalPos.z);
				}

				transform.position = Vector3.SmoothDamp(transform.position, target, ref vel, (elapsedTime / dur) * .01f, 500);

				//transform.Translate(PosX,PosY,originalPos.z);
				yield return null;


			}


			transform.position = originalPos;

		
	}



	public void activate(float mag,float dur){

		if (SettingsManager.gameSettings.cameraShake) {
			StartCoroutine(shake(mag * 20,dur * .6f));
		}
	}


}
