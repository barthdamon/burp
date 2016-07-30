using UnityEngine;
using System.Collections;

public class AggressiveContact : MonoBehaviour {

	public float damageAmount = 15f;
	public bool passive = true;
	public ParticleSystem hitParticlePrefab;

	private ParticleSystem hitParticles;
	private PlayerManager playerManager;
	private float explosionForce = 3f;

	// Use this for initialization
	void Start () {
		playerManager = transform.root.GetComponent<PlayerManager>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		pos.y = Mathf.Clamp(transform.position.y, -7.9f, 7.9f);
		transform.position = pos;
	}

	void OnCollisionEnter(Collision collisionInfo) {
		if (collisionInfo.gameObject.tag != "PlayerBody")
			return;

		Transform otherLimbTransform = collisionInfo.gameObject.transform;

		PlayerManager collidedPlayer = collisionInfo.gameObject.transform.root.GetComponent<PlayerManager>();

		if (collidedPlayer.playerNumber == playerManager.playerNumber)
			return;

		AggressiveContact otherPlayerPart = collisionInfo.gameObject.GetComponent<AggressiveContact> ();

		PlayerMove otherPlayerMove = collidedPlayer.GetComponentInChildren<PlayerMove> ();
		PlayerMove playerMove = playerManager.GetComponentInChildren<PlayerMove> ();

		// this will deal 2x the explosions....
		// Both not passive
		bool damaged = false;
		if (!passive && !otherPlayerPart.passive) {
//			explode = true;
			// cause deflection, both explode no damage
//			Debug.Log("DEFLECT");
		}

		if (passive && !otherPlayerPart.passive) {
			damaged = true;
//			explode = true;
			// this gets damaged
//			Debug.Log("Player DAMAGED");
		}

		// Both passive
		//TODO: only cause one of the explosions not x2...
		if (passive && otherPlayerPart.passive) {
//			explode = true;
			damaged = true;
			// deal both of them damage, both explode
//			Debug.Log("DAMAGE BOTH");
		}

		if (otherPlayerMove.playerManager.IsAlive() && playerManager.IsAlive()) {
			playHitParticles (damaged, otherLimbTransform);
			float amountDamaged = 0f;
			if (damaged) {
				amountDamaged = damageAmount;
				playerManager.PlayHitSound ();
			} else {
				playerManager.PlayDeflectSound ();
			}
			playerMove.ExplosiveForceAdded (explosionForce, otherPlayerPart.transform.position, amountDamaged);
		}
	}

	void playHitParticles(bool damaged, Transform otherLimb) {
		// set it facing opposite direction from where it was hit...
		Vector3 particleDirection = transform.position - otherLimb.position;
		Vector3 pos = transform.position;
		if (damaged) {
			hitParticles = null;
			hitParticles = Instantiate(hitParticlePrefab, pos, Quaternion.FromToRotation (Vector3.back, particleDirection.normalized)) as ParticleSystem;
			hitParticles.GetComponent<Renderer> ().material.SetColor ("_TintColor", playerManager.PlayerColor ());
			hitParticles.Play ();
		}
	}
}
