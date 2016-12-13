using UnityEngine;
using System.Collections;

public class Ball : TinyObject {

	private float forceConstant = 100000f;
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
		var obj = tiny.gameObject;

		var x = transform.position.x - obj.transform.position.x;
		var y = transform.position.y - obj.transform.position.y;

		var force = new Vector2(x, y);

		body.AddForce(force * forceConstant);
	}
}
