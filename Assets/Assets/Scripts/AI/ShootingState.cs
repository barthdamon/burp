using UnityEngine;
using System.Collections;

[System.Serializable]
public class ShootingState : State {

	// Constants
	float BallOffset = 0.5f;
	float ShootingRange = 1f;
	float SlowDownRange = 3f;

	float TopSpeed = 10f;
	float SlowDownSpeed = 5f;
	float ShootingSpeed = 2f;


	public ShootingState(AIStateMachine AI, EState EState)
	{
		this.Name = "Shooting State";
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
		Debug.Log ("Executing Shooting State");
		// First get the position of the ball
		Vector3 BallPos = AI.Ball.transform.position;
		// Need the static position of the center of the goal
		Vector3 GoalPos = new Vector3(20f,0f,0f);

		// Find the vector from the ball to the goal
		Vector3 DesiredTrajectory = GoalPos - BallPos;

		// Find the position behind the ball where the player needs to be (like an offset)
		// Reverse the vector backwards behind the ball by the ballOffset, then set the position as the desired destination
		Vector3 TargetPos = -1 * DesiredTrajectory * BallOffset;

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
		Vector3 ComputerPlayerPos = AI.ComputerManager.transform.position;
		Vector3 DistanceToDestination = TargetPosition - ComputerPlayerPos;

		if (DistanceToDestination.magnitude > ShootingRange) {
			// Keep going
			AI.SetCurrentSpeed(DistanceToDestination.magnitude > SlowDownRange ? TopSpeed : SlowDownSpeed);
			return DistanceToDestination.normalized;
		} else {
			// Calculate shoot position
			// Player is on the wrong side of the ball, how to get it to move to the right side before hitting....
			// Prevent player from collider with the ball on a vector that would direct it towards its own goal (make it try to keep going to the other side)
			// once within a certain range and on correct side, pull some kind of hit maneuver in the direction of the ball (up, left, down to get leg swung around)
			AI.SetCurrentSpeed(ShootingSpeed);
			return DistanceToDestination.normalized;
		}

	}
}
