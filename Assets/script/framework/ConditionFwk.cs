using System;

public class ConditionFwk<Personnage> {

	private Context<Personnage> context;

	private EtatFwk<Personnage> suivant = null;

	private Func<Context<Personnage>,bool> condition = null;

	private Func<Context<Personnage>,bool> action = null;

	//Si la condition est valide => on reste ou pas dans l'etat
	private bool rester = false;


	public ConditionFwk(Context<Personnage> context, EtatFwk<Personnage> suivant, Func<Context<Personnage>,bool> condition){
		this.context = context;
		this.suivant = suivant;
		this.condition = condition;
	}

	public ConditionFwk(Context<Personnage> context, EtatFwk<Personnage> suivant, Func<Context<Personnage>,bool> condition, Func<Context<Personnage>,bool> action
	) : this(context, suivant, condition){
		this.action = action;
	}

	public ConditionFwk(Context<Personnage> context, Func<Context<Personnage>,bool> condition, Func<Context<Personnage>,bool> action,
		bool rester) : this(context, null, condition, action){
		this.rester = rester;
	}

	public bool appliqueCondition(){
		return condition (context);
	}

	public void appliqueAction(){
		if(action != null){
			action (context);
		}
	}

	public EtatFwk<Personnage> getSuivant(){
		return suivant;
	}

	public bool getRester(){
		return this.rester;
	}

}
