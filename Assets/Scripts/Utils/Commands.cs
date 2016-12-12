using UnityEngine;
using System.Collections;
using DevConsole;
using System.Collections.Generic;
using System.Reflection;
using InControl;

public class Commands : MonoBehaviour {

	protected static LevelManager levelManager;

	private Dictionary<string, string> commands = new Dictionary<string, string> {
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

	static void add_exp(string exp)
	{
		Tiny.Player1.AddExperience(int.Parse(exp));
	}

}
