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

	private List<Powerup> powerupList = new List<Powerup>()
	{
		new SpeedPowerup(),
		new PlasmaRiflePowerup(),
		new SniperPowerup(),
		new ChargeBlasterPowerup()
	};

	// Use this for initialization
	private PowerupManager () {
		powerups = new Dictionary<string, Powerup>();

		foreach (Powerup p in powerupList) {
			powerups.Add(p.id, p);
		}

	}

	public static List<string> GetIds()
	{
		return instance.powerups.Keys.ToList();
	}

	public static Powerup FindById(string id)
	{
		return instance.powerups[id];
	}

}
