using UnityEngine;
using System.Collections;
using System;

public class ChargeBlaster : ProjectileWeapon
{
	public ChargeBlaster()
	{
		chargeTime = 0;
		reloadDuration = 2;
		fireRate = 0f;
		magSize = 1;
		fireMode = FireMode.charge;
		roundsInMag = magSize;
	}

	public override void Fire()
	{
		var newProjectile = CreateProjectile();

		if (chargeTime < 0.5)
			chargeTime = 0.5f;

		newProjectile.transform.localScale = new Vector2(chargeTime, chargeTime);

		ReloadWeapon();
	}
}
