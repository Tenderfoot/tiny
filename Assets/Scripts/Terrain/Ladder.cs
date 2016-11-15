﻿using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.tag == "Player")
		{
			other.gameObject.GetComponent<Player>().ladder = transform;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.transform.tag == "Player")
		{
			other.gameObject.GetComponent<Player>().ladder = null;
		}
	}
}
