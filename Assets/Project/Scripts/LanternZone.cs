using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare40 {
	[RequireComponent(typeof(Collider))]
	public class LanternZone : MonoBehaviour {
		[SerializeField]
		Lantern lantern;

		void OnTriggerEnter(Collider other) {
			Debug.Log (other.name);
			DynamicCollider collider = DynamicCollider.GetScript (other);
			if (collider != null) {
				collider.OnEnterLantern (lantern);
			}
		}

		void OnTriggerExit(Collider other) {
			DynamicCollider collider = DynamicCollider.GetScript (other);
			if (collider != null) {
				collider.OnExitLantern (lantern);
			}
		}
	}
}