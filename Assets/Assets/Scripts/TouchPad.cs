using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ProjectileTouchPad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {

	public float smoothing;

	private Vector2 origin;
	private Vector2 direction;
	private Vector2 smoothDirection;
	private bool touched;
	private int pointerId;

	void Awake() {
		direction = Vector2.zero;
		touched = false;
	}

	public void OnPointerDown(PointerEventData data) {
		// Set our start point
		if (!touched) {
			touched = true;
			pointerId = data.pointerId;
			origin = data.position;
		}
	}

	public void OnDrag(PointerEventData data) {
		if (data.pointerId == pointerId) {
			// Compare the difference from start point
			Vector2 currentPosition = data.position;
			Vector2 directionRaw = currentPosition - origin;
			direction = directionRaw.normalized;
		}
	}

	public void OnPointerUp(PointerEventData data) {
		// Reset Everything
		if (data.pointerId == pointerId) {
			direction = Vector2.zero;
			touched = false;
		}
	}

	public Vector2 GetDirection() {
		smoothDirection = Vector2.MoveTowards (smoothDirection, direction, smoothing);
		return smoothDirection;
	}

}