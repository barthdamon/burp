using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	public PlayerMove playerOne;
	public PlayerMove playerTwo;
	public Transform ball;

	private Vector3 startPosition;

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
	}
	
	// Update is called once per frame
//	void Update () {
//
//	}


	void FixedUpdate() {
		float playerDistanceX = playerOne.transform.position.x - playerTwo.transform.position.x;
		float playerDistanceY = playerOne.transform.position.y - playerTwo.transform.position.y;
		Vector3 averagePosition = (playerOne.transform.position + playerTwo.transform.position) / 2;
		Vector3 distance = playerOne.transform.position - playerTwo.transform.position;
		ManipulateTime (distance);
		ZoomCamera (playerDistanceX, playerDistanceY, averagePosition);
		// if its less than certain distance, lerp the camera closer to average, otherwise lerp to startPosition
		// 2) if distance is past certain point begin slowing down time....
	}

	void ManipulateTime(Vector3 distance) {
		float magnitude = distance.magnitude;
		bool playersAlive = playerOne.playerManager.IsAlive () && playerTwo.playerManager.IsAlive ();
		if (magnitude < 4f && playersAlive) {
			Time.timeScale = 0.6f;
		} else {
			Time.timeScale = 1.0f;
		}
	}

	void ZoomCamera(float distanceX, float distanceY, Vector3 averagePosition) {
//		Vector3 trejectory = (averagePosition - transform.position).normalized;
//		trejectory.y = 0;
//		trejectory.z = 0;
//
//		transform.rotation = Quaternion.LookRotation (trejectory);
		float maxZoom = distanceX > distanceY ? distanceX : distanceY;
		averagePosition.z = maxZoom < -10 ? maxZoom + 3 : -10;
		Vector3 destination;
		if (maxZoom < 5) {
			destination = averagePosition;
		} else {
			destination = startPosition;
		}
//		transform.position = Mathf.SmoothDamp(transform.position, destination, 10.0f, Time.deltaTime);
		Vector3 pos = transform.position;
		pos.y = Mathf.Clamp(transform.position.y, 5, 0);
		transform.position = Vector3.Lerp (pos, destination, Time.deltaTime);
	}
}
