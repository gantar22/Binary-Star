using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angle { // represents an angle
					 //add stuff as you want more features

	public float theta;
	public enum angle_mode {radians,degrees}
	public angle_mode mode;
	public Angle(float theta){
		this.theta = theta % 360;
		this.mode = angle_mode.degrees;
	}
	public Angle(float theta, angle_mode mode){
		this.mode = mode;
		if(mode == angle_mode.degrees){
			this.theta = theta % 360;
		} else if(mode == angle_mode.radians){
			this.theta = theta % (2 * Mathf.PI);
		}
	}


}
