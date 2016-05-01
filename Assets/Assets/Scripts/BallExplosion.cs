using UnityEngine;
using System.Collections;
using System.Resources;

public class BallExplosion : MonoBehaviour {
	public LayerMask playerMask;
	public ParticleSystem explosionParticles;       
	public AudioSource explosionAudio;                              
	public Material ballGoalLineMaterial;
	public Material ballMaterial;

	private float explosionRadius = 20f;
	private float explosionForce = 200f;
	private float goalDepth = 3;


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
			PlayerExplosiveRecognizer explosiveForceRecognizer = colliders [i].GetComponent<PlayerExplosiveRecognizer> ();
			if (explosiveForceRecognizer) {
				Debug.Log ("Player found");
				// hard coded to back of the goal...
				float xForce = transform.position.x < 0 ? -23 : 23;
				Vector3 forcePosition = new Vector3(xForce, 0,0);
				explosiveForceRecognizer.player.ExplosiveForceAdded (explosionForce, forcePosition, 0);
			}
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
