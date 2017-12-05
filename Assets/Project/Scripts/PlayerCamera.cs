using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace LudumDare40 {
	[RequireComponent(typeof(Animator))]
	public class PlayerCamera : MonoBehaviour {
		private enum AheadDirection {
			None,
			Positive,
			Negative
		}

		static PlayerCamera instance;

		[SerializeField]
		Vector2 margin = new Vector3(2f, 2f);
		[SerializeField]
		Vector2 lookAhead = new Vector3(1f, 1f);
		[SerializeField]
		Vector2 smooth = new Vector3(8f, 8f);

		[SerializeField]
		PlayerCharacter player; // Reference to the player's transform.
		[SerializeField]
		string damageAnimation = "PlayDamage";
		[SerializeField]
		string victoryAnimation = "PlayVictory";

		AheadDirection xDirection = AheadDirection.None;
		AheadDirection yDirection = AheadDirection.None;
		Animator animator = null;

		void Start() {
			instance = this;
			animator = GetComponent<Animator> ();
		}

		public static void PlayDamageAnimation() {
			instance.animator.SetTrigger (instance.damageAnimation);
		}

		public static void PlayVictoryAnimation() {
			instance.animator.SetTrigger (instance.victoryAnimation);
		}

		void CheckXMargin(ref float newXPosition) {
			// Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
			CalculatePosition (ref newXPosition, ref xDirection,
				player.Body.velocity.x, player.Body.position.x, transform.position.x, margin.x, lookAhead.x);
		}

		void CheckYMargin(ref float newYPosition) {
			// Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
			CalculatePosition (ref newYPosition, ref yDirection,
				player.Body.velocity.y, player.Body.position.y, transform.position.y, margin.y, lookAhead.y);
		}

		static void CalculatePosition (ref float newXPosition, ref AheadDirection direction,
				float playerVelocity, float playerPosition, float cameraPosition, float margin, float lookAhead) {
			if ((direction == AheadDirection.Positive) && (playerVelocity > 0)) {
				newXPosition = playerPosition + lookAhead;
			} else if ((direction == AheadDirection.Negative) && (playerVelocity < 0)) {
				newXPosition = playerPosition - lookAhead;
			} else {
				direction = AheadDirection.None;
				if (Mathf.Abs (cameraPosition - playerPosition) > margin) {
					direction = AheadDirection.Negative;
					newXPosition = playerPosition - lookAhead;
					if (playerVelocity > 0) {
						direction = AheadDirection.Positive;
						newXPosition = playerPosition + lookAhead;
					}
				}
			}
		}

		void OnDrawGizmos() {
			if (player != null) {
				Vector3 size = Vector3.zero;
				size.x = margin.x * 2;
				size.y = margin.y * 2;
				Gizmos.color = Color.white;
				Gizmos.DrawWireCube (player.transform.position, size);
			}
		}

		void FixedUpdate() {
			TrackPlayer ();
		}

		void TrackPlayer()
		{
			// By default the target x and y coordinates of the camera are it's current x and y coordinates.
			float targetX = transform.position.x;
			float targetY = transform.position.y;

			// If the player has moved beyond the x margin...
			CheckXMargin(ref targetX);
			// ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
			targetX = Mathf.Lerp(transform.position.x, targetX, (smooth.x * Time.deltaTime));

			// If the player has moved beyond the y margin...
			CheckYMargin(ref targetY);
			// ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
			targetY = Mathf.Lerp(transform.position.y, targetY, (smooth.y * Time.deltaTime));

			// Set the camera's position to the target position with the same z component.
			transform.position = new Vector3(targetX, targetY, transform.position.z);
		}
	}
}

