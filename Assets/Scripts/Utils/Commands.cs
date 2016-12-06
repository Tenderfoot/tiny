using UnityEngine;
using System.Collections;
using DevConsole;
using System.Collections.Generic;
using System.Reflection;
using InControl;

public class Commands : MonoBehaviour {

	protected static LevelManager levelManager;

	private Dictionary<string, string> commands = new Dictionary<string, string> {
		//{ "add_player", "|device| adds a player by device number (defaults to ActiveDevice)" },
		//{ "remove_player", "|player_num| removes player by number (defaults to most recent player)" },
		//{ "list_devices", "lists all available devices"},
		{ "list_powerups", "lists all available powerups" },
		{ "add_powerup", "adds specified powerup to first player" },
		{ "rm_powerup", "removes the specified powerup from the first player" },
		{ "add_exp", "adds experience to the first player" },
		{ "mute", "toggles audio"},
	};

	// Use this for initialization
	void Start () {

		levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

		foreach (KeyValuePair<string, string> p in commands)
		{
			MethodInfo method = this.GetType().GetMethod(p.Key, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

			DevConsole.Console.AddCommand(new Command<string>(
				p.Key, 
				(Command<string>.ConsoleMethod)System.Delegate.CreateDelegate(typeof(Command<string>.ConsoleMethod), method), 
				p.Value
			));
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	static void add_player(string device = "")
	{
		try
		{
			int i = int.TryParse(device, out i) ? i : -1;
		}
		catch (TooManyPlayersException)
		{
			Console.Log("maximum number of players allowed", Color.red);
		}	
		catch (MultipleBindingException)
		{
			Console.Log("That device is already bound to a player", Color.red);
		}
			
	}

	static void remove_player(string playerNum = "")
	{
		int i = int.TryParse(playerNum, out i) ? i : -1;
		levelManager.RemovePlayer(i);
	}

	static void list_devices(string arg = "")
	{
		for (var i = 0; i < InputManager.Devices.Count; i++)
		{
			var device = InputManager.Devices[i];
			Console.Log(string.Format("Device: {0}        {1}", i, device.Name));
		}
	}

	static void mute(string arg = "")
	{
		AudioListener.pause = !AudioListener.pause;
	}

	static void list_powerups(string arg = "")
	{
		foreach (var name in PowerupManager.GetIds())
		{
			Console.Log(name);
		}
	}

	static void add_powerup(string powerup_id)
	{
		Tiny.Player1.AddPowerup(powerup_id);
	}

	static void rm_powerup(string powerup_id)
	{
		Tiny.Player1.RemovePowerup(powerup_id);
	}

	static void add_exp(string exp)
	{
		Tiny.Player1.AddExperience(int.Parse(exp));
	}

}
