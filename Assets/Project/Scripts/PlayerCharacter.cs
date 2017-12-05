using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using OmiyaGames;

namespace LudumDare40 {
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(Sprite))]
	[RequireComponent(typeof(Rigidbody2D))]
	public class PlayerCharacter : MonoBehaviour {
		const string LastDirection = "LastDirection";
		const string Horizontal = "Horizontal";
		const string Vertical = "Vertical";
		const string Velocity = "Velocity";

		public enum Direction {
			Down,
			Up,
			Left,
			Right
		}

		[SerializeField]
		float movementSpeed;
		[SerializeField]
		SoundEffect footsteps;
		[SerializeField]
		bool isInControl = true;

		Vector2 controls = Vector2.zero;
		Vector2 move = Vector2.zero;
		Rigidbody2D body = null;
		Animator animator = null;
		Direction facing = Direction.Down;

		public Rigidbody2D Body {
			get {
				return body;
			}
		}

		public Direction Facing {
			get {
				return facing;
			}
		}

		public bool IsInControl {
			get {
				return isInControl;
			}
			set {
				isInControl = value;
			}
		}

		void Start() {
			body = GetComponent<Rigidbody2D> ();
			animator = GetComponent<Animator> ();
		}

		// Update is called once per frame
		void Update () {
			controls.x = 0;
			controls.y = 0;
			if (IsInControl == true) {
				// Get the controls
				controls.x = CrossPlatformInputManager.GetAxis ("Horizontal");
				controls.y = CrossPlatformInputManager.GetAxis ("Vertical");

				// Normalize directions (diagonals are the same speed as normal controls)
				if ((Mathf.Approximately (controls.x, 0) == false) || (Mathf.Approximately (controls.y, 0) == false)) {
					controls.Normalize ();
				}
			}

			// Update animation
			animator.SetFloat (Velocity, controls.sqrMagnitude);
			animator.SetFloat (Horizontal, controls.x);
			animator.SetFloat (Vertical, controls.y);
			if ((Mathf.Abs (controls.x) > 0.1f) || (Mathf.Abs(controls.y) > 0.1f)) {
				if (Mathf.Abs (controls.x) > Mathf.Abs (controls.y)) {
					facing = Direction.Left;
					if (controls.x > 0) {
						facing = Direction.Right;
					}
				} else {
					facing = Direction.Down;
					if (controls.y > 0) {
						facing = Direction.Up;
					}
				}
			}
			animator.SetInteger (LastDirection, ((int)facing));
		}

		void FixedUpdate() {
			// Multiply by movement speed
			move.x = controls.x * movementSpeed * Time.deltaTime;
			move.y = controls.y * movementSpeed * Time.deltaTime;

			// Apply force on the character
			if (move.sqrMagnitude > 0.1f) {
				body.velocity = move;
			}
		}

		public void PlayFootSteps() {
			footsteps.Play ();
		}
	}
}
