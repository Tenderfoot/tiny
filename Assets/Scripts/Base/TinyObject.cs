using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class TinyObject : MonoBehaviour {

	// Everything should inherit from TinyObject
	// We can keep utility functions and useful things here
	private List<Powerup> powerups = new List<Powerup>();

	public virtual void HandleHit(TinyObject tiny)
	{
		Destroy(tiny.gameObject);
		Destroy(this.gameObject);
	}

	public void AddPowerup(Powerup powerup)
	{
		powerup.OnActivated(this);
		powerups.Add(powerup);
	}

	public void RemovePowerup(Powerup powerup)
	{
		powerup.OnDeactivated(this);
		powerups.Remove(powerup);
	}

}
