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

	public void AddPowerup(string powerup_id)
	{
		var powerup = PowerupManager.FindById(powerup_id);
		powerup.OnActivated(gameObject);
		powerups.Add(powerup);
	}

	public void RemovePowerup(string powerup_id)
	{
		var powerup = PowerupManager.FindById(powerup_id);
		powerup.OnDeactivated(gameObject);
		powerups.Remove(powerup);
	}
}
