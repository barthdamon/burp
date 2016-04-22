using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerMove : MonoBehaviour {

	float speed = 8;
	public float ragdollBlendAmount = 0.5f;
	public PlayerManager playerManager;

//	float horizontalMovement;
//	float verticalMovement;

	Rigidbody rb;


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



//	void FixedUpdate() {
//		float hMove = horizontalMovement * speed * Time.deltaTime;
//		float vMove = verticalMovement * speed * Time.deltaTime;
//		Debug.Log ("updating " + rb + " with force horizontal: " + hMove + " Vertical: " + vMove);
//		Vector3 movement = new Vector3 (hMove, vMove, 0f);
////		rb.MovePosition (rb.position + movement);
//		rb.AddForce(transform.up * speed * verticalMovement);
////		rb.position = Vector3.Lerp(rb.position, movement, Time.deltaTime);
//////		rb.MovePosition (rb.position + hMove);
////		rb.AddForce (transform.right * horizontalMovement * speed * Time.deltaTime);
////		rb.AddForce (transform.up * verticalMovement * speed * Time.deltaTime);
//
//

		void FixedUpdate() {
			// Either 1 or zero cause its raw
			string horizontalKey = playerManager.playerNumber == 1 ? "HorizontalOne" : "HorizontalTwo";
			string verticalKey = playerManager.playerNumber == 1 ? "VerticalOne" : "VerticalTwo";
			float h = Input.GetAxisRaw (horizontalKey);
			float v = Input.GetAxisRaw (verticalKey);
			Move (h, v);
		}

		void Move(float h, float v) {
			Vector3 movement = new Vector3 (h, v, 0f);
			Vector3 nMovement = movement.normalized * speed;
	//		transform.Translate (nMovement, Space.World);

			//5 here being gravity
	//		Vector3 movementY = transform.up * speed;
	//		Vector3 movementX = transform.right * speed;
			rb.velocity = nMovement;
	//		Vector3 localVel = transform.InverseTransformDirection (rb.velocity);
	//		rb.velocity.y += v * speed * Time.deltaTime;
	//		rb.velocity.x += h * speed * Time.deltaTime;

	//		rb.velocity = transform.TransformDirection(localVel);
	//		rb.velocity = localVel;

	//		Component[] children=GetComponentsInChildren(typeof(Rigidbody));
	//		for (var i = 0; i < bodyParts.Count; i++) {
	//			Transform childTransform = bodyParts [i].transform;
	//			childTransform.Translate (nMovement, Space.World);
	////			childRb.angularVelocity = rb.angularVelocity;
	//			//child.rigidbody.AddForce (Vector3.up * 1000);
	//		}
			Vector3 pos = transform.position;
			pos.z = Mathf.Clamp(transform.position.z, 0, 0);
			transform.position = pos;

		}


	public void ExplosiveForceAdded(float explosiveForce, Vector3 position, float explosionRadius) {
		Debug.Log ("EXPLOSIVE FORCE BEING ADDED");
		GetComponent<Rigidbody>().AddExplosionForce (explosiveForce, position, explosionRadius);
	}

//	GetComponent<Rigidbody> ().position = new Vector3 (
//		Mathf.Clamp(GetComponent<Rigidbody> ().position.x, boundary.xMin, boundary.xMax),
//		0.0f,
//		Mathf.Clamp(GetComponent<Rigidbody> ().position.z, boundary.zMin, boundary.zMax)
//	);
		
//		foreach (BodyPart b in bodyParts)
//		{
//			if (b.transform!=transform){ //this if is to prevent us from modifying the root of the character, only the actual body parts
//				//position is only interpolated for the hips
////				if (b.transform==anim.GetBoneTransform(HumanBodyBones.Hips))
//				b.transform.position=Vector3.Lerp(b.transform.position, b.transform.position + movement, ragdollBlendAmount);
//				//rotation is interpolated for all body parts
////				b.transform.rotation=Quaternion.Slerp(b.transform.rotation, b.storedRotation, ragdollBlendAmount);
//			}
//		}
//	}
}
