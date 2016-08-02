using UnityEngine;
using System.Collections;

public class BroadCaster : MonoBehaviour {

	public AIStateMachine ComputerPlayer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RelayTelegram(Telegram Telegram)
	{
		// get the state machine and send the message to the state machine somehow!
		ComputerPlayer.HandleMessage(Telegram);
	}
}

public enum EMessage { GoalScored, PlayerContact, KockOutOccured, PlayerLowHealth, ComputerLowHealth };
public struct Telegram {

	public EMessage Message;
	public GameObject Sender;

	public Telegram(EMessage Message, GameObject Sender)
	{
		this.Message = Message;
		this.Sender = Sender;
	}
}