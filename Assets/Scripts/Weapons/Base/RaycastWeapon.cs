using UnityEngine;
using System.Collections;
using System;

public abstract class RaycastWeapon : TinyWeapon
{
	public GameObject rayEffect;

	void Start()
	{
		roundsInMag = magSize;
	}

	public override void Fire()
	{
		roundsInMag--;
		RaycastHit2D hit;
		Vector2 direction;

		if (transform.parent.gameObject.transform.localScale.x > 0)
			direction = Vector2.right;
		else
			direction = Vector2.left;

		hit = Physics2D.Raycast(new Vector3(transform.parent.transform.position.x + direction.x, transform.parent.transform.position.y, 0), direction);

		if (hit.collider != null)
		{
			HitObject(hit.collider.gameObject);
			SpawnRay(hit, direction);
		}
	}

	void SpawnRay(RaycastHit2D hit, Vector2 dir)
	{
		GameObject rayEffectObj = (GameObject)Instantiate(rayEffect, new Vector2(transform.parent.transform.position.x+((hit.distance/2)*dir.x), transform.parent.transform.position.y), Quaternion.identity);
		rayEffectObj.transform.localScale = new Vector3(hit.distance*dir.x, 0.25f, 1);
	}

	public abstract void HitObject(GameObject hit);
}
