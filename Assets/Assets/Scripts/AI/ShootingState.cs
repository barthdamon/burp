﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class ShootingState : State {

	// Constants
	float BallOffset = .25f;
	float ShootingRange = 1f;
	float SlowDownRange = 3f;

	float TopSpeed = 10f;
	float SlowDownSpeed = 8f;
	float ShootingSpeed = 10f;
	float ShootOffsetConst = 16f;

	// Need the static position of the center of the goal
	Vector3 GoalPos = new Vector3(20f,0f,0f);

	// the constant for how close the human needs to be in shooting range to shoot at walls instead
	float HumanPosAvoidConstant = 0.8f;

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
//		Debug.Log ("Entering Shooting State");
	}

	// Called when exiting this state
	public override void Exit() {
//		Debug.Log ("Exiting Shooting State");
	}

	// Called when in this state
	public override void Execute() {
//		Debug.Log ("Executing Shooting State");
		// Determine if need to switch state to defending

		// Deal with when caught going back and forth up and down trying to score in front of human players goal

//		if (AI.ComputerDistanceToBall ().magnitude > AI.HumanDistanceToBall ().magnitude + 7) {
//			AI.ChangeState (EState.Retreating);
//		}

		if ((AI.Ball.transform.position - AI.ComputerMove.transform.position).x < 0) {
			BallOffset = .75f;
		} else {
			BallOffset = .25f;
		}


		Vector3 TargetPos = CalculateTargetPos ();

		Vector3 NewHeading = new Vector3(0f,0f,0f);

		NewHeading = ArriveToShoot(TargetPos);
			
		AI.SetCurrentHeading (NewHeading);
	}

	// Called when state recieves a message
	public override void HandleMessage(Telegram Telegram)
	{
		switch (Telegram.Message) {
		case EMessage.ComputerLowHealth:
			AI.ChangeState (EState.Evading);
			break;
		case EMessage.PlayerLowHealth:
			// dont wait to attack if you already have the human beat
			if (AI.ComputerMove.transform.position.x - AI.HumanMove.transform.position.x > 0) {
				AI.ChangeState (EState.Attacking);
			}
			break;
		default:
			break;
		}
	}
		



	private Vector3 CalculateTargetPos()
	{

		// First get the position of the ball
		if (AI.ShotOnTarget (AI.HumanGoal)) {
//			Debug.Log("Ball on target");
			return new Vector3 (0f, 0f, 0f);
		}

		Vector3 BallPos = AI.Ball.transform.position;
		if ((AI.Ball.transform.position - AI.ComputerMove.transform.position).x > -1 ) {
			BallPos = AI.Ball.transform.position;
		} else {
			BallPos = AI.Ball.GetFuturePositionFromDistance (AI.ComputerDistanceToBall ().magnitude, AI.GetCurrentSpeed ());
		}

		Vector3 DesiredTrajectory = BallPos - GoalPos;
		Vector3 TargetPos;

		Vector3 Dest = DesiredTrajectory.normalized;
		TargetPos = BallPos + (Dest * BallOffset);
		//		Debug.Log ("Target Pos: " + TargetPos);

		// IF other player is in the way, and computer is outside shooting range, change the target pos to be a relative distance to the goal that produces a good angle to score (around the player)
		Vector3 VectorToHuman = AI.HumanMove.transform.position - BallPos;
		if ((Vector3.Dot (VectorToHuman, DesiredTrajectory) > HumanPosAvoidConstant) && DesiredTrajectory.magnitude > 8 && (AI.Ball.transform.position - AI.HumanMove.transform.position).x < 0) {
			// Human is in the way, try to shoot around him
			Vector3 ScorePastHumanTrajectory = new Vector3(Mathf.Cos(Dest.x - (Mathf.PI / 8)), Mathf.Sin(Dest.y - (Mathf.PI / 8)), 0f);
			TargetPos = BallPos + (ScorePastHumanTrajectory * BallOffset);
		}

		// If computer is on the opposite side of the desired pos (negative dot product with desired pos vec and computer to ball)
			// go slightly down and back away from the ball
		if (Vector3.Dot (Dest, AI.ComputerDistanceToBall().normalized) > 0.1 && AI.ComputerDistanceToBall().x < 0) {
			// go sideways then directly to the point
			float YPosition = AI.ComputerDistanceToBall().y;
			float XPosition = AI.ComputerDistanceToBall().x;
			TargetPos = BallPos + new Vector3 ((XPosition * 2f), (YPosition * 2f), 0f);
		}


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
				AI.SetCurrentSpeed (SlowDownSpeed);
				return DistanceToDestination.normalized;
			}
		} else {
			// Put through the shoot maneuver somehow...
			AI.SetCurrentSpeed(ShootingSpeed);
//			float Offset = AI.Ball.transform.position.y / ShootOffsetConst;
//			TargetPosition.y += Offset;
			Vector3 DestVec = TargetPosition - ComputerPlayerPos;
			if ((GoalPos - ComputerPlayerPos).magnitude > DistanceToSpin) {
				// now that he is in shooting position, perhaps he can swing for either side of the wall that lets him bounce up to it.
				// need to follow through a spin...
				if (!IsSpinningToShoot) {
					// start rotating last second somehow
				}
				return DestVec.normalized;
			} else {
				return DestVec.normalized;
//				return DistanceToDestination.normalized;
			}
		}
	}


}
