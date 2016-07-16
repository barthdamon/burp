using UnityEngine;
using System.Collections;

public class AggressiveContact : MonoBehaviour {

	public float damageAmount = 15f;
	public bool passive = true;
	public ParticleSystem hitParticles;
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
		hitParticles.GetComponent<Renderer> ().material.SetColor ("_TintColor", playerManager.PlayerColor ());
		// set it facing opposite direction from where it was hit...

		Vector3 particleDirection = transform.position - otherLimb.position;
		hitParticles.transform.rotation = Quaternion.identity;
		hitParticles.transform.rotation = Quaternion.FromToRotation (Vector3.back, particleDirection.normalized);
		if (damaged) {
			StartCoroutine (toggleParticles ());
		}
	}

	IEnumerator toggleParticles() {
		hitParticles.Play ();
		yield return new WaitForSeconds(0.10f);
		hitParticles.Stop ();
	}

	//	public LayerMask playerMask;
	//	public ParticleSystem explosionParticles;       
	//	public AudioSource explosionAudio; 
	//	public float maxDamage = 200f;                  
	//	public float explosionForce = 10f;            
	//	public float maxLifeTime = 2f;                  
	//	public float explosionRadius = 10f;              
	//
	//
	//	private void Start()
	//	{
	//		//		Destroy(gameObject, maxLifeTime);
	//	}
	//
	//
	//	private void OnTriggerEnter(Collider other)
	//	{
	//		Collider[] colliders = Physics.OverlapSphere (transform.position, explosionRadius, playerMask);
	//		Debug.Log ("COLLIDERS FOUND: " + colliders.Length);
	//		for (int i = 0; i < colliders.Length; i++) {
	//			Rigidbody targetRb = colliders[i].GetComponent<Rigidbody>();
	//			if (!targetRb)
	//				continue;
	//			targetRb.AddExplosionForce (explosionForce, transform.position, explosionRadius);
	//			//			EnemyController targetController = targetRb.GetComponent<EnemyController> ();
	//			//			if (!targetController)
	//			//				continue;
	//			//			float damage = CalculateDamage (targetRb.position);
	//			//			targetController.TakeDamage (damage);
	//		}
	//
	//		explosionParticles.Play ();
	//		explosionAudio.Play ();
	//		Debug.Log ("DESTROYING");
	//
	//		//		Destroy (explosionParticles.gameObject, explosionParticles.duration);
	//		//		Destroy (gameObject);
	//	}




	// when colliding with floor do nothing
	// each has a part - passive or aggressive
	// if it collides with another aggressive, there is a deflection
	// if it collides with a passive, it does damage


	// should the player lose limbs??? - not now doesnt really add anything here...
	// each player should have a health bar that drains and when its out they drink
}
