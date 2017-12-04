using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare40 {
	[RequireComponent(typeof(Animator))]
	public class LanternIndicator : MonoBehaviour {

		static LanternIndicator instance;

		[SerializeField]
		Lantern focus = null;
		[SerializeField]
		float smooth = 8;

		public static Lantern Focus {
			get {
				return instance.focus;
			}
			set {
				if (PlayerInventory.HoldingLantern == null) {
					instance.UpdateIndicator (instance.focus, value);
					instance.focus = value;
				}
			}
		}

		// Use this for initialization
		void Awake () {
			instance = this;

			// FIXME: provide actual animation
			gameObject.SetActive (false);
		}
		
		void UpdateIndicator (Lantern lastFocus, Lantern newFocus) {
			if (newFocus != null) {
				if (lastFocus == null) {
					// FIXME: provide actual animation
					gameObject.SetActive (true);
					transform.position = newFocus.transform.position;
				} else {
					// FIXME: play a new animation
				}
			} else {
				// FIXME: provide actual animation
				gameObject.SetActive(false);
			}
		}

		void Update() {
			if (focus != null) {
				transform.position = Vector3.Lerp (transform.position, focus.transform.position, (smooth * Time.deltaTime));
			}
		}
	}
}