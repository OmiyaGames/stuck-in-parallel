using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OmiyaGames;
using OmiyaGames.Menu;

namespace LudumDare40 {
	[RequireComponent(typeof(Collider2D))]
	public class TutorialZone : MonoBehaviour {

		[SerializeField]
		string playerTag = "Player";
		[SerializeField]
		[Multiline]
		string message = "Tutorial!";
		[SerializeField]
		[Tooltip("Maximum number of times tutorial appears on the screen.  0 or less for infinite.")]
		int numRepeat = 0;
		[SerializeField]
		[Tooltip("Only show tutorial if holding a specific item.  Default for no check.")]
		Universe isHolding = Universe.Default;
		[SerializeField]
		SoundEffect showTutorial = null;

		Collider2D cacheCollider = null;
		ulong popUpId = PopUpManager.InvalidId;
		int numTimesEntered = 0;

		public Collider2D CacheCollider {
			get {
				if (cacheCollider == null) {
					cacheCollider = GetComponent<Collider2D> ();
				}
				return cacheCollider;
			}
		}

		public bool CanShowTutorial {
			get {
				bool returnFlag = true;
				if ((numRepeat > 0) && (numTimesEntered >= numRepeat)) {
					returnFlag = false;
				}
				if (isHolding != Universe.Default) {
					if (PlayerInventory.HoldingLantern == null) {
						returnFlag = false;
					} else if (PlayerInventory.HoldingLantern.Universe != isHolding) {
						returnFlag = false;
					}
				}
				return returnFlag;
			}
		}

		PopUpManager PopUps {
			get {
				return Singleton.Get<MenuManager> ().PopUps;
			}
		}

		void OnDrawGizmos() {
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(CacheCollider.bounds.center, CacheCollider.bounds.size);
		}

		void OnTriggerEnter2D(Collider2D other) {
			if ((other.CompareTag (playerTag) == true) && (CanShowTutorial == true)) {
				ShowPopUp ();
			}
		}

		void OnTriggerExit2D(Collider2D other) {
			if (other.CompareTag (playerTag) == true) {
				HidePopUp();
			}
		}

		void ShowPopUp() {
			HidePopUp ();
			popUpId = PopUps.ShowNewDialog (message);
			showTutorial.Play ();
			++numTimesEntered;
		}

		void HidePopUp() {
			if (popUpId != PopUpManager.InvalidId) {
				PopUps.RemoveDialog (popUpId);
				popUpId = PopUpManager.InvalidId;
			}
		}
	}
}
