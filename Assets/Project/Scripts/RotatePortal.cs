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

//		[Header("In Inventory")]
//		[SerializeField]
//		float diffMultiplier = 100f;

		Rigidbody2D body;
		float rotateSpeed;
		Vector3 rotateDirection;
//		Vector2 lastPosition;

		// Use this for initialization
		void Start () {
			body = GetComponent<Rigidbody2D> ();
			spinObject.gameObject.SetActive (true);
		}
		
		// Update is called once per frame
		void Update () {
			float x = body.velocity.x;
			float y = body.velocity.y;
//			if (PlayerInventory.HoldingLantern == this) {
//				x = (body.position.x - lastPosition.x) * diffMultiplier;
//				y = (body.position.y - lastPosition.y) * diffMultiplier;
//			}

			// Spin the object
			rotateSpeed = Mathf.Clamp((x * -velocityToSpin * Time.deltaTime), -maxSpin, maxSpin);
			spinObject.RotateAround (spinObject.position, Vector3.up, rotateSpeed);
			rotateSpeed = Mathf.Clamp((y * velocityToSpin * Time.deltaTime), -maxSpin, maxSpin);
			spinObject.RotateAround (spinObject.position, Vector3.right, rotateSpeed);

			// Update last position
//			lastPosition.x = body.position.x;
//			lastPosition.y = body.position.y;
		}
	}
}