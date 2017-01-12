using UnityEngine;
using System.Collections;

public abstract class ProjectileWeapon : TinyWeapon
{
	public GameObject projectilePrefab;

	public GameObject CreateProjectile()
	{
		roundsInMag--;

		GameObject projectileObj = (GameObject)GameObject.Instantiate(projectilePrefab, new Vector2(holder.transform.position.x, holder.transform.position.y), Quaternion.identity);
		var projectile = projectileObj.GetComponent<Projectile>();

		projectile.direction = ((Player)holder).actions.Aim.Value;
		projectile.owner = (Player)holder;

		if (projectile.direction == Vector2.zero)
			projectile.direction = (holder.transform.localScale.x < 0) ? Vector2.left : Vector2.right;

		return projectileObj;
	}
}
