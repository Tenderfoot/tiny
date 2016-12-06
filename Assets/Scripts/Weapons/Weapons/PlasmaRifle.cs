using UnityEngine;
using System.Collections;
using System;

public class PlasmaRifle : ProjectileWeapon
{
	public PlasmaRifle()
	{
		chargeTime = 0;
		reloadDuration = 5;
		fireRate = 0.1f;
		magSize = 20;
		fireMode = FireMode.auto;
		roundsInMag = magSize;
	}

	public override void Fire()
	{
		CreateProjectile();
	}
}
