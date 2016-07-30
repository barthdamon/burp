using UnityEngine;
using System.Collections;

[System.Serializable]
public class GlobalState : State {

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
		// Clamp postion within the boundary somehow....
//		Debug.Log ("Executing Global State");
	}

	// Called when state recieves a message
	public override void HandleMessage(Telegram Telegram)
	{
		Debug.Log ("State Disregards Message");
	}
}
