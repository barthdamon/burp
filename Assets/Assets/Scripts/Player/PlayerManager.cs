using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class Boundary {
	public float xMin, xMax, zMin, zMax, yMin, yMax;
}

public class PlayerManager : MonoBehaviour {

	public int playerNumber;
	public Boundary boundary;
	public AudioSource hitSound;
	public AudioSource deflectSound;
	public Slider playerHealthSlider;
	public Image playerHealthFill;
	public GameManager gameManager;

	private bool knockedOut = false;
	private float health = 100;

	private AIStateMachine StateMachine;

	// Use this for initialization
	void Start () {
		playerHealthFill.color = playerNumber == 1 ? Color.yellow : Color.cyan;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		pos.z = Mathf.Clamp(transform.position.z, 0, 0);
		transform.position = pos;
	}

	public void LostHealth (float amount) {
		if (!knockedOut) {
			health -= amount;
			if (health > 0) {
				playerHealthSlider.value = health / 100;
			} else {
				playerHealthSlider.value = 0;
				health = 0;
				knockedOut = true;
				Debug.Log ("HEALTH OUT, RESETTING");
				StartCoroutine (RechargeHealth ());
			}
		}
	}

	//temporary until drinking is a thing
	void ResetHealth() {
		knockedOut = false;
	}

	IEnumerator RechargeHealth() {
		playerHealthFill.color = Color.red;
		while (knockedOut) {
			Debug.Log ("Recharging....." + health);
			health += 1;
			playerHealthSlider.value = health / 100;
			if (health == 100) {
				playerHealthFill.color = playerNumber == 1 ? Color.yellow : Color.cyan;
				Debug.Log ("Stopping Coroutine");
				ResetHealth ();
				StopCoroutine (RechargeHealth ());
//				return;
			}
			yield return new WaitForSeconds (0.08f);
		}
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
		return health > 0 && !knockedOut && gameManager.GameIsActive();
	}
}
