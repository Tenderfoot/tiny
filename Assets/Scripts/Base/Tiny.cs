using UnityEngine;
using System.Collections;

public class Tiny {

	private static Tiny _instance;
	private static Tiny instance
	{
		get
		{
			if (_instance == null)
				_instance = new Tiny();

			return _instance;
		}
	}

	public static Player Player1 {
		get
		{
			var assignments = GameObject.Find("PlayerAssignmentManager").GetComponent<PlayerAssignmentManager>();
			var assignment = assignments.GetAssignment(1);
			return assignment.playerObject.GetComponent<Player>();
		}
	}


}
