using UnityEngine;
using System.Collections;

public abstract class WeaponPowerup : Powerup {

	protected string weaponPath;
	protected GameObject weapon;

	public override void OnActivated(TinyObject tiny)
	{
		// Make sure to remove old one if it exists
		OnDeactivated(tiny);

		var player = (Player) tiny;
		weapon = NewWeapon();
		player.weapon1 = weapon;
		weapon.GetComponent<TinyWeapon>().holder = player;
	}

	public override void OnDeactivated(TinyObject tiny)
	{
		var player = (Player)tiny;

		if (weapon != null)
			GameObject.Destroy(weapon);

		player.weapon1 = weapon = null;
	}

	private GameObject NewWeapon()
	{
		Debug.Log(weaponPath);
		return GameObject.Instantiate((GameObject)Resources.Load(weaponPath));
	}

}

