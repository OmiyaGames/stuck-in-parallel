using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare40 {
	[RequireComponent(typeof(Collider2D))]
	public class LanternTrigger : MonoBehaviour {
		[SerializeField]
		string playerTag = "Player";
		[SerializeField]
		Lantern parent;

		void OnTriggerEnter2D(Collider2D other) {
			if (other.CompareTag (playerTag) == true) {
				LanternIndicator.Focus = parent;
			}
		}

		void OnTriggerExit2D(Collider2D other) {
			if ((other.CompareTag (playerTag) == true) && (LanternIndicator.Focus == parent)) {
				LanternIndicator.Focus = null;
			}
		}
	}
}
