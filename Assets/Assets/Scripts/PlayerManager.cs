using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary {
	public float xMin, xMax, zMin, zMax, yMin, yMax;
}

public class PlayerManager : MonoBehaviour {

	public int playerNumber;
	public Boundary boundary;
	public float health = 100;
	public AudioSource hitSound;
	public AudioSource deflectSound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		pos.z = Mathf.Clamp(transform.position.z, 0, 0);
		transform.position = pos;
	}

	public void LostHealth (float amount) {
		health -= amount;
		if (health <= 0) {
			Debug.Log ("HEALTH OUT, RESETTING");
			StartCoroutine (ResetHealth ());
		}
		// update health status bar
	}

	//temporary until drinking is a thing
	IEnumerator ResetHealth() {
		yield return new WaitForSeconds (5f);
		health = 100f;
	}

	public void PlayHitSound() {
		hitSound.Play ();
	}

	public void PlayDeflectSound() {
		deflectSound.Play ();
	}

	public Color PlayerColor() {
		return playerNumber == 1 ? Color.yellow : Color.cyan;
	}

	public bool IsAlive() {
		return health > 0;
	}
}
