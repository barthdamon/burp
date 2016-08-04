using UnityEngine;
using System.Collections;

public class BallMove : MonoBehaviour {

	public BoxCollider floorCollider;
	private Rigidbody rb;
	private float ShootingLength = 2f;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
//		float floorMax = floorCollider.size.x;
		Vector3 pos = transform.position;
		pos.z = Mathf.Clamp (transform.position.z, 0, 0);
//		pos.x = Mathf.Clamp (transform.position.x, -floorMax, floorMax);
//		pos.y = Mathf.Clamp (transform.position.y, 0, 7);
		transform.position = pos;

		if (transform.position.y < -5) {
			// Set y to zero when its below -5
			rb.AddForce(new Vector3(0f,2f,0f));
//			Vector3 CurrentVelocity = rb.velocity;
//			float CurrentY = rb.velocity.y;
//			if (CurrentVelocity.y < 0) {
//				CurrentVelocity.y += 0.1f;
//			}
//			float NewY = Mathf.Lerp (CurrentY, CurrentVelocity.y, 1.0f);
//			rb.velocity.y = NewY;
		}
	}

	public Vector3 GetFuturePositionFromDistance(float Distance, float Speed)
	{
		// take the distance and figure out how 
		Vector3 CurrentPos = transform.position;
		Vector3 Velocity = GetComponent<Rigidbody> ().velocity;
		float TimeToReach = Distance / Speed;
		Vector3 WhereToGo = CurrentPos + (Velocity * TimeToReach);
		return WhereToGo;

	}

	public ShootingCircle[] GetShootingCircles(Vector3 ShootingVector)
	{
		// use these when the balls x velocity is low

		// calculate the center points


		ShootingCircle TopCircle = new ShootingCircle (Vector3.zero, 2f);
		ShootingCircle BottomCircle = new ShootingCircle (Vector3.zero, 2f);
		return new ShootingCircle[] { TopCircle, BottomCircle };
	}
		
}


[System.Serializable]
public struct ShootingCircle {
	public Vector3 CenterPoint;
	public float Radius;
	public float Distance;

	public ShootingCircle(Vector3 CenterPoint, float Distance)
	{
		this.CenterPoint = CenterPoint;
		this.Radius = 0.5f;
		this.Distance = Distance;
	}
}
