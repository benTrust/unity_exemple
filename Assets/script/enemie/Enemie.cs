using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemie : MonoBehaviour {

	public GameObject go;
	private Guerrier guerrier;


	void Start () {
		guerrier = go.GetComponent<Guerrier> ();
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.E)){
			Debug.Log ("A l'attaque!");
			guerrier.addToucherExterne (new ToucherExterne (this));
		}
	}
}
