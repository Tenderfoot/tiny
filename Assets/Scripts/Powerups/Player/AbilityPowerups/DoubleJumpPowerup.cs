using UnityEngine;
using System.Collections;
using System;

public class DoubleJumpPowerup : Powerup {

	public DoubleJumpPowerup()
	{
		this.name = "double jump";
		this.id = "double_jump";
	}

	public override void OnActivated(TinyObject tiny)
	{
		Debug.Log("hi");
		var player = (Player)tiny;
		player.extraJumps += 1; 
	}

	public override void OnDeactivated(TinyObject tiny)
	{
		var player = (Player)tiny;
		player.extraJumps -= 1;
	}

}
