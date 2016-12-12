using UnityEngine;
using System.Collections;
using InControl;

public class PlayerAssignment {

	public int playerNum { get; set; }
	public InputDevice device { get; set; }
	public GameObject playerObject { get; set; }
	public PlayerActions actions { get; set; }

	public void Bind(Player player)
	{
		playerObject = player.gameObject;
		player.actions = actions;
		player.assignment = this;
	}

	public override string ToString()
	{
		return string.Format("Player {0}", playerNum);
	}

}
