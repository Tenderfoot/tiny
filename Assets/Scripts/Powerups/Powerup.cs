using UnityEngine;
using System.Collections;

public abstract class Powerup {

	public string name; // The display name of the powerup
	public string id; // The identifier key of the powerup 

	public abstract void OnActivated(TinyObject tiny);
	public abstract void OnDeactivated(TinyObject tiny);

}
