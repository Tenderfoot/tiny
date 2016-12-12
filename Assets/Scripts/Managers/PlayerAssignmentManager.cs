using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using InControl;

public class TooManyPlayersException : Exception { };
public class MultipleBindingException : Exception { };

public class PlayerAssignmentManager : MonoBehaviour {

	private Dictionary<InputDevice, PlayerActions> knownDevices = new Dictionary<InputDevice, PlayerActions>();

	private int maxPlayers = 4;
	private int connectedPlayers = 0;
	private List<PlayerAssignment> assignments = new List<PlayerAssignment>();

	// Use this for initialization
	void Start () {
		foreach (var d in InputManager.Devices)
		{
			PlayerActions actions = PlayerActions.CreateWithDefaultBindings();
			actions.Device = d;
			knownDevices.Add(d, actions);
			Debug.Log(string.Format("Adding new device: {0}", d.Name));
		}
	}
	
	// Update is called once per frame
	void Update () {
		JoinNewPlayers();
	}

	public PlayerAssignment GetAssignment(int player_num)
	{
		return assignments[player_num - 1];
	}

	public PlayerAssignment GetAssignment(Player player)
	{
		return assignments.Find(p => p.playerObject.GetComponent<Player>() == player);
	}

	public bool HasUnboundAssignments()
	{
		return assignments.Count > 0;
	}

	// Gets player assignments that have no player object bound to them
	public List<PlayerAssignment> GetUnboundAssignments()
	{
		return assignments.FindAll(a => a.playerObject == null);
	}

	public bool canAddPlayer()
	{
		return connectedPlayers < maxPlayers;
	}

	private void AddPlayer(InputDevice device, PlayerActions actions)
	{
		if (connectedPlayers == maxPlayers)
			throw new TooManyPlayersException();

		// You can't have more than one player on the same device
		var existing = assignments.FindAll(a => a.device == device);
		if (existing.Count > 0)
			throw new MultipleBindingException();

		connectedPlayers++;

		var assignment = new PlayerAssignment()
		{
			playerNum = connectedPlayers,
			device = device,
			actions = actions
		};

		assignments.Add(assignment);
	}

	public void RemovePlayer(int playerNum)
	{
		if (connectedPlayers == 0)
			return;

		if (playerNum < 0)
			playerNum = connectedPlayers;

		var assignment = assignments.Find(a => a.playerNum == playerNum);
		GameObject.Destroy(assignment.playerObject);
		assignments.Remove(assignment);		
	}

	// Detect "Start" or "Enter" to add a new player to the map
	private void JoinNewPlayers()
	{
		foreach (KeyValuePair<InputDevice, PlayerActions> p in knownDevices)
		{
			var device = p.Key;
			var actions = p.Value;

			if (canAddPlayer() && actions.Join.WasPressed)
			{


				if (assignments.FindAll(d => d.device.GUID == device.GUID).Count == 0)
					AddPlayer(device, actions);
			}
		}
	}

	public override string ToString()
	{
		return string.Format("{0} players connected", connectedPlayers);
	}

}
