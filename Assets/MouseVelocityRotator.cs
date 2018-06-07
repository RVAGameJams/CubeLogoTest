using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseVelocityRotator : MonoBehaviour {

	public float VelocityMultiplier = 2f;
	Vector3 prevMouse;
	bool mouseTouching;

	Quaternion originalRotation;
	[Range(0, 2f)]
	public float DelayBeforeReset = 0.5f;
	float timeToReset;
	[Range(0, 0.25f)]
	public float ResetSpeed = 0.1f;

	Rigidbody rb;

	void Start() {
		rb = GetComponent<Rigidbody>();
		originalRotation = rb.rotation;
	}
	
	void FixedUpdate() {
		var mouseDelta = Input.mousePosition - prevMouse;
		if (mouseTouching) {
			rb.AddTorque(new Vector3(mouseDelta.y, -mouseDelta.x, 0), ForceMode.Force);
			if (mouseDelta.magnitude < 0.001f) {
				rb.angularVelocity = Vector3.zero;
			}
		} else {
			if (timeToReset > 0) {
				timeToReset -= Time.deltaTime;
			}
			if (timeToReset <= 0) {
				rb.angularVelocity = Vector3.zero;
				rb.rotation = Quaternion.Slerp(rb.rotation, originalRotation, ResetSpeed);
			}
		}
		prevMouse = Input.mousePosition;
	}

	void OnMouseEnter() {
		mouseTouching = true;
		rb.angularVelocity = Vector3.zero;
	}

	void OnMouseExit() {
		mouseTouching = false;
		timeToReset = DelayBeforeReset;
	}
}
