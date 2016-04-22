using UnityEngine;
using System.Collections;

public class AggressiveContact : MonoBehaviour {

	public bool passive = true;
	private PlayerManager player;

	// Use this for initialization
	void Start () {
		player = transform.root.GetComponent<PlayerManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collisionInfo) {
		if (collisionInfo.gameObject.tag != "PlayerBody")
			return;
		
		PlayerManager collidedPlayer = collisionInfo.gameObject.transform.root.GetComponent<PlayerManager>();

		if (collidedPlayer.playerNumber == player.playerNumber)
			return;

		AggressiveContact otherPlayer = collisionInfo.gameObject.GetComponent<AggressiveContact> ();


		// Both not passive
		if (!passive && !otherPlayer.passive) {
			// cause deflection
			Debug.Log("DEFLECT");
		}

		// Hit a passive target
		if (!passive && otherPlayer.passive) {
			// deal damage
			Debug.Log("DAMAGE OTHER");
		}

		// Both passive
		if (passive && otherPlayer.passive) {
			// deal both of them damage
			Debug.Log("DAMAGE BOTH");
		}
			
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
