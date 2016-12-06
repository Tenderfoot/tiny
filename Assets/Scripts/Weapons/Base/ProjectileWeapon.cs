using UnityEngine;
using System.Collections;

public abstract class ProjectileWeapon : TinyWeapon
{
	public GameObject projectile;

	public GameObject CreateProjectile()
	{
		roundsInMag--;

		GameObject newProjectile = (GameObject)GameObject.Instantiate(projectile, new Vector2(holder.transform.position.x, holder.transform.position.y), Quaternion.identity);
		newProjectile.GetComponent<Projectile>().direction = (holder.transform.localScale.x < 0) ? Vector2.left : Vector2.right;

		return newProjectile;
	}
}
