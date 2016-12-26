using UnityEngine;
using System.Collections;

public class Ball : TinyObject {

	private float forceConstant = 5000f;
	private Rigidbody2D body;

	// Use this for initialization
	void Start () {
		body = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void HandleHit(TinyObject tiny)
	{
		var damager = (Damager)tiny;
		var projectileBody = tiny.gameObject.GetComponent<Rigidbody2D>();

		body.AddForce(projectileBody.velocity.normalized * forceConstant * damager.damage);

		GameObject.Destroy(tiny.gameObject);
	}
}
