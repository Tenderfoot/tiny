using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Leveller {

	private int level = 0;
	private int experience = 0;
	private TinyObject host;

	private Dictionary<int, int> levelMap = new Dictionary<int, int>()
	{
		{ 1, 0 },
		{ 2, 300 },
		{ 3, 900 },
		{ 4, 1500 },
		{ 5, 2500 }
	};

	private Dictionary<int, Powerup> powerupMap;

	// The object that this leveller will make changes to
	public GameObject gameObject { get; set; }

	public Leveller(TinyObject host, Dictionary<int, Powerup> powerups)
	{
		powerupMap = powerups;
		this.host = host;

		level = 0;
		LevelUp();
	}

	private void LevelUp()
	{
		level += 1;
		levelMap.TryGetValue(level, out experience);

		Powerup val = null; 
		powerupMap.TryGetValue(level, out val);

		if (val != null)
		{
			host.AddPowerup(val);
		}

		Debug.Log("Level Up");

	}

	public void AddExperience(int exp)
	{
		experience += exp;

		int levelExp;
		var levelExists = levelMap.TryGetValue(level, out levelExp);

		int difference = experience - levelExp;

		if (experience > levelExp && levelExists)
		{
			LevelUp();
			experience += difference;
		}
	}

}
