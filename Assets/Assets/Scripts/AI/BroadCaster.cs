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

	public void SendMessage(Telegram Telegram)
	{
		// get the state machine and send the message to the state machine somehow!
		ComputerPlayer.HandleMessage(Telegram);
	}
}

public enum EMessage { PlayerScored, ComputerScored, PlayerContact, PlayerKnockedOut, ComputerKnockedOut };
public struct Telegram {

	public EMessage Message;
	public Object Sender;

	public Telegram(EMessage Message, BroadCaster Sender)
	{
		this.Message = Message;
		this.Sender = Sender;
	}
}