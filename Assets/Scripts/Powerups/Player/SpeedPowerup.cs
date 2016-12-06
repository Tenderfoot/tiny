using UnityEngine;
using System.Collections;
using System;

public class SpeedPowerup : Powerup {

	public SpeedPowerup()
	{
		this.name = "Speed";
		this.id = "speed";
	}

	public override void OnActivated(TinyObject tiny)
	{
		var player = (Player)tiny;
		player.force *= 2f; 
	}

	public override void OnDeactivated(TinyObject tiny)
	{
		var player = (Player)tiny;
		player.force /= 2f;
	}

}
