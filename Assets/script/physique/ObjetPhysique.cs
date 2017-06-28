using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetPhysique : MonoBehaviour {

	protected Vector2 groundNormal;
	protected Rigidbody2D rb2d;
	public Vector2 velocity;
	protected ContactFilter2D contactFilter;
	protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];

	public static int ETAT_SOL = 0;
	public static int ETAT_TOMBE = 1;
	public static int ETAT_SAUT= 2;

	public int currentState = ETAT_TOMBE; 

	public Vector2 targetVelocity;

	private bool graviteActive = true;

	void OnEnable()
	{
		rb2d = GetComponent<Rigidbody2D> ();
	}

	// Use this for initialization
	void Start () {
		contactFilter.useTriggers = false;
		contactFilter.SetLayerMask (Physics2D.GetLayerCollisionMask (gameObject.layer));
		contactFilter.useLayerMask = true;
		this.start ();
	}

	protected virtual void start()
	{

	}
	
	void Update () 
	{
		targetVelocity = velocity;
		ComputeVelocity (); 
	}

	protected virtual void ComputeVelocity()
	{

	}

	protected virtual void toucheInterne(ToucherInterne t)
	{

	}

	private void appliqueGravite(){
		velocity += ParametrePhysique.gravityModifier * Physics2D.gravity * Time.deltaTime;
	}


	public void mouvementX(float x, float maxVitesseX){
		if(x == 0){
			targetVelocity.x = 0;
			return;
		}

		targetVelocity.x += x;

		int direction = -1;
		if (x > 0) {
			direction = 1;
		}

		targetVelocity.x = Mathf.Abs(targetVelocity.x) > maxVitesseX ? maxVitesseX * direction : targetVelocity.x;
	}

	public void saute(float y){
		targetVelocity.y = y;
	}

	public void yPourcentage(float coef){
		targetVelocity.y *= coef;
	}

	public void frottementX(float coef){
		targetVelocity.x *= coef;
	}

	public void enableGravite(bool cond){
		this.graviteActive = cond;
	}

	void FixedUpdate()
	{
		velocity = targetVelocity;

		if(graviteActive){
			appliqueGravite ();
		}

		Vector2 deltaPosition = velocity * Time.deltaTime;

		MovementY (Vector2.up * deltaPosition.y);

		Vector2 moveAlongGround = new Vector2 (groundNormal.y, -groundNormal.x);

		MovementX (moveAlongGround * deltaPosition.x);

		AligneAvecSol ();
	}

	public bool toucheSol(){
		return currentState == ETAT_SOL;
	}


	void MovementX(Vector2 move){
		float distance = move.magnitude;

		int count = 0;

		if (distance > ParametrePhysique.minMoveDistance) {
			count = rb2d.Cast (move, contactFilter, hitBuffer, distance + ParametrePhysique.shellRadius);


			for (int i = 0; i < count; i++) {

				toucheInterne(new ToucherInterne(hitBuffer[i],velocity));

				if(hitBuffer[i].transform.tag == Tag.NON_BLOQUANT){
					continue;
				}

				velocity.x = 0;

				if(velocity.y > 0){
					velocity.y = 0;
				}

				float modifiedDistance = hitBuffer [i].distance - ParametrePhysique.shellRadius;
				distance = modifiedDistance < distance ? modifiedDistance : distance;
			}
		}



		rb2d.position = rb2d.position + move.normalized * distance;
	}


	void AligneAvecSol(){
		if(!toucheSol()){
			return;
		}
		int count = rb2d.Cast (Vector2.down, contactFilter, hitBuffer, ParametrePhysique.distanceAligneAvecSol);

		for (int i = 0; i < count; i++) {

			if (hitBuffer [i].transform.tag == Tag.SOL) {
				float distance = hitBuffer [i].distance - ParametrePhysique.shellRadius;
				rb2d.position = rb2d.position + Vector2.down * distance;
				currentState = ETAT_SOL;
				groundNormal = hitBuffer [i].normal;
				return;
			}

		}

	}

	void MovementY(Vector2 move)
	{
		float distance = move.magnitude;

		if(toucheSol()){
			currentState = ETAT_TOMBE; 
		}

		if (distance > ParametrePhysique.minMoveDistance) 
		{
			int count = rb2d.Cast (move, contactFilter, hitBuffer, distance + ParametrePhysique.shellRadius);

			for (int i = 0; i < count; i++) {

				if(hitBuffer[i].transform.tag == Tag.NON_BLOQUANT){
					continue;
				}


				if(hitBuffer [i].transform.tag == Tag.SOL && move.y < 0){

					groundNormal = hitBuffer [i].normal;
					currentState = ETAT_SOL;
					/*
					if (groundNormal.y > ParametrePhysique.minGroundNormalY) {
						currentState = ETAT_SOL;
					} else {
						print ("groundNormal.y <= minGroundNormalY");
						//currentState = ETAT_GLISSE;
					}
					*/
				}

				if (toucheSol()) {
					velocity.y = 0;
				}

				if(!toucheSol() && move.y > 0){
					velocity.y = 0;
				}

				float modifiedDistance = hitBuffer [i].distance - ParametrePhysique.shellRadius;
				distance = modifiedDistance < distance ? modifiedDistance : distance;	
			}
		}

		if (!toucheSol ()) {
			if (move.y > 0) {
				currentState = ETAT_SAUT;
			} else {
				currentState = ETAT_TOMBE;
			}
		}

		if (currentState == ETAT_SAUT) {
			groundNormal = Vector2.up;
		}

		rb2d.position = rb2d.position + move.normalized * distance;
	}


	public bool touche(string tag){
		int count = rb2d.Cast (Vector2.zero, contactFilter, hitBuffer, 0);

		for (int i = 0; i < count; i++) {

			if (hitBuffer [i].transform.tag == tag) {
				return true;
			}

		}
		return false;
	}

	public RaycastHit2D toucheReturn(string tag){
		int count = rb2d.Cast (Vector2.zero, contactFilter, hitBuffer, 0);

		for (int i = 0; i < count; i++) {

			if (hitBuffer [i].transform.tag == tag) {
				return hitBuffer [i];
			}

		}

		return new RaycastHit2D ();
	}
}
