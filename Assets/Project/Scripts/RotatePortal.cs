using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare40 {
	[RequireComponent(typeof(Rigidbody2D))]
	public class RotatePortal : MonoBehaviour {
		[SerializeField]
		float velocityToSpin = 0.1f;
		[SerializeField]
		float maxSpin = 1f;
		[SerializeField]
		Transform spinObject;

		Rigidbody2D body;
		float rotateSpeed;
		Vector3 rotateDirection;

		// Use this for initialization
		void Start () {
			body = GetComponent<Rigidbody2D> ();
		}
		
		// Update is called once per frame
		void Update () {
			rotateSpeed = Mathf.Clamp((body.velocity.x * -velocityToSpin * Time.deltaTime), -maxSpin, maxSpin);
			spinObject.RotateAround (spinObject.position, Vector3.up, rotateSpeed);
			rotateSpeed = Mathf.Clamp((body.velocity.y * velocityToSpin * Time.deltaTime), -maxSpin, maxSpin);
			spinObject.RotateAround (spinObject.position, Vector3.right, rotateSpeed);
		}
	}
}