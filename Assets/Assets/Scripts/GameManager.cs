using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public Text teamOneScoreText;
	public Text teamTwoScoreText;

	public Text gameOverText;
	public Text winnerText;
	public GameObject playAgainButton;

	public Text gameTimerText;

	public GameObject ball;

	private bool gameActive = false;
	private float endTime;
	private int teamOneScore = 0;
	private int teamTwoScore = 0;
	// Use this for initialization
	void Start () {
		ResetGame ();
	}
	
	// Update is called once per frame
	void Update () {
		if (gameActive) {
			int timeLeft = (int)(endTime - Time.time);
			if (timeLeft < 0) {
				GameOver ();
			} else {
				gameTimerText.text = StringToGameTime (timeLeft);
			}
		}
	}

	private void ResetGame() {
		playAgainButton.SetActive(false);
		gameOverText.enabled = false;
		winnerText.enabled = false;
		// Spawn a new ball, reset the characters to their spawn points, and destroy the old ball
		teamOneScore = 0;
		teamTwoScore = 0;
		teamOneScoreText.text = "0";
		teamTwoScoreText.text = "0";


		endTime = Time.time + 180;
		gameActive = true;
	}
		
	public void PlayAgainPressed() {
		ResetGame ();
		Application.LoadLevel (Application.loadedLevel);
	}

	public bool GameIsActive() {
		return gameActive;
	}

	public void GoalScored(int teamNumber) {
		if (gameActive) {
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

	private void GameOver() {
		Debug.Log ("GAME OVER");
		gameActive = false;
		if (teamOneScore == teamTwoScore) {
			winnerText.text = "It's a tie!";
			winnerText.color = Color.white;
		} else if (teamOneScore > teamTwoScore) {
			winnerText.text = "Yellow Wins!";
			winnerText.color = Color.yellow;
		} else {
			winnerText.text = "Blue Wins!";
			winnerText.color = Color.cyan;
		}
		winnerText.enabled = true;
		gameOverText.enabled = true;
		playAgainButton.SetActive(true);
	}

	private string StringToGameTime(int time) {
		int minutes = time / 60;
		string tenMinutes = "";
		string tenSeconds = "";
		int seconds = 0;

		if (minutes > 0) {
			seconds = time % 60;
		} else {
			seconds = time;
		}

		if (seconds < 10) {
			tenSeconds = "0";
		}
		if (minutes < 10) {
			tenMinutes = "";
		}

		return tenMinutes + minutes + ":" + tenSeconds + seconds;
	}
}
