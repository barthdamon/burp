using UnityEngine;
using System.Collections;

[System.Serializable]
public class GlobalState : State {

	float HumanPosAvoidConstant = 0.3f;
	Vector3 GoalPos = new Vector3(20f,0f,0f);

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

		// If the computer is on the wrong side of the ball, 
		// and the human is on their correct side and the human is lined up to hit the ball
		// within a certain range of the ball, then go on defense
		// also not on the opponents end
		if (((BallPos - ComputerPos).x < 0) && ((BallPos - HumanPos).x < 0) && BallPos.x < 15 && AI.HumanPlayer.IsAlive()) {
			AI.ChangeState (EState.Defending);
		}
	}

	// Called when state recieves a message
	public override void HandleMessage(Telegram Telegram)
	{
		Debug.Log ("State Disregards Message");

	}
}
