using UnityEngine;
using System.Collections;

public class Managers {
	/*
		This class is a helper class to get instances of manager objects
	*/

	private static Managers _instance;
	private static Managers instance
	{
		get
		{
			if (_instance == null)
				_instance = new Managers();

			return _instance;
		}
	}

	private Managers()
	{

	}

	public static PlayerAssignmentManager PlayerAssignmentManager
	{
		get { return GameObject.Find("PlayerAssignmentManager").GetComponent<PlayerAssignmentManager>(); }
	}
}
