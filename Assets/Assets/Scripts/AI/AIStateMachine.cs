using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIStateMachine : MonoBehaviour {

	// For the AI State machine to monitor the Human Player and the ball
	public BallMove Ball;
	public PlayerManager HumanPlayer;
	public PlayerMove HumanMove;

	// For the AI State machine to change the computers behavior
	public PlayerManager ComputerManager;
	public PlayerMove ComputerMove;

	// An array to store all of the state singletons
	private List<State> StateSingletons;
	// This stores the base steering behaviors
	private State m_GlobalState;

	private State CurrentState;
	private State PreviusState;

	private Vector3 CurrentHeading;
	private float CurrentSpeed = 10f;


	// Use this for initialization
	void Start () {

	}

	public void StartAI() {
		m_GlobalState = new GlobalState(this, EState.Global);

		StateSingletons = new List<State>();
		StateSingletons.Add (new ShootingState(this, EState.Shooting));
		StateSingletons.Add (new EvadingState(this, EState.Evading));
		StateSingletons.Add (new DefendingState(this, EState.Defending));
		StateSingletons.Add (new AttackingState(this, EState.Attacking));

		BeginSimulation ();
	}

	// Update is called once per frame
	void Update () {
//		Debug.Log ("Current State: " + CurrentState.Name + "Global State: " + m_GlobalState.Name);
		if (CurrentState != null && m_GlobalState != null) 
		{
//			Debug.Log ("AI MACHINE EXECUTING");
			CurrentState.Execute ();
			m_GlobalState.Execute ();
		}
	}

	void BeginSimulation() 
	{
		ChangeState (EState.Shooting);
	}

	public State FetchState(EState EState)
	{
		for (int i = 0; i < StateSingletons.Count; i++)
		{
			if (StateSingletons [i].EState == EState) 
			{
				return StateSingletons [i];
			}
		}
		return null;
	}

	public void ChangeState(EState NewStateEState)
	{
		State NewState = FetchState (NewStateEState);
		if (NewState != null && NewState != CurrentState)
		{
			if (CurrentState != null) {
				Debug.Log ("AI Changing state from : " + CurrentState.EState + ", to: " + NewState.EState);
			}
			PreviusState = CurrentState;
			CurrentState = NewState;

			if (PreviusState != null) {
				PreviusState.Exit ();
			}
			CurrentState.Enter ();
		}
	}



	public void HandleMessage(Telegram Telegram) 
	{
		CurrentState.HandleMessage (Telegram);
	}



	// Getters and Setters
	public void SetCurrentHeading(Vector3 NewHeading)
	{
//		Debug.Log ("New Heading: " + NewHeading);
		CurrentHeading = NewHeading;
	}

	public Vector3 GetCurrentHeading()
	{
		return CurrentHeading;
	}

	public void SetCurrentSpeed(float NewSpeed)
	{
		CurrentSpeed = NewSpeed;
	}

	public float GetCurrentSpeed()
	{
		return CurrentSpeed;
	}


	// Math Calculation Helpers
	public bool BallBehindPlayer() {
		Vector3 BallPos = Ball.transform.position;
		Vector3 PlayerPos = HumanMove.transform.position;
		return BallPos.x > PlayerPos.x;
	}

	public bool ComputerBehindPlayer() {
		Vector3 ComputerPos = ComputerMove.transform.position;
		Vector3 PlayerPos = HumanMove.transform.position;
		return ComputerPos.x > PlayerPos.x;
	}

}
	

