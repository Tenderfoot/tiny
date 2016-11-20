using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public Vector2 direction;
	public float projectileSpeed;
	private float lifeTime;
	// Use this for initialization
	void Start () {
		lifeTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<Rigidbody2D>().velocity = direction*projectileSpeed;
		lifeTime += Time.deltaTime;

		if (lifeTime > 5)
			Destroy(this.gameObject);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag != "Player")
		{
			Destroy(other.gameObject);
			Destroy(this.gameObject);
		}
	}
}
