using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Guerrier : GestionGrapheFwk<Guerrier> {

	private Animator animator;
	private SpriteRenderer spriteRenderer;

	private float maxSpeed = 7f;
	private float maxSaut = 4f;

	private float direction = 1f;

	protected override void start(){
		this.animator = GetComponent<Animator> ();
		spriteRenderer = GetComponent<SpriteRenderer> (); 

		this.creerGraphe ();
	}

	private void creerGraphe(){
		GrapheFwk<Guerrier> graphe = new GrapheFwk<Guerrier> ("guerrier", this, this.animator);

		this.init (graphe);

		EtatFwk<Guerrier> etatIdle = new EtatFwk<Guerrier> ("idle", this.context);
		this.setEtatCourant (etatIdle);

		graphe.addEtat (etatIdle);

		EtatFwk<Guerrier> etatCourir = new EtatFwk<Guerrier> ("courir", this.context);



		EtatFwk<Guerrier> etatGrimper = new EtatFwk<Guerrier> ("grimper", this.context);

		Func<Context<Guerrier>,bool> cond_courir_grimper = delegate(Context<Guerrier> context) {
			return context.getTouchers().getToucherInterne(Tag.MUR) != null || (Input.GetKeyDown(KeyCode.X) && context.getPersonnage().touche(Tag.NON_BLOQUANT));
		};

		Func<Context<Guerrier>,bool> action_courir_grimper = delegate(Context<Guerrier> context) {
			context.getPersonnage().enableGravite(false);
			context.getPersonnage().mouvementX(0f,0f);
			return true;
		};

		etatCourir.addCondition (etatGrimper, cond_courir_grimper, action_courir_grimper);

		etatIdle.addCondition (etatGrimper, cond_courir_grimper, action_courir_grimper);


		Func<Context<Guerrier>,bool> cond_grimper_courir = delegate(Context<Guerrier> context) {
			return Mathf.Abs(context.getPersonnage().velocity.x) > 0;
		};

		Func<Context<Guerrier>,bool> action_grimper_courir = delegate(Context<Guerrier> context) {
			context.getPersonnage().enableGravite(true);
			Guerrier g = context.getPersonnage();
			if(g.velocity.x != 0f){
				g.setDirection( g.velocity.x > 0f ? 1f : -1f );
				g.setSpriteRendererX(g.direction == -1f);
			}
			return true;
		};







		Func<Context<Guerrier>,bool> cond_grimperSaut = delegate(Context<Guerrier> context) {
			return Input.GetButtonDown ("Jump");
		};

		Func<Context<Guerrier>,bool> action_grimperSaut = delegate(Context<Guerrier> context) {
			Guerrier g = context.getPersonnage();
			float x = -10 * g.getDirection();
			g.mouvementX(x,10);
			g.saute(3);
			return true;
		};

		etatGrimper.addCondition (cond_grimperSaut, action_grimperSaut, true);

		etatGrimper.addCondition (etatCourir, cond_grimper_courir,action_grimper_courir);




		graphe.addEtat (etatCourir);

		Func<Context<Guerrier>,bool> cond_idle_courir = delegate(Context<Guerrier> c) {
			return Mathf.Abs(context.getPersonnage().velocity.x) > 0;
		};

		etatIdle.addCondition (etatCourir, cond_idle_courir);

		Func<Context<Guerrier>,bool> cond_courir_idle = delegate(Context<Guerrier> context) {
			return Mathf.Abs(context.getPersonnage().velocity.x) == 0;
		};

		etatCourir.addCondition (etatIdle, cond_courir_idle);

		Func<Context<Guerrier>,bool> cond_avancer = delegate(Context<Guerrier> context) {
			return context.getPersonnage().toucheSol();
		};

		Func<Context<Guerrier>,bool> action_avancer = delegate(Context<Guerrier> context) {
			Guerrier g = context.getPersonnage();
			float h = Input.GetAxis ("Horizontal");
			g.mouvementX ( h * g.getMaxSpeed(), g.getMaxSpeed());

			if(g.velocity.x != 0f){
				g.setDirection( g.velocity.x > 0f ? 1f : -1f );
				g.setSpriteRendererX(g.direction == -1f);
			}

			return true;
		};

		Func<Context<Guerrier>,bool> action_grimperAvancer = delegate(Context<Guerrier> context) {
			Guerrier g = context.getPersonnage();
			float h = Input.GetAxis ("Horizontal");
			if(h == 0f){
				return true;
			}
			g.mouvementX ( h * g.getMaxSpeed(), g.getMaxSpeed());

			if(g.velocity.x != 0f){
				g.setDirection( g.velocity.x > 0f ? 1f : -1f );
				g.setSpriteRendererX(g.direction == -1f);
			}

			return true;
		};


		etatGrimper.addCondition (delegate(Context<Guerrier> context) {
			return !context.getPersonnage().touche(Tag.HAUT_MUR);
		}, action_grimperAvancer, true);

		Func<Context<Guerrier>,bool> action_grimper = delegate(Context<Guerrier> context) {
			Guerrier g = context.getPersonnage();
			g.saute(Input.GetAxis ("Vertical") * g.getMaxSpeed());
			return true;
		};

		etatGrimper.addCondition (delegate(Context<Guerrier> context) {
			return true;
		}, action_grimper, true);





		/*
		EtatFwk<Guerrier> etatGrimpeHaut = new EtatFwk<Guerrier> ("grimpeHaut", this.context);

		etatGrimper.addCondition ( etatGrimpeHaut,  delegate(Context<Guerrier> context) {
			return context.getPersonnage().touche(Tag.HAUT_MUR);
		});





		Func<Context<Guerrier>,bool> action_grimpeHaut = delegate(Context<Guerrier> context) {
			Guerrier g = context.getPersonnage();
			g.saute(Input.GetAxis ("Vertical") * g.getMaxSpeed());
			return true;
		};
	*/









		etatIdle.addCondition (cond_avancer, action_avancer, true);

		etatCourir.addCondition (cond_avancer, action_avancer, true);

		Func<Context<Guerrier>,bool> action_vitesse_0 = delegate(Context<Guerrier> context) {
			context.getPersonnage().mouvementX(0,0);
			return true;
		};


		EtatFwk<Guerrier> etatAttaquer = new EtatFwk<Guerrier> ("attaquer", this.context);

		Func<Context<Guerrier>,bool> cond_idle_attaquer = delegate(Context<Guerrier> context) {
			return Input.GetKey(KeyCode.A);
		};

		etatIdle.addCondition (etatAttaquer, cond_idle_attaquer, action_vitesse_0);

		Func<Context<Guerrier>,bool> cond_attaquer_idle = delegate(Context<Guerrier> context) {
			return context.animationFini();
		};

		etatAttaquer.addCondition (etatIdle, cond_attaquer_idle);


		EtatFwk<Guerrier> etatDefendre = new EtatFwk<Guerrier> ("defendre", this.context);

		Func<Context<Guerrier>,bool> cond_idle_defendre = delegate(Context<Guerrier> context) {
			return Input.GetKey(KeyCode.D);
		};

		etatIdle.addCondition (etatDefendre, cond_idle_defendre, action_vitesse_0);

		Func<Context<Guerrier>,bool> cond_defendre_idle = delegate(Context<Guerrier> context) {
			return Input.GetKeyUp(KeyCode.D);
		};

		etatDefendre.addCondition (etatIdle, cond_defendre_idle);



		EtatFwk<Guerrier> etatToucher = new EtatFwk<Guerrier> ("toucher", this.context);

		Func<Context<Guerrier>,bool> cond_idle_toucher = delegate(Context<Guerrier> context) {
			return context.getTouchers().getMonoBehaviour<Enemie>() != null;
		};

		etatIdle.addCondition (etatToucher, cond_idle_toucher, action_vitesse_0);

		Func<Context<Guerrier>,bool> cond_toucher_idle = delegate(Context<Guerrier> context) {
			return context.animationFini();
		};

		etatToucher.addCondition (etatIdle, cond_toucher_idle);


		EtatFwk<Guerrier> etatTomber = new EtatFwk<Guerrier> ("tomber", this.context);

		Func<Context<Guerrier>,bool> cond_idle_tomber = delegate(Context<Guerrier> context) {
			return context.getPersonnage().velocity.y < 0;
		};

		etatIdle.addCondition (etatTomber, cond_idle_tomber);

		Func<Context<Guerrier>,bool> cond_tomber_idle = delegate(Context<Guerrier> context) {
			return context.getPersonnage().toucheSol();
		};

		etatTomber.addCondition (etatIdle, cond_tomber_idle);



		EtatFwk<Guerrier> etatSauter = new EtatFwk<Guerrier> ("sauter", this.context);

		Func<Context<Guerrier>,bool> cond_idle_sauter = delegate(Context<Guerrier> context) {
			return context.getPersonnage().velocity.y > 0;
		};

		etatIdle.addCondition (etatSauter, cond_idle_sauter);

		Func<Context<Guerrier>,bool> cond_sauter_idle = delegate(Context<Guerrier> context) {
			return context.getPersonnage().toucheSol();
		};

		etatSauter.addCondition (etatIdle, cond_sauter_idle);

		Func<Context<Guerrier>,bool> cond_sauter_tomber = delegate(Context<Guerrier> context) {
			return context.getPersonnage().velocity.y < 0;
		};

		etatSauter.addCondition (etatTomber, cond_sauter_tomber);


		Func<Context<Guerrier>,bool> cond_sauter = delegate(Context<Guerrier> context) {
			return context.getPersonnage().toucheSol() && Input.GetButtonDown ("Jump");
		};

		Func<Context<Guerrier>,bool> action_sauter = delegate(Context<Guerrier> context) {
			context.getPersonnage().saute(context.getPersonnage().getMaxSaut());
			return true;
		};

		etatIdle.addCondition (cond_sauter, action_sauter, true);



		Func<Context<Guerrier>,bool> cond_ralentirSauter = delegate(Context<Guerrier> context) {
			return Input.GetButtonUp ("Jump");
		};

		Func<Context<Guerrier>,bool> action_ralentirSauter = delegate(Context<Guerrier> context) {
			context.getPersonnage().yPourcentage(0.1f);
			return true;
		};

		etatSauter.addCondition (cond_ralentirSauter, action_ralentirSauter, true);


		etatCourir.addCondition (etatSauter, cond_idle_sauter);
		etatCourir.addCondition (cond_sauter, action_sauter, true);
		etatCourir.addCondition (etatTomber, cond_idle_tomber);

		etatCourir.addCondition (etatToucher, cond_idle_toucher, action_vitesse_0);
		etatSauter.addCondition (etatToucher, cond_idle_toucher, action_vitesse_0);
		etatTomber.addCondition (etatToucher, cond_idle_toucher, action_vitesse_0);
		etatAttaquer.addCondition (etatToucher, cond_idle_toucher, action_vitesse_0);


		etatSauter.addCondition (etatGrimper, cond_courir_grimper, action_courir_grimper);

		etatTomber.addCondition (etatGrimper, cond_courir_grimper, action_courir_grimper);
	}

	public void setSpriteRendererX(bool b){
		this.spriteRenderer.flipX = b;
	}

	public float getMaxSpeed(){
		return this.maxSpeed;
	}

	public float getDirection(){
		return this.direction;
	}

	public void setDirection(float direction){
		this.direction = direction;
	}

	public float getMaxSaut(){
		return this.maxSaut;
	}
}
