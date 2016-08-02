using UnityEngine;
using System.Collections;

public enum EState { Global, Shooting, Defending, Attacking, Evading };

[System.Serializable]
public abstract class State {
	// Any variables or methods that all states need to have
	public string Name;
	public AIStateMachine AI;
	public EState EState;

	public State() {}

	// Called when entering this state
	public abstract void Enter();

	// Called when exiting this state
	public abstract void Exit();

	// Called when in this state
	public abstract void Execute();

	// Called when state recieves a message
	public virtual void HandleMessage(Telegram Telegram)
	{
	}

}
