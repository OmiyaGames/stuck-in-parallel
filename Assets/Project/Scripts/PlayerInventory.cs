using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace LudumDare40 {
	[RequireComponent(typeof(Animator))]
	public class PlayerInventory : MonoBehaviour {
		static PlayerInventory instance;

		[SerializeField]
		Lantern holdingLantern;

		// FIXME: start using these once we know the direction the player is facing
		[Header("Player")]
		[SerializeField]
		PlayerCharacter player;
		[SerializeField]
		Transform dropLeft;
		[SerializeField]
		Transform dropRight;
		[SerializeField]
		Transform dropTop;
		[SerializeField]
		Transform dropBottom;

		public static Lantern HoldingLantern {
			get {
				return instance.holdingLantern;
			}
		}

		void Awake() {
			instance = this;
		}

		void Update() {
			if (holdingLantern == null) {
				if ((LanternIndicator.Focus != null) && (CrossPlatformInputManager.GetButtonUp ("Fire1") == true)) {
					// Remove the lantern from the lantern indicator
					Lantern lantern = LanternIndicator.Focus;
					LanternIndicator.Focus = null;

					// Remove the lantern physics
					lantern.IsInInventory = true;

					// Attach the lantern to the inventory
					holdingLantern = lantern;
				}
			} else if (CrossPlatformInputManager.GetButtonUp ("Fire1") == true) {
				// Figure out the location to drop the lantern
				Transform dropPosition = dropBottom;
				switch (player.Facing) {
				case PlayerCharacter.Direction.Up:
					dropPosition = dropTop;
					break;
				case PlayerCharacter.Direction.Left:
					dropPosition = dropLeft;
					break;
				case PlayerCharacter.Direction.Right:
					dropPosition = dropRight;
					break;
				}

				// Move the lantern to the drop position
				holdingLantern.transform.position = dropPosition.position;

				// Re-enable the lantern physics
				holdingLantern.IsInInventory = false;

				// Remove the lantern from the inventory
				holdingLantern = null;
			} else {
				// Have the lantern follow the player
				holdingLantern.transform.position = transform.position;
			}
		}
	}
}