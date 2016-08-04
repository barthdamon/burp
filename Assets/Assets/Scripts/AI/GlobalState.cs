using UnityEngine;
using System.Collections;

[System.Serializable]
public class GlobalState : State {

	float BallDodgeDistance = 3f;
	float TurnAroundDodgeDistance = 1f;
	float HumanPosAvoidConstant = 0.3f;
	Vector3 GoalPos = new Vector3(20f,0f,0f);

	float DistanceToAttackEvade = 5f;
	EMessage MessageToSend;


	public GlobalState(AIStateMachine AI, EState EState)
	{
		this.Name = "Global State";
		Debug.Log ("BEING CREATED: " + Name);
		this.AI = AI;
		this.EState = EState;
	}

	// Called when entering this state
	public override void Enter() {
	}

	// Called when exiting this state
	public override void Exit() {
	}

	// Called when in this state
	public override void Execute() {
		// Always need to be ready to defend
		Vector3 BallPos = AI.Ball.transform.position;
		Vector3 HumanPos = AI.HumanMove.transform.position;
		Vector3 ComputerPos = AI.ComputerMove.transform.position;

		// Find the vector from the ball to the goal
		Vector3 DesiredTrajectory = BallPos - GoalPos;
		Vector3 VectorToHuman = AI.HumanMove.transform.position - BallPos;

		if (DetermineIfHealthLow()) {
			Telegram Telegram = new Telegram (MessageToSend, null);
			AI.HandleMessage (Telegram);
		}


		// Override current heading to make sure doesnt hit ball into its own goal
		Vector3 ComputerToBall = BallPos - ComputerPos;
		if (ComputerToBall.x < -2 && ComputerToBall.magnitude < BallDodgeDistance) {
			Vector3 NewHeading = AI.GetCurrentHeading();
			AI.SetCurrentHeading(CalculateAvoidBallTrajectory(NewHeading));
		}

		if (((BallPos - ComputerPos).x < 0) && ((BallPos - HumanPos).x < 0) && BallPos.x < 15 && AI.HumanPlayer.IsAlive()) {
			AI.ChangeState (EState.Defending);
		}

		if (ComputerToBall.x < 0 && ComputerToBall.magnitude < TurnAroundDodgeDistance) {
			AI.SetCurrentHeading (-ComputerToBall.normalized);
		}

		// Do something to change the current heading so that it doesn't jam into the wall here.....
		AI.SetCurrentHeading(RasberryJam());
	}

	// Called when state recieves a message
	public override void HandleMessage(Telegram Telegram) {}

	private bool DetermineIfHealthLow() {
		float PlayerDistance = (AI.ComputerMove.transform.position - AI.HumanMove.transform.position).magnitude;
		if (AI.HumanPlayer.LowHealth() && AI.ComputerManager.LowHealth()) {
			return false;
		} else if (AI.HumanPlayer.LowHealth() && PlayerDistance < DistanceToAttackEvade) {
			MessageToSend = EMessage.PlayerLowHealth;
			return true;
		} else if (AI.ComputerManager.LowHealth() && PlayerDistance < DistanceToAttackEvade) {
			if ((GoalPos - AI.Ball.transform.position).magnitude > 5f) {
				// dont ever want to evade when the player is about to score
				MessageToSend = EMessage.ComputerLowHealth;
				return true;
			}
		}
		return false;
	}

	private Vector3 CalculateAvoidBallTrajectory(Vector3 Dest) {
		// set a course 45degrees up or down from target pos vector depending on which is closer
		Vector3 AvoidBallTrajectory;
		if (AI.Ball.transform.position.y > -5) {
			if (AI.ComputerMove.transform.position.y < 0) {
				// below the ball
				//			Debug.Log ("greater than y");
				AvoidBallTrajectory = new Vector3 (Mathf.Cos (Dest.x - (Mathf.PI / 4)), Mathf.Sin (Dest.y - (Mathf.PI / 4)), 0f);
				if (AvoidBallTrajectory.x > 0) {
					AvoidBallTrajectory.x *= -1;
				}
				return AvoidBallTrajectory.normalized;
			} else {
				// above the ball
				//			Debug.Log ("Less than y");
				AvoidBallTrajectory = new Vector3 (Mathf.Cos (Dest.x - (Mathf.PI / 4)), Mathf.Sin (Dest.y - (Mathf.PI / 4)), 0f);
				if (AvoidBallTrajectory.x > 0) {
					AvoidBallTrajectory.x *= -1;
				}
				return AvoidBallTrajectory.normalized;
			}
		} else {
			return Dest;
		}

	}

	private Vector3 RasberryJam() {
		// clamp the heading normals so that it doesnt try to go through walls
		return AI.GetCurrentHeading();
	}
}
