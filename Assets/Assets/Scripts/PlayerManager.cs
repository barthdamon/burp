using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary {
	public float xMin, xMax, zMin, zMax, yMin, yMax;
}

public class PlayerManager : MonoBehaviour {

	public int playerNumber;
	public Boundary boundary;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		pos.z = Mathf.Clamp(transform.position.z, 0, 0);
		transform.position = pos;
	}
}
