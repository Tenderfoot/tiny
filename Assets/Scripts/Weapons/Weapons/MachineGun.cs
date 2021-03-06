﻿using UnityEngine;
using System.Collections;
using System;

public class MachineGun : ProjectileWeapon
{
	public MachineGun()
	{
		chargeTime = 0;
		reloadDuration = 0;
		fireRate = 0.1f;
		magSize = 1;
		fireMode = FireMode.auto;
		roundsInMag = magSize;
	}

	public override void Fire()
	{
		CreateProjectile();
	}
}