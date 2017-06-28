using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToucherInterne{

	private RaycastHit2D touche;

	private Vector2 velocity;

	public ToucherInterne(RaycastHit2D touche, Vector2 velocity) {
		this.touche = touche;
		this.velocity = velocity;
	}

	public RaycastHit2D getTouche(){
		return this.touche;
	}

	public Vector2 getVelocity(){
		return this.velocity;
	}
}
