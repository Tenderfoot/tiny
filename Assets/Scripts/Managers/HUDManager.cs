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

	// Use this for initialization
	void Start () {
        teamManager = GameObject.Find("TeamManager").GetComponent<TeamManager>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		canvas = GameObject.Find("Canvas");
	}
	
	public void AddStatusForPlayer(Player p)
	{
		GameObject newGameObject= ((GameObject)Instantiate(statusPrefab, new Vector3(0,0,0), Quaternion.identity));
		GameObject refPosition = GameObject.Find("Ref");

		// parent it and scale it properly..
		int numOfStatus = GameObject.FindGameObjectsWithTag("PlayerStatus").GetLength(0);
		newGameObject.transform.SetParent(canvas.transform);
		newGameObject.transform.localScale = new Vector3(1, 1, 1);
		newGameObject.transform.position = new Vector3(refPosition.transform.position.x, refPosition.transform.position.y-(10*numOfStatus), 0);

		// Hook it up with the player
		PlayerStatus newStatus = newGameObject.GetComponent<PlayerStatus>();
		newStatus.player = p;
	}

	// Update is called once per frame
	void Update () {
        TeamScore0.text = "Score: " + teamManager.GetScore(0);
        TeamScore1.text = "Score: " + teamManager.GetScore(1);
        ballRespawnText.enabled = (levelManager.GetBallRespawnTime() > 0);
        ballRespawnText.text = ""+(int)levelManager.GetBallRespawnTime();
    }
}
