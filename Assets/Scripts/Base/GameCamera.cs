using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {

	public enum cameraType { staticCam, followCam }

	private GameObject followObject;
	protected Vector3 startPosition;
	public cameraType type;

	// Use this for initialization
	void Start() {
		startPosition = transform.position;
	}

	// Update is called once per frame
	void Update () {
		if (type == cameraType.followCam)
		{
			if (followObject == null && GameObject.FindGameObjectWithTag("Player") != null)
				followObject = GameObject.FindGameObjectWithTag("Player");
			else if(followObject != null)
				transform.position = new Vector3(followObject.transform.position.x, followObject.transform.position.y, startPosition.z);
		}
	}

	public void SetFollowObject(GameObject objToFollow)
	{
		followObject = objToFollow;
	}
}
