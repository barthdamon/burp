using UnityEngine;
using System.Collections;
using System.Resources;

public class BallExplosion : MonoBehaviour {
	public LayerMask playerMask;
	public ParticleSystem explosionParticles;       
	public AudioSource explosionAudio;                  
	public float explosionForce = 200f;              
	public float explosionRadius = 200f;

	public Material ballGoalLineMaterial;
	public Material ballMaterial;


	void Start()
	{
//		Destroy(gameObject, maxLifeTime);
//		explosionParticles.Play ();
	}


	private void OnTriggerEnter(Collider other)
	{

	}

	public void ToggleOnGoalLine(bool onLine) {
		Material currentMaterial = onLine ? ballGoalLineMaterial : ballMaterial;
		GetComponent<Renderer> ().material = currentMaterial;
	}

	public void Explode() {
		Collider[] colliders = Physics.OverlapSphere (transform.position, explosionRadius, playerMask);
		Debug.Log ("COLLIDERS FOUND: " + colliders.Length);
		for (int i = 0; i < colliders.Length; i++) {
			PlayerMove player = colliders [i].GetComponentInChildren<PlayerMove> ();
			if (!player)
				continue;
			player.ExplosiveForceAdded (explosionForce, transform.position, explosionRadius);
			//			EnemyController targetController = targetRb.GetComponent<EnemyController> ();
			//			if (!targetController)
			//				continue;
			//			float damage = CalculateDamage (targetRb.position);
			//			targetController.TakeDamage (damage);
		}

		explosionParticles.Play ();
		explosionAudio.Play ();
		Debug.Log ("DESTROYING");

		Destroy (explosionParticles.gameObject, explosionParticles.duration - 0.5f);
		gameObject.GetComponent<Renderer>().enabled = false;
		Destroy (gameObject, explosionParticles.duration);
	}
}
