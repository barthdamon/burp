using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerMove : MonoBehaviour {

	public float ragdollBlendAmount = 0.5f;
	public PlayerManager playerManager;

	// AI Variables
	public AIStateMachine AI;
	public bool isComputerPlayer = false;
	private float hInputValue;
	private float vInputValue;


	Rigidbody rb;
	float speed = 10;
	float ProbeLength = 0.5f;


//	//Declare a class that will hold useful information for each body part
	public class BodyPart
	{
		public Transform transform;
		public Vector3 storedPosition;
		public Quaternion storedRotation;
	}

	//Declare a list of body parts, initialized in Start()
	List<BodyPart> bodyParts=new List<BodyPart>();

	// Use this for initialization
	void Start () {
//		rb = gameObject.Find("rig/root/ORG-hips/ORG-spine/ORG-chest/ORG-neck/DEF-head").GetComponent<Rigidbody> ();
		rb = gameObject.GetComponent<Rigidbody> ();

		//Find all the transforms in the character, assuming that this script is attached to the root
		Component[] components=GetComponentsInChildren(typeof(Transform));

		//For each of the transforms, create a BodyPart instance and store the transform 
		foreach (Component c in components)
		{
			BodyPart bodyPart=new BodyPart();
			bodyPart.transform=c as Transform;
			bodyParts.Add(bodyPart);
		}

		GetComponent<Rigidbody>().AddExplosionForce (5f, transform.position, 10f);
	}

	void FixedUpdate() {
		// Either 1 or zero cause its raw
		if (!isComputerPlayer) {
			string horizontalKey = playerManager.playerNumber == 1 ? "HorizontalOne" : "HorizontalTwo";
			string verticalKey = playerManager.playerNumber == 1 ? "VerticalOne" : "VerticalTwo";
			float h = Input.GetAxisRaw (horizontalKey);
			float v = Input.GetAxisRaw (verticalKey);
			Move (h, v);
		} else {
			ComputerMove ();
		}
	}

	void Move(float h, float v) {
		Vector3 movement = new Vector3 (h, v, 0f);
		Vector3 nMovement = movement.normalized * speed;
		Vector3 pos = transform.position;
		pos.z = Mathf.Clamp(transform.position.z, 0, 0);
		transform.position = pos;
		if (playerManager.IsAlive()) {
			rb.velocity = nMovement;
		}
	}

	public void ResetPosition() {
		if (playerManager.playerNumber == 1) {
			transform.position = new Vector3 (6f,1f,0f);
		} else {
			transform.position = new Vector3 (-6f,1f,0f);
		}
	}


	public void ExplosiveForceAdded(float explosiveForce, Vector3 position, float damageAmount) {
		Vector3 direction = transform.position - position;
		float distance = direction.magnitude;
//		direction.Normalize();
		Vector3 force = direction.normalized * explosiveForce;
//		Debug.Log ("x: " + force.x + " y: " + force.y + " z: " + force.z);
//		GetComponent<Rigidbody>().AddForce(1000,180,100, ForceMode.Impulse);
//		GetComponent<Rigidbody>().AddForce(nDirection.x * force, nDirection.y * force, nDirection.z * force, ForceMode.Impulse);
		Component[] children = GetComponentsInChildren(typeof(Rigidbody));
		for (var i = 0; i < children.Length; i++) {
			children [i].GetComponent<Rigidbody> ().AddForce(force.x, force.y, 0, ForceMode.Impulse);
//			children [i].GetComponent<Rigidbody> ().AddExplosionForce (explosiveForce, position, distance);
		}
//		Debug.Log ("EXPLOSIVE FORCE BEING ADDED");
		playerManager.LostHealth (damageAmount);
//		GetComponent<Rigidbody>().AddExplosionForce (explosiveForce, position, explosionRadius);
	}


	// AI METHODS
	void ComputerMove() {
		// Perhaps reduce the speed
		Vector3 nMovement = AI.GetCurrentHeading() * AI.GetCurrentSpeed();
//		Debug.Log("Computer Moving... x:" + nMovement.x.ToString() + " y: " + nMovement.y.ToString());
		Vector3 pos = transform.position;
		pos.z = Mathf.Clamp(transform.position.z, 0, 0);
		transform.position = pos;

		if (playerManager.IsAlive()) {
			rb.velocity = FitToBounds(transform.position, nMovement);
		}
	}

	private Vector3 FitToBounds(Vector3 Position, Vector3 nMovement)
	{
		Vector3 Probe = nMovement;
//		if (Position.x > 20 || Position.x < -20)
//		{
//			if (nMovement.y > 0) {
//				Probe = new Vector3 (0, 1, 0);
//			} else {
//				Probe = new Vector3 (0, -1, 0);
//			}
//		}
//
//		if (Position.y > 7.9 || Position.y < -7.9)
//		{
//			if (nMovement.x > 0) {
//				Probe = new Vector3 (1, 0, 0);
//			} else {
//				Probe = new Vector3 (-1, 0, 0);
//			}
//		}
		return Probe;
	}
		
}
