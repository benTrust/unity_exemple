using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToucherExterne {

	private MonoBehaviour enemie = null;

	public ToucherExterne(MonoBehaviour enemie) {
		this.enemie = enemie;
	}

	public MonoBehaviour getEnemie(){
		return this.enemie;
	}
}
