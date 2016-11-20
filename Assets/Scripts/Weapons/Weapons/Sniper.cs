using UnityEngine;
using System.Collections;
using System;

public class Sniper : RaycastWeapon
{
	public override void HitObject(GameObject hit)
	{
		Destroy(hit);
	}
}
