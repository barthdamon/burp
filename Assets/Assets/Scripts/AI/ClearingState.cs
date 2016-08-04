using UnityEngine;
using System.Collections;

[System.Serializable]
public class ClearingState : State {

	float BallOffset = .25f;

	public ClearingState(AIStateMachine AI, EState EState)
	{
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
		CheckForChangeState ();

		if (AI.Ball.transform.position.y < 0) {
			// clear to upper corner
			AI.SetCurrentHeading (CalculateHeadingForClearPoint(new Vector3(-20, 8,0)).normalized);
		} else {
			// clear to bottom corner
			AI.SetCurrentHeading (CalculateHeadingForClearPoint(new Vector3(-20, -8,0)).normalized);
		}
	}

	// Called when state recieves a message
	public override void HandleMessage(Telegram Telegram)
	{
		
	}

	private Vector3 CalculateHeadingForClearPoint(Vector3 ClearPoint)
	{
		Vector3 BallPos = AI.Ball.transform.position;
		Vector3 BallToClear = BallPos - ClearPoint;

		Vector3 NewHeading = BallPos + (BallToClear.normalized * BallOffset);

		return NewHeading;
	}

	private bool CheckForChangeState () 
	{
		// will naturally go on defense, just check if should go on offense here
		// If the ball is past the player and the computer is past the player then start shooting
		if (AI.ComputerDistanceToBall ().x > 0 || !AI.ShotOnTarget (AI.ComputerGoal)) {
			AI.ChangeState (EState.Defending);
			return true;
		} else {
			return false;
		}
	}
}
