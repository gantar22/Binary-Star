using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : ScriptableObject {

	public gunnerEffect gunEffect;
	public pilotEffect pilEffect;
	public X_Ability prereq;

	[TextArea(10,20)]
	public string description;
	public Sprite iconGunner;
	public Sprite iconPilot;
}
