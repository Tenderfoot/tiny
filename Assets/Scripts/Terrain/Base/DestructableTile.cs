using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestructableTile : Tile {

	private int damageTaken = 0;

	// override this for subclasses with different animations
	protected Animator animator;
	protected AnimatorStates states;

	// int: <damageTaken>, string: <animatorState>
	protected Dictionary<int, string> destructStates = new Dictionary<int, string>()
	{
		{60, "destroy"},
		{40, "crumbling"},
		{20, "tarnished"},
		{0, "clean" }
	};

	void Start()
	{
		animator = transform.GetComponent<Animator>();

		string[] vals = new string[destructStates.Count];
		destructStates.Values.CopyTo(vals, 0);

		states = new AnimatorStates(animator, vals);
	}

	protected void TakeDamage(int damage)
	{
		damageTaken += damage;

		foreach (KeyValuePair<int, string> pair in destructStates)
		{
			if (damageTaken >= pair.Key)
			{
				if (pair.Value == "destroy")
				{
					GameObject.Destroy(this.gameObject);
					return;
				}

				states.ChangeState(pair.Value);
				break;
			}
		}
	}

	public override void HandleHit(TinyObject tiny)
	{
		var damager = tiny as Damager;

		if (damager != null)
		{
			TakeDamage(damager.damage);
		}

		GameObject.Destroy(tiny.gameObject);
	}

}
