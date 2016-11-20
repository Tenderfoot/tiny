using UnityEngine;
using System.Collections;

public abstract class TinyWeapon : TinyObject
{
	// Different Fire modes
	// Semiauto you press the trigger each time you want to fire
	// Auto you hold the trigger down to fire continuously
	// Charge you hold the trigger down then release
	public enum FireMode { semiauto, auto, charge}

	// changable in the editor...
	public float chargeTime;
	public float reloadDuration;
	public float fireRate;
	public int magSize;
	public FireMode fireMode;
	public Sprite weaponImage;
	public int roundsInMag;

	// Control stuff
	private bool isPressed;
	private float timeSinceFired;
	public bool reloading;
	public float reloadTime;

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

	void Start()
	{
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

	void ToggleWeapon(bool alive)
	{
		foreach (var collider in this.gameObject.GetComponents<Collider2D>())
		{
			collider.enabled = alive;
		}
		((SpriteRenderer)this.gameObject.GetComponent<SpriteRenderer>()).enabled = alive;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Player player = other.GetComponentInParent<Player>();
		if(player != null)
		{
			if(other.transform.childCount > 0)
				Destroy(other.transform.GetChild(0).gameObject);

			player.equippedWeapon = this.gameObject;
			player.equippedWeapon.transform.parent = player.transform;
			ToggleWeapon(false);
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
		if (roundsInMag > 0 || fireMode == FireMode.charge)
			Fire();
		else
			ReloadWeapon();
	}

	public abstract void Fire();

}
