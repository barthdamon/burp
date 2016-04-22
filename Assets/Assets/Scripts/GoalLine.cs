using UnityEngine;
using System.Collections;

public class GoalLine : MonoBehaviour {

	public GameManager gameManager;
	public int teamNumber;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Ball") {
			BallExplosion explosion = other.gameObject.GetComponent<BallExplosion> ();
			// ball.start being electrified
			explosion.ToggleOnGoalLine(true);
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Ball") {
			bool goal2 = other.gameObject.transform.position.x < transform.position.x;
			bool goal1 = other.gameObject.transform.position.x > transform.position.x;

			bool goal = teamNumber == 1 ? goal1 : goal2;
			BallExplosion explosion = other.gameObject.GetComponent<BallExplosion> ();
			if (goal) {
				explosion.Explode ();
				gameManager.GoalScored (teamNumber);
				// explode the ball
			} else {
				// stop electrifying the ball
				explosion.ToggleOnGoalLine(false);
			}
		}
	}

}
