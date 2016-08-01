using UnityEngine;
using System.Collections;

[System.Serializable]
public class DefendingState : State {

	// Need the static position of the center of the goal
	Vector3 GoalPos = new Vector3(-20f,0f,0f);
	float BlockingDistancePercentageScalar = 3f;
	float DistanceToFullGoalBlocking = 5f;
	float PercentageCloserThanPlayer = 0.7f;

	public DefendingState(AIStateMachine AI, EState EState)
	{
		this.AI = AI;
		this.EState = EState;
	}

	// Called when entering this state
	public override void Enter() {
		Debug.Log ("Entering Defending State");
	}

	// Called when exiting this state
	public override void Exit() {
	}

	// Called when in this state
	public override void Execute() {

		// if comptuer is way closer to the ball than the player, start shooting
		Vector3 BallPos = AI.Ball.transform.position;
		float PlayerToBall = (BallPos - AI.HumanMove.transform.position).magnitude;
		float ComputerToBall = (BallPos - AI.ComputerMove.transform.position).magnitude;
		if (PlayerToBall / (PlayerToBall + ComputerToBall) >= PercentageCloserThanPlayer) {
			AI.ChangeState (EState.Shooting);
			return;
		}

		Vector3 TargetPos = CalculateTargetPos ();

		Vector3 NewHeading = new Vector3(0f,0f,0f);

		NewHeading = ArriveToDefend(TargetPos);

		AI.SetCurrentHeading (NewHeading);
	}

	// Called when state recieves a message
	public override void HandleMessage(Telegram Telegram)
	{
		switch (Telegram.Message) {
		case EMessage.KockOutOccured:
			if (Telegram.Sender == AI.HumanPlayer.GetComponent<GameObject> ()) {
				AI.ChangeState (EState.Shooting);
			}
			break;
		case EMessage.GoalScored:
			AI.ChangeState (EState.Shooting);
			break;
		default:
			break;
		}
	}

	private Vector3 CalculateTargetPos()
	{
		// First get the position of the ball
		Vector3 BallPos = AI.Ball.transform.position;

		// Find the vector from the ball to the goal
		Vector3 DesiredTrajectory = BallPos - GoalPos;
		Vector3 TargetPos;
		if (DesiredTrajectory.magnitude > DistanceToFullGoalBlocking) {
			// Multiply the trajectory by the percentage away from the goal the player should be
			TargetPos = GoalPos + DesiredTrajectory.normalized * (DesiredTrajectory.magnitude / BlockingDistancePercentageScalar);
		} else {
			// Multiply the trajectory by the percentage away from the goal the player should be
			Vector3 BlockPos = new Vector3(GoalPos.x, BallPos.y, 0f);
			TargetPos = BlockPos + DesiredTrajectory.normalized * (DesiredTrajectory.magnitude / BlockingDistancePercentageScalar);
		}

		return TargetPos;
		
	}

	private Vector3 ArriveToDefend(Vector3 TargetPosition)
	{
		Vector3 ComputerPlayerPos = AI.ComputerMove.transform.position;
		Vector3 DistanceToDestination = TargetPosition - ComputerPlayerPos;

		Vector3 DistanceToBall = AI.Ball.transform.position - ComputerPlayerPos;
		if (DistanceToBall.magnitude > 0.5) {
			return DistanceToDestination.normalized;
		} else {
			return CalculateAvoidBallTrajectory (DistanceToDestination.normalized);
		}
		// If about the hit the ball, steer away from it, otherwise just go there
	}

	private Vector3 CalculateAvoidBallTrajectory(Vector3 Dest) {
		// set a course 45degrees up or down from target pos vector depending on which is closer
		Vector3 AvoidBallTrajectory;
		if (Dest.y > 0) {
			// below the ball
			AvoidBallTrajectory = new Vector3(-1 * Mathf.Cos(Dest.x - (Mathf.PI / 4)), Mathf.Sin(Dest.y - (Mathf.PI / 4)), 0f);
			return AvoidBallTrajectory.normalized;
		} else {
			// above the ball
			AvoidBallTrajectory = new Vector3(Mathf.Cos(Dest.x + (Mathf.PI / 4)), Mathf.Sin(Dest.y + (Mathf.PI / 4)), 0f);
			return AvoidBallTrajectory.normalized;
		}
	}

}
