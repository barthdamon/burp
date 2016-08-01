using UnityEngine;
using System.Collections;

[System.Serializable]
public class ShootingState : State {

	// Constants
	float BallOffset = .5f;
	float ShootingRange = 1f;
	float SlowDownRange = 3f;

	float TopSpeed = 10f;
	float SlowDownSpeed = 10f;
	float ShootingSpeed = 10f;

	// Need the static position of the center of the goal
	Vector3 GoalPos = new Vector3(20f,0f,0f);

	// the constant for how close the human needs to be in shooting range to shoot at walls instead
	float HumanPosAvoidConstant = 0.3f;

	float DistanceToSpin = 10f;

	bool IsSpinningToShoot = false;
	float SpinDegrees = 0f;


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
		Vector3 TargetPos = CalculateTargetPos ();

		Vector3 NewHeading = new Vector3(0f,0f,0f);

		NewHeading = ArriveToShoot(TargetPos);
			
		AI.SetCurrentHeading (NewHeading);
	}

	// Called when state recieves a message
	public override void HandleMessage(Telegram Telegram)
	{
		Debug.Log ("State Disregards Message");
	}





	private Vector3 CalculateTargetPos()
	{
		// First get the position of the ball
		Vector3 BallPos = AI.Ball.transform.position;
		// calculate the future pos of the ball, and aim for that...
//		Vector3 BallPos = CurrentBallPos + AI.Ball.GetComponent<Rigidbody>().velocity.normalized * AI.Ball.GetComponent<Rigidbody>().velocity.magnitude;

		// Find the vector from the goal to the ball
		Vector3 DesiredTrajectory = BallPos - GoalPos;
		//		Debug.Log ("Desired Traj: " + DesiredTrajectory);
		// IF other player is in the way, and computer is outside shooting range, change the target pos to be a relative distance to the goal that produces a good angle to score (around the player)
		Vector3 TargetPos;

		Vector3 VectorToHuman = AI.HumanMove.transform.position - BallPos;
		if (Vector3.Dot (VectorToHuman, DesiredTrajectory) > HumanPosAvoidConstant) {
			// get cross product with the goal being the destination to find the position on the wall it needs to get to
		} else {
			
		}

		// Find the position behind the ball where the player needs to be (like an offset)
		// Reverse the vector backwards behind the ball by the ballOffset, then set the position as the desired destination
		TargetPos = BallPos + (DesiredTrajectory.normalized * BallOffset);
		//		Debug.Log ("Target Pos: " + TargetPos);

		return TargetPos;
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
			AI.SetCurrentSpeed(ShootingSpeed);
			if ((GoalPos - ComputerPlayerPos).magnitude > DistanceToSpin) {
				// now that he is in shooting position, perhaps he can swing for either side of the wall that lets him bounce up to it.
				// need to follow through a spin...
				if (!IsSpinningToShoot) {
					// start rotating
				}
				return DistanceToDestination.normalized;
			} else {
				return DistanceToDestination.normalized;
			}
		}
	}

	private Vector3 CalculateAvoidBallTrajectory(Vector3 Dest) {
		// set a course 45degrees up or down from target pos vector depending on which is closer
		Vector3 AvoidBallTrajectory;
		if (Dest.y > 0) {
			// below the ball
//			Debug.Log ("greater than y");
			AvoidBallTrajectory = new Vector3(-1 * Mathf.Cos(Dest.x - (Mathf.PI / 4)), Mathf.Sin(Dest.y - (Mathf.PI / 4)), 0f);
			return AvoidBallTrajectory.normalized;
		} else {
			// above the ball
//			Debug.Log ("Less than y");
			AvoidBallTrajectory = new Vector3(Mathf.Cos(Dest.x + (Mathf.PI / 4)), Mathf.Sin(Dest.y + (Mathf.PI / 4)), 0f);
			return AvoidBallTrajectory.normalized;
		}
	}
}
