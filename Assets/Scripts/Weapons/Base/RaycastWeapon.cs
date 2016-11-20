using UnityEngine;
using System.Collections;
using System;

public abstract class RaycastWeapon : TinyWeapon
{
	void Start()
	{
		roundsInMag = magSize;
	}

	public override void Fire()
	{
		roundsInMag--;
		RaycastHit2D hit;

		if (transform.parent.gameObject.transform.localScale.x > 0)
			hit = Physics2D.Raycast(new Vector3(transform.parent.transform.position.x + 1, transform.parent.transform.position.y, 0), Vector2.right);
		else
			hit = Physics2D.Raycast(new Vector3(transform.parent.transform.position.x - 1, transform.parent.transform.position.y, 0), Vector2.left);

		if (hit.collider != null)
		{
			HitObject(hit.collider.gameObject);
		}
	}

	public abstract void HitObject(GameObject hit);
}
