using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapheFwk<Personnage> {

	private string layerAnimation;

	private Personnage personnage;

	private List<EtatFwk<Personnage>> etats;

	private Animator animator;

	public GrapheFwk(string layerAnimation, Personnage personnage, Animator animator){
		this.layerAnimation = layerAnimation;
		this.personnage = personnage;
		this.etats = new List<EtatFwk<Personnage>> ();
		this.animator = animator;
	}

	public void addEtat(EtatFwk<Personnage> etat){
		this.etats.Add (etat);
	}

	public void setAnimation(EtatFwk<Personnage> etat){
		animator.SetTrigger (etat.getAnimation());
	}

	public Personnage getPersonnage(){
		return this.personnage;
	}

	public Animator getAnimator(){
		return this.animator;
	}
}
