using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour {

	public Image health, background, emptyRef, deadOverlay;
	public Text Timer;
	public Player player;

	// Use this for initialization
	void Start () {
		Timer.enabled = false;
	}

	void UpdateHealth()
	{
		RectTransform startSize = background.rectTransform;
		RectTransform endSize = emptyRef.rectTransform;

		health.rectTransform.sizeDelta = Vector2.Lerp(endSize.sizeDelta, startSize.sizeDelta, player.HealthAsPercent());
		health.rectTransform.position = Vector3.Lerp(endSize.position, startSize.position, player.HealthAsPercent());

		deadOverlay.enabled = (player.IsDead() == true);
		Timer.enabled = (player.IsDead() == true);

		Timer.text = "" + player.GetSpawnTimeLeft();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(player != null)
		{
			UpdateHealth();
		}
	}
}
