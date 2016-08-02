using UnityEngine;
using System.Collections;

[System.Serializable]
public class EvadingState : State {

	public EvadingState(AIStateMachine AI, EState EState)
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

		Vector3 PlayerPos = AI.HumanMove.transform.position;
		Vector3 ComputerPos = AI.ComputerMove.transform.position;

		Vector3 AvoidRoute = ComputerPos - PlayerPos;
		AI.SetCurrentHeading (AvoidRoute.normalized);
	}

	// Called when state recieves a message
	public override void HandleMessage(Telegram Telegram)
	{
	}

	private bool CheckForChangeState () 
	{
		// will naturally go on defense, just check if should go on offense here
		if (AI.BallBehindPlayer () && AI.ComputerBehindPlayer ()) {
			AI.ChangeState (EState.Shooting);
			return true;
		} else {
			return false;
		}
	}
}
