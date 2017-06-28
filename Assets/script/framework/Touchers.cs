using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Touchers{

	private List<ToucherExterne> touchersExterne;

	private List<ToucherInterne> touchersInterne;


	public Touchers(){
		this.touchersExterne = new List<ToucherExterne> ();
		this.touchersInterne = new List<ToucherInterne> ();
	}

	public void addToucherInterne(ToucherInterne t){
		this.touchersInterne.Add (t);
	}

	public void addToucherExterne(ToucherExterne t){
		this.touchersExterne.Add (t);
	}

	public void clearTouchers(){
		this.touchersExterne.Clear ();
		this.touchersInterne.Clear ();
	}

	public void copy(Touchers touchers){
		this.clearTouchers ();
		foreach(ToucherExterne t in touchers.touchersExterne){
			this.touchersExterne.Add (t);
		}
		foreach(ToucherInterne t in touchers.touchersInterne){
			this.touchersInterne.Add (t);
		}
	}

	public T getMonoBehaviour<T>() where T : class{
		foreach(ToucherExterne t in touchersExterne){
			if(t.getEnemie() != null && t.getEnemie() is T){
				return t.getEnemie () as T;
			}
		}
		return null;
	}

	public ToucherInterne getToucherInterne(string tag){
		foreach(ToucherInterne t in touchersInterne){
			if(t.getTouche().transform.tag.Equals(tag)){
				return t;
			}
		}
		return null;
	}
}
