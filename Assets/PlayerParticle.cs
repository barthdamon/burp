using UnityEngine;
using System.Collections;

public class PlayerParticle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Object.Destroy (gameObject, 0.3f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
