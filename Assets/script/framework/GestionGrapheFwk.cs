using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GestionGrapheFwk<Personnage>: ObjetPhysique {

	protected GrapheFwk<Personnage> graphe;

	protected EtatFwk<Personnage> etatCourant;

	protected Context<Personnage> context;

	protected Touchers _touchers;
	protected Touchers touchers;
	protected System.Object lockToucher = new object();

	protected void init(GrapheFwk<Personnage> graphe, EtatFwk<Personnage> etatCourant){
		this.init (graphe);
		this.etatCourant = etatCourant;
	}

	protected void init(GrapheFwk<Personnage> graphe){
		this.graphe = graphe;
		this._touchers = new Touchers ();
		this.touchers = new Touchers ();
		this.context = new Context<Personnage> (graphe.getPersonnage(), this.touchers, graphe.getAnimator());
	}

	public void setEtatCourant(EtatFwk<Personnage> etatCourant){
		this.etatCourant = etatCourant;
	}


	//Appele a chaque frame
	public void Gerer(){
		EtatFwk<Personnage> etatSuivant = null;

		this.copyTouchers ();
		etatSuivant = etatCourant.conditionActive ();

		if (etatSuivant == null) {
			return;//On reste sur le meme etat
		}

		lock (lockToucher) {
			ChangerEtat (etatSuivant);
		}
	}

	private void copyTouchers(){
		lock (lockToucher) {
			this.touchers.copy (this._touchers);
			this._touchers.clearTouchers ();
		}
	}

	public void addToucherInterne(ToucherInterne t){
		lock (lockToucher) {
			if(this.etatCourant.filtreToucher(t)){
				this._touchers.addToucherInterne (t);
			}
		}
	}

	public void addToucherExterne(ToucherExterne t){
		lock (lockToucher) {
			//Filtre sur toucherExterne ???
			this._touchers.addToucherExterne (t);
		}
	}

	private void ChangerEtat(EtatFwk<Personnage> etatSuivant){
		etatCourant = etatSuivant;
		graphe.setAnimation (etatCourant);
	}

	public Context<Personnage> getContext(){
		return this.getContext ();
	}

	protected override void ComputeVelocity()
	{
		this.Gerer ();
	}

	protected override void toucheInterne(ToucherInterne t)
	{
		this.addToucherInterne (t);
	}

}
