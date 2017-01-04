using UnityEngine;
using System.Collections;

public class Projectile : Damager {

	public Vector2 direction;
	public float projectileSpeed;
	public TinyObject owner;
	private float lifeTime;

	// Use this for initialization
	void Start () {
		lifeTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<Rigidbody2D>().velocity = direction.normalized*projectileSpeed;
		lifeTime += Time.deltaTime;

		if (lifeTime > 5)
			Destroy(this.gameObject);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		TinyObject target = other.gameObject.GetComponent<TinyObject>();

		if (target != null && target != owner)
		{
			target.HandleHit(this);
			GameObject.Destroy(gameObject);
		}
	}
}
