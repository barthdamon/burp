using UnityEngine;
using System.Collections;

[System.Serializable]
public class ShootingState : State {

	// Constants
	float BallOffset = 0.5f;
	float ShootingRange = 1f;
	float SlowDownRange = 3f;

	float TopSpeed = 10f;
	float SlowDownSpeed = 8f;
	float ShootingSpeed = 10f;


	public ShootingState(AIStateMachine AI, EState EState)
	{
		this.Name = "Shooting State";
		Debug.Log ("BEING CREATED: " + Name);
		this.AI = AI;
		this.EState = EState;
	}

	// Called when entering this state
	public override void Enter() {
		Debug.Log ("Entering Shooting State");
	}

	// Called when exiting this state
	public override void Exit() {
		Debug.Log ("Exiting Shooting State");
	}

	// Called when in this state
	public override void Execute() {
//		Debug.Log ("Executing Shooting State");
		// Determine if need to switch state to defending

		// First get the position of the ball
		Vector3 BallPos = AI.Ball.transform.position;
		// Need the static position of the center of the goal
		Vector3 GoalPos = new Vector3(20f,0f,0f);

		// Find the vector from the ball to the goal
		Vector3 DesiredTrajectory = BallPos - GoalPos;
//		Debug.Log ("Desired Traj: " + DesiredTrajectory);

		// Find the position behind the ball where the player needs to be (like an offset)
		// Reverse the vector backwards behind the ball by the ballOffset, then set the position as the desired destination
		Vector3 TargetPos = BallPos + (DesiredTrajectory.normalized * BallOffset);
//		Debug.Log ("Target Pos: " + TargetPos);

		Vector3 NewHeading = new Vector3(0f,0f,0f);

		NewHeading = ArriveToShoot(TargetPos);
			
		AI.SetCurrentHeading (NewHeading);
	}

	// Called when state recieves a message
	public override void HandleMessage(Telegram Telegram)
	{
		Debug.Log ("State Disregards Message");
	}


	private Vector3 ArriveToShoot(Vector3 TargetPosition)
	{
		// if the distance to the position from the player is greater than some constant, go there, else shoot
		Vector3 ComputerPlayerPos = AI.ComputerMove.transform.position;
		Vector3 DistanceToDestination = TargetPosition - ComputerPlayerPos;

		if (DistanceToDestination.magnitude > ShootingRange) {
			// Keep going
			if (DistanceToDestination.magnitude > SlowDownRange) {
				AI.SetCurrentSpeed (TopSpeed);
				return DistanceToDestination.normalized;
			} else {
				// figure out if on the wrong side of the ball (always going right or up in this situation)
//				Vector3 BallPos = AI.Ball.transform.position;
				if (DistanceToDestination.x < -1 * SlowDownRange || DistanceToDestination.x > 0f) {
					AI.SetCurrentSpeed (SlowDownSpeed);
					return DistanceToDestination.normalized;
				} else {
					// Get around the ball before continuing
					return CalculateAvoidBallTrajectory (DistanceToDestination.normalized);
				}
			}
		} else {
			// Put through the shoot maneuver somehow...
			// Need logic for if it is right near the goal vs far away. Perhaps find trajectories against the wall that it can shoot against to
			AI.SetCurrentSpeed(ShootingSpeed);
			return DistanceToDestination.normalized;
		}
	}

	private Vector3 CalculateAvoidBallTrajectory(Vector3 Dest) {
		// set a course 45degrees up or down from target pos vector depending on which is closer
		Vector3 AvoidBallTrajectory;
		if (Dest.y > 0) {
			// below the ball
			Debug.Log ("greater than y");
			AvoidBallTrajectory = new Vector3(-1 * Mathf.Cos(Dest.x - (Mathf.PI / 4)), Mathf.Sin(Dest.y - (Mathf.PI / 4)), 0f);
			return AvoidBallTrajectory.normalized;
		} else {
			// above the ball
			Debug.Log ("Less than y");
			AvoidBallTrajectory = new Vector3(Mathf.Cos(Dest.x + (Mathf.PI / 4)), Mathf.Sin(Dest.y + (Mathf.PI / 4)), 0f);
			return AvoidBallTrajectory.normalized;
		}
	}
}
