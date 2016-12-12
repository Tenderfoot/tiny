using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

	// can abstract this if you think there will
	// ever be more than two teams
	public Text TeamScore0, TeamScore1;
	public Text ballRespawnText;
	public GameObject statusPrefab;
	private TeamManager teamManager;
	private LevelManager levelManager;
	private GameObject canvas;
	private new Camera camera;

	// Use this for initialization
	void Start () {
		teamManager = GameObject.Find("TeamManager").GetComponent<TeamManager>();
		levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		canvas = GameObject.Find("Canvas");
		camera = GameObject.Find("Camera").GetComponent<Camera>();
	}
	
	public void AddStatusForPlayer(PlayerAssignment a)
	{
		GameObject playerHUD = ((GameObject)Instantiate(statusPrefab, new Vector3(0,0,0), Quaternion.identity));

		Vector3 playersArea = camera.ViewportToWorldPoint(new Vector3(0.1f, 1f, 0));

		// parent it and scale it properly..
		playerHUD.transform.SetParent(canvas.transform);
		playerHUD.transform.localScale = new Vector3(1, 1, 1);
		playerHUD.transform.position = new Vector3(playersArea.x, playersArea.y - (10*a.playerNum), 0);

		// Hook it up with the player
		PlayerStatus newStatus = playerHUD.GetComponent<PlayerStatus>();
		newStatus.player = a.playerObject.GetComponent<Player>();
	}

	// Update is called once per frame
	void Update () {
		TeamScore0.text = "Score: " + teamManager.GetScore(0);
		TeamScore1.text = "Score: " + teamManager.GetScore(1);
		ballRespawnText.enabled = (levelManager.GetBallRespawnTime() > 0);
		ballRespawnText.text = ""+(int)levelManager.GetBallRespawnTime();
	}
}
