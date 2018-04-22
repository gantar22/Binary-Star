using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum gunnerEffect {sword, missile};
public enum pilotEffect {turtle, dash};
public enum X_Ability {None, Turtle_Sword, Dash_Missile};

public class Upgrade : ScriptableObject {

	public gunnerEffect gunEffect;
	public pilotEffect pilEffect;
	public X_Ability prereq;

	[TextArea(10,20)]
	public string description;
	public Sprite icon;
}
