using UnityEngine;
using System.Collections;

[System.Serializable]
public class SpinningToShootState : State {

	public SpinningToShootState(AIStateMachine AI, EState EState)
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

		Vector3 ComputerToHuman = PlayerPos - ComputerPos;

		AI.SetCurrentHeading (ComputerToHuman.normalized);
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
		}
	}

	private bool CheckForChangeState () 
	{
		// will naturally go on defense, just check if should go on offense here
		// If the ball is past the player and the computer is past the player then start shooting
		if (AI.BallBehindPlayer () && AI.ComputerBehindPlayer ()) {
			AI.ChangeState (EState.Shooting);
			return true;
		} else {
			return false;
		}
	}
}
