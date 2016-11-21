using UnityEngine;
using System.Collections;
using System;

public class ChargeBlaster : ProjectileWeapon
{
	void Start()
	{
		roundsInMag = magSize;
	}

	public override void Fire()
	{
		roundsInMag--;

		GameObject newProjectile = (GameObject)Instantiate(projectile, new Vector2(transform.parent.transform.position.x, transform.parent.transform.position.y), Quaternion.identity);
		newProjectile.GetComponent<Projectile>().direction = (transform.parent.transform.localScale.x < 0) ? Vector2.left : Vector2.right;

		if (chargeTime < 0.5)
			chargeTime = 0.5f;

		newProjectile.transform.localScale = new Vector2(chargeTime, chargeTime);

		ReloadWeapon();
	}
}
