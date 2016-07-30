using UnityEngine;
using System.Collections;

public class BallMove : MonoBehaviour {

	public BoxCollider floorCollider;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		float floorMax = floorCollider.size.x;
		Vector3 pos = transform.position;
		pos.z = Mathf.Clamp (transform.position.z, 0, 0);
//		pos.x = Mathf.Clamp (transform.position.x, -floorMax, floorMax);
//		pos.y = Mathf.Clamp (transform.position.y, 0, 7);
		transform.position = pos;
	}

}
