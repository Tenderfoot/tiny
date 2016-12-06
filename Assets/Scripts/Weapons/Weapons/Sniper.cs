using UnityEngine;
using System.Collections;
using System;

public class Sniper : RaycastWeapon
{
	public Sniper()
	{
		chargeTime = 0;
		reloadDuration = 2;
		fireRate = 0.075f;
		magSize = 1;
		fireMode = FireMode.semiauto;
		roundsInMag = 1;
}

	public override void HitObject(GameObject hit)
	{
		GameObject.Destroy(hit);
	}
}
