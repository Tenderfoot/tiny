using UnityEngine;
using System.Collections;

public class RaycastEffect : MonoBehaviour {

	private float timeAlive;
	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		timeAlive = 0;
		spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		spriteRenderer.color = new Vector4(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a - (timeAlive / 10));

		if (timeAlive > 3)
			Destroy(this.gameObject);

		timeAlive += Time.deltaTime;
	}
}
