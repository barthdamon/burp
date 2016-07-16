using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public Text teamOneScoreText;
	public Text teamTwoScoreText;

	public GameObject ball;


	private int teamOneScore = 0;
	private int teamTwoScore = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GoalScored(int teamNumber) {
		Debug.Log ("GOAL SCORED, starting again");
		if (teamNumber == 2) {
			teamOneScore++;
			teamOneScoreText.text = "Yellow Team: " + teamOneScore;
			// dock points for team # 1
		} else {
			teamTwoScore++;
			teamTwoScoreText.text = "Blue Team: " + teamTwoScore;
			// dock points for team # 2
		}
		Vector3 spawnPoint = new Vector3 (0, 6, 0);
		Instantiate(ball, spawnPoint, Quaternion.identity);
	}
}
