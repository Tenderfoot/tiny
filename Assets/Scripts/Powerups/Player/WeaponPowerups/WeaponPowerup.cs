using UnityEngine;
using System.Collections;

public abstract class WeaponPowerup : Powerup {

	protected string weaponPath;

	public override void OnActivated(TinyObject tiny)
	{
		// Make sure to remove old one if it exists
		OnDeactivated(tiny);

		var player = (Player) tiny;
		player.weapon1 = NewWeapon();
		player.weapon1.GetComponent<TinyWeapon>().holder = player;
	}

	public override void OnDeactivated(TinyObject tiny)
	{
		var player = (Player)tiny;

		if (player.weapon1 != null)
			GameObject.Destroy(player.weapon1);

		player.weapon1 = null;
	}

	private GameObject NewWeapon()
	{
		return GameObject.Instantiate((GameObject)Resources.Load(weaponPath));
	}

}

