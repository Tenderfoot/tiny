using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class TinyObject : MonoBehaviour {

	// Everything should inherit from TinyObject
	// We can keep utility functions and useful things here
	private List<Powerup> powerups = new List<Powerup>();

	public virtual void HandleHit(TinyObject tiny)
	{
		// Default behaviour is to do nothing.
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

	// Flipping and Rotation
	public void FlipX()
	{
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void FlipY()
	{
		Vector3 theScale = transform.localScale;
		theScale.y *= -1;
		transform.localScale = theScale;
	}

}
