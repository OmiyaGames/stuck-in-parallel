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
		[SerializeField]
		Transform dropLeft;
		[SerializeField]
		Lantern dropRight;
		[SerializeField]
		Lantern dropTop;
		[SerializeField]
		Lantern dropBottom;

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
				if ((LanternIndicator.Focus != null) && (CrossPlatformInputManager.GetButton ("Fire1") == true)) {
					// Remove the lantern from the lantern indicator
					Lantern lantern = LanternIndicator.Focus;
					LanternIndicator.Focus = null;

					// Remove the lantern physics
					lantern.IsInInventory = true;

					// Attach the lantern to the inventory
					holdingLantern = lantern;
				}
			} else {
				// Have the lantern follow the player
				holdingLantern.transform.position = transform.position;
			}
		}
	}
}