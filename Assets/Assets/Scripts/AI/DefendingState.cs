using UnityEngine;
using System.Collections;

[System.Serializable]
public class DefendingState : State {

	public DefendingState(AIStateMachine AI, EState EState)
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
	}

	// Called when state recieves a message
	public override void HandleMessage(Telegram Telegram)
	{
		Debug.Log ("State Disregards Message");
	}

}
