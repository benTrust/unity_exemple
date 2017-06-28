using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Context<Personnage>{

	private Personnage personnage;

	private Touchers touchers;

	private Animator animator;

	public Context(Personnage p, Touchers t){
		this.personnage = p;
		this.touchers = t;
	}

	public Context(Personnage p, Touchers t, Animator a) : this(p,t){
		this.animator = a;
	}

	public bool animationFini(){
		return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
	}

	public Personnage getPersonnage(){
		return this.personnage;
	}

	public Touchers getTouchers(){
		return this.touchers;
	}

}
