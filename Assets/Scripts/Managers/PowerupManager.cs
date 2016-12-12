using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PowerupManager {

	private static PowerupManager _instance;
	private static PowerupManager instance
	{
		get
		{
			if (_instance == null)
				_instance = new PowerupManager();

			return _instance;
		}
	}


	private Dictionary<string, Powerup> powerups;

	// Use this for initialization
	private PowerupManager () {
	}

	public static List<string> GetIds()
	{
		return instance.powerups.Keys.ToList();
	}


}
