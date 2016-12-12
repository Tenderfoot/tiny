using UnityEngine;
using System.Collections;

public abstract class TinyWeapon : TinyObject
{
	// Different Fire modes
	// Semiauto you press the trigger each time you want to fire
	// Auto you hold the trigger down to fire continuously
	// Charge you hold the trigger down then release
	public enum FireMode { semiauto, auto, charge}

	protected float chargeTime;
	protected float reloadDuration;
	protected float fireRate;
	protected int magSize;
	protected FireMode fireMode;
	protected Sprite weaponImage;
	protected int roundsInMag;

	// Control stuff
	private bool isPressed;
	private float timeSinceFired;
	protected bool reloading;
	protected float reloadTime;

	public TinyObject holder;

	public float GetReloadDuration() { return reloadDuration; }
	public int GetMagSize() { return magSize; }
	public int GetRoundsInMag() { return roundsInMag; }
	public bool IsReloading() { return reloading; }
	public float GetReloadTime() { return reloadTime; }

	public void OnTriggerPressed()
	{
		if (!reloading)
		{
			switch (fireMode)
			{
				case FireMode.semiauto:
					if (isPressed == false)
						TryToFire();
					break;
				case FireMode.auto:
					timeSinceFired += Time.deltaTime;
					if (timeSinceFired > fireRate)
					{
						timeSinceFired = 0;
						TryToFire();
					}
					break;
				case FireMode.charge:
					chargeTime += Time.deltaTime;
					break;
			}
			isPressed = true;
		}
	}

	void Update()
	{ 
		if (reloading)
			reloadTime += Time.deltaTime;

		if(reloadTime>reloadDuration)
		{
			reloading = false;
			reloadTime = 0;
			roundsInMag = magSize;
		}
	}

	public void OnTriggerReleased()
	{
		if (!reloading)
		{
			if (fireMode == FireMode.charge && isPressed)
			{
				TryToFire();
			}

			timeSinceFired = 0;
			isPressed = false;
		}
	}

	public void ReloadWeapon()
	{
		reloading = true;
		reloadTime = 0;
		chargeTime = 0;
	}

	void TryToFire()
	{
		if (roundsInMag > 0 || fireMode == FireMode.charge || reloadDuration == 0)
			Fire();
		else
			ReloadWeapon();
	}

	public abstract void Fire();

}
