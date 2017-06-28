using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EtatFwk<Personnage> {

	//Nom de l'animation et du trigger(condition dans Animator)
	private string animation;

	private Context<Personnage> context;

	private List<ConditionFwk<Personnage>> conditions;

	private Func<ToucherInterne,bool> filtreSurToucherInterne;

	public EtatFwk(string animation,  Context<Personnage> context){
		this.animation = animation;
		this.conditions = new List<ConditionFwk<Personnage>> ();
		this.context = context;
		this.filtreSurToucherInterne = delegate(ToucherInterne t) {
			return true;
		};
	}

	public void addCondition(Func<Context<Personnage>,bool> condition, Func<Context<Personnage>,bool> action, bool rester){
		this.conditions.Add (new ConditionFwk<Personnage>(context,condition,action,rester));
	}

	public void addCondition(EtatFwk<Personnage> suivant, Func<Context<Personnage>,bool> condition, Func<Context<Personnage>,bool> action){
		this.conditions.Add (new ConditionFwk<Personnage>(context,suivant,condition,action));
	}

	public void addCondition(EtatFwk<Personnage> suivant, Func<Context<Personnage>,bool> condition){
		this.conditions.Add (new ConditionFwk<Personnage>(context,suivant,condition));
	}

	public EtatFwk<Personnage> conditionActive(){
		foreach (ConditionFwk<Personnage> c in conditions) {
			if(c.appliqueCondition()){
				c.appliqueAction ();
				if(!c.getRester()){
					return c.getSuivant ();
				}
			}
		}
		return null;
	}

	public string getAnimation(){
		return this.animation;
	}

	public void setFiltreSurToucherInterne(Func<ToucherInterne,bool> filtreSurToucherInterne){
		this.filtreSurToucherInterne = filtreSurToucherInterne;
	}

	public bool filtreToucher(ToucherInterne t){
		return filtreSurToucherInterne (t);
	}

}
