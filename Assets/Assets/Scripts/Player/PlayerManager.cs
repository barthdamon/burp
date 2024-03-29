﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class Boundary {
	public float xMin, xMax, zMin, zMax, yMin, yMax;
}

public class PlayerManager : MonoBehaviour {

	public GameObject Model;

	public int playerNumber;
	public Boundary boundary;
	public AudioSource hitSound;
	public AudioSource deflectSound;
	public Slider playerHealthSlider;
	public Image playerHealthFill;
	public GameManager gameManager;
	public PlayerMove playerMove;

	public BroadCaster Broadcaster;

	private bool knockedOut = false;
	private float LastHit = 0f;
	private float health = 100;

	private bool RecentlyHit = false;
	private float HitRecharge = 1;


	// Use this for initialization
	void Start () {
		playerHealthFill.color = playerNumber == 1 ? Color.yellow : Color.cyan;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		pos.z = Mathf.Clamp(transform.position.z, 0, 0);
		transform.position = pos;

		if (health < 100 && IsAlive() && Time.time > LastHit + 5f) {
			health += 0.05f;
			playerHealthSlider.value = health / 100;
		}
	}

	public void ResetHealthForStart()
	{
		health = 100f;
		playerHealthSlider.value = 1;
	}

	public void LostHealth (float amount) {
		if (!knockedOut) {
			if (amount > 0) {
				LastHit = Time.time;
			}
			health -= amount;
			RecentlyHit = true;
			StartCoroutine (ResetHittable ());
			if (health > 0) {
				playerHealthSlider.value = health / 100;
			} else {
				playerHealthSlider.value = 0;
				health = 0;
				knockedOut = true;
				Debug.Log ("HEALTH OUT, RESETTING");
				Telegram NewTelegram = new Telegram (EMessage.KnockOutOccured, GetComponent<GameObject>());
				Broadcaster.RelayTelegram (NewTelegram);
				StartCoroutine (RechargeHealth ());
			}
		}
	}

	//temporary until drinking is a thing
	void ResetHealth() {
		knockedOut = false;
	}

	IEnumerator ResetHittable() {
		while (RecentlyHit) {
			HitRecharge -= .5f;
			if (HitRecharge == 0) {
				HitRecharge = 1f;
				RecentlyHit = false;
				StopCoroutine(ResetHittable());
			}	
			yield return new WaitForSeconds(0.5f);
		}
	}

	IEnumerator RechargeHealth() {
		playerHealthFill.color = Color.red;
		while (knockedOut) {
//			Debug.Log ("Recharging....." + health);
			health += 1;
			playerHealthSlider.value = health / 100;
			if (health == 100) {
				playerHealthFill.color = playerNumber == 1 ? Color.yellow : Color.cyan;
//				Debug.Log ("Stopping Coroutine");
				ResetHealth ();
				StopCoroutine (RechargeHealth ());
//				return;
			}
			yield return new WaitForSeconds (0.08f);
		}
	}

	public bool LowHealth() {
		return health < 25 && IsAlive();
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

	public bool IsRecentlyHit() {
		return RecentlyHit;
	}

	public bool CanBeHit() {
		return IsAlive () && !IsRecentlyHit ();
	}
}
