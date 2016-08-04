using UnityEngine;
using System.Collections;

[System.Serializable]
public class DefendingState : State {

	// Need the static position of the center of the goal
	Vector3 GoalPos = new Vector3(-20f,0f,0f);
	float BlockingDistancePercentageScalar = 3f;
	float DistanceToFullGoalBlocking = 7f;
	float PercentageCloserThanPlayer = 0.5f;

	public DefendingState(AIStateMachine AI, EState EState)
	{
		this.AI = AI;
		this.EState = EState;
	}

	// Called when entering this state
	public override void Enter() {
//		Debug.Log ("Entering Defending State");
	}

	// Called when exiting this state
	public override void Exit() {
	}

	// Called when in this state
	public override void Execute() {


		// if comptuer is way closer to the ball than the player, start shooting
		Vector3 BallPos = AI.Ball.transform.position;
		Vector3 PlayerToBall = (BallPos - AI.HumanMove.transform.position);
		Vector3 ComputerToBall = (BallPos - AI.ComputerMove.transform.position);
		if ((PlayerToBall.magnitude / (PlayerToBall.magnitude + ComputerToBall.magnitude) >= PercentageCloserThanPlayer) && ComputerToBall.x > 0f) {
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
		case EMessage.KnockOutOccured:
			if (Telegram.Sender == AI.HumanPlayer.GetComponent<GameObject> ()) {
				AI.ChangeState (EState.Shooting);
			}
			break;
		case EMessage.GoalScored:
			AI.ChangeState (EState.Shooting);
			break;
		case EMessage.ComputerLowHealth:
			AI.ChangeState (EState.Evading);
			break;
		case EMessage.PlayerLowHealth:
			// dont wait to attack if you already have the human beat
//			if (AI.ComputerMove.transform.position.x - AI.HumanMove.transform.position.x > 0) {
				AI.ChangeState (EState.Attacking);
//			}
			break;
		default:
			break;
		}
	}

	private Vector3 CalculateTargetPos()
	{
		// First get the position of the ball
		Vector3 BallPos;
		if (AI.ComputerDistanceToBall ().magnitude > 5f) {
			BallPos = AI.Ball.GetFuturePositionFromDistance (AI.ComputerDistanceToBall ().magnitude, AI.GetCurrentSpeed ());
		} else {
			BallPos = AI.Ball.transform.position;
		}


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
		return DistanceToDestination.normalized;

//		Vector3 ComputerVectorToBall = AI.Ball.transform.position - ComputerPlayerPos;
//		if (ComputerVectorToBall.x > 0.5) {
//			return DistanceToDestination.normalized;
//		} else if ((GoalPos - ComputerPlayerPos).magnitude < 3) {
//			// computer is too close to the goal to try and dodge
//			return DistanceToDestination.normalized;
//		} else {
//			return CalculateAvoidBallTrajectory (DistanceToDestination.normalized);
//		}
		// If about the hit the ball, steer away from it, otherwise just go there
	}

}
