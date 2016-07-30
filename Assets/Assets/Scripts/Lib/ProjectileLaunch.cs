using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProjectileLaunch : MonoBehaviour {
	    
//		public Rigidbody projectile;            
//		public Transform fireTransform;    
//		public Slider aimSlider;
//		public ProjectileTouchPad touchPad;
////		public AudioSource m_ShootingAudio;  
////		public AudioClip m_ChargingClip;     
////		public AudioClip m_FireClip;         
//		public float minLaunchForce = 15f;
//		public float maxLaunchForce = 50f;
//		public float maxChargeTime = 1.5f;
//
//		private string fireButton;         
//		private float currentLaunchForce;  
//		private float chargeSpeed;         
//		private bool launched;                
//
//
//		private void OnEnable()
//		{
//			currentLaunchForce = minLaunchForce;
//			aimSlider.value = minLaunchForce;
//		}
//
//
//		private void Start()
//		{
////			m_FireButton = "Fire" + m_PlayerNumber;
//			chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
//		}
//
//		private void Update()
//		{
//			// Track the current state of the fire button and make decisions based on the current launch force.
//			// You can set the min launch force twice in the same update cause by the end it will only be one thing
//			aimSlider.value = minLaunchForce;
//			if(Input.GetButtonDown(m_FireButton)) {
//				// pressed button for first time
//				launched = false;
//				m_CurrentLaunchForce = m_MinLaunchForce;
//				m_ShootingAudio.clip = m_ChargingClip;
//				m_ShootingAudio.Play ();
//			} else if (Input.GetButton(m_FireButton) && !m_Fired) {
//				//holding the button
//				m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
//				m_AimSlider.value = m_CurrentLaunchForce;
//			} else if (Input.GetButtonUp(m_FireButton) && !m_Fired) {
//				//releasing the button
//				Fire ();
//			}
//
//		}
//
//
//		private void Launch()
//		{
//			// Instantiate and launch the shell.
//			launched = true;
//			Rigidbody shellInstance = Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
//			shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;
//			m_ShootingAudio.clip = m_FireClip;
//			m_ShootingAudio.Play ();
//			m_CurrentLaunchForce = m_MinLaunchForce;
//		}
//	}
}
