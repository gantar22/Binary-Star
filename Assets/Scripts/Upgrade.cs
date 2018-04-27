using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum gunnerEffect {sword, missile};
[System.Serializable]
public enum pilotEffect {turtle, dash};
[System.Serializable]
public enum X_Ability {None, Turtle_Sword, Dash_Missile};

[CreateAssetMenu]
public class Upgrade : ScriptableObject {

	public gunnerEffect gunEffect;
	public pilotEffect pilEffect;
	public X_Ability prereq;

	[TextArea(10,20)]
	public string description;
	public Sprite pilot_icon;
	public Sprite gunner_icon;
}
