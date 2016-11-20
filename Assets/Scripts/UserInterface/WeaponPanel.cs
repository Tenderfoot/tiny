using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeaponPanel : MonoBehaviour {

	public Image weaponImage, reloadOverlay, reloadOverlayFull, reloadOverlayEmpty;
	public Text ammoRemaining;
	private TinyWeapon weapon;

	// Use this for initialization
	void Start () {
		weaponImage.enabled = false;
		ammoRemaining.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		GameObject player = GameObject.FindGameObjectWithTag("Player");

		if (player != null && player.GetComponent<Player>().equippedWeapon != null)
		{
			TinyWeapon test = player.GetComponent<Player>().equippedWeapon.GetComponent<TinyWeapon>();

			if (test != weapon)
				reloadOverlay.rectTransform.sizeDelta = reloadOverlayEmpty.rectTransform.sizeDelta;

			weapon = player.GetComponent<Player>().equippedWeapon.GetComponent<TinyWeapon>();

			weaponImage.enabled = true;
			ammoRemaining.enabled = true;
		}

		if(weapon != null)
		{
			ammoRemaining.text = weapon.roundsInMag + " / " + weapon.magSize;

			weaponImage.sprite = weapon.GetComponent<SpriteRenderer>().sprite;

			RectTransform startSize = reloadOverlayEmpty.rectTransform;
			RectTransform endSize = reloadOverlayFull.rectTransform;

			if (weapon.reloading)
			{
				float timeRemaining = weapon.reloadTime / weapon.reloadDuration;

				reloadOverlay.rectTransform.sizeDelta = Vector2.Lerp(endSize.sizeDelta, startSize.sizeDelta, timeRemaining);
				reloadOverlay.rectTransform.position = Vector3.Lerp(endSize.position, startSize.position, timeRemaining);
			}

		}

	}
}
