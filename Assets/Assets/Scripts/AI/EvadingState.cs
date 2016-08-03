using UnityEngine;
using System.Collections;

[System.Serializable]
public class EvadingState : State {

	Vector3 GoalPos = new Vector3(-20f,0f,0f);

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

		if (AvoidRoute.magnitude > 7f) {
			AI.ChangeState (EState.Defending);
		}

		AI.SetCurrentHeading (AvoidRoute.normalized);
	}

	// Called when state recieves a message
	public override void HandleMessage(Telegram Telegram)
	{
		switch (Telegram.Message) {
		case EMessage.KnockOutOccured:
			if (Telegram.Sender == AI.HumanPlayer.GetComponent<GameObject> ()) {
				AI.ChangeState (EState.Shooting);
			} else {
				AI.ChangeState (EState.Defending);
			}
			break;
		case EMessage.PlayerLowHealth:
			AI.ChangeState (EState.Shooting);
			break;
		}
	}

	private bool CheckForChangeState () 
	{
		// will naturally go on defense, just check if should go on offense here
		if (AI.BallBehindPlayer () && AI.ComputerBehindPlayer ()) {
			AI.ChangeState (EState.Shooting);
			return true;
		} else if ((GoalPos - AI.Ball.transform.position).magnitude < 5f) {
			AI.ChangeState (EState.Defending);
			return true;
		} else {
			return false;
		}
	}
}
