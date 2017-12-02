using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace LudumDare40 {
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(Sprite))]
	[RequireComponent(typeof(Rigidbody2D))]
	public class PlayerCharacter : MonoBehaviour {

		[SerializeField]
		float movementSpeed;

		Vector2 controls = Vector2.zero;
		Vector2 move = Vector2.zero;
		Rigidbody2D body = null;

		public Rigidbody2D Body {
			get {
				return body;
			}
		}

		void Start() {
			body = GetComponent<Rigidbody2D> ();
		}

		// Update is called once per frame
		void Update () {
			// Get the controls
			controls.x = CrossPlatformInputManager.GetAxis("Horizontal");
			controls.y = CrossPlatformInputManager.GetAxis("Vertical");

			// Normalize directions (diagonals are the same speed as normal controls)
			if ((Mathf.Approximately (controls.x, 0) == false) || (Mathf.Approximately (controls.y, 0) == false)) {
				controls.Normalize ();
			}
			//Debug.Log (controls);
		}

		void FixedUpdate() {
			// Multiply by movement speed
			move.x = controls.x * movementSpeed * Time.deltaTime;
			move.y = controls.y * movementSpeed * Time.deltaTime;

			// Apply force on the character
			if (move.sqrMagnitude > 0) {
				body.velocity = move;
			}
		}
	}
}
