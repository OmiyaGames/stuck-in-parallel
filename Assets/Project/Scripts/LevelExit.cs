using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OmiyaGames;

namespace LudumDare40 {
	[RequireComponent(typeof(Collider2D))]
	public class LevelExit : MonoBehaviour {
		[SerializeField]
		PlayerCharacter controls;

		bool isTransitioning = false;

		public void RestartLevel() {
			if (isTransitioning == false) {
				Singleton.Get<SceneTransitionManager> ().ReloadCurrentScene ();
				controls.IsInControl = false;
				isTransitioning = true;
			}
		}

		// Update is called once per frame
		void OnTriggerEnter2D (Collider2D other) {
			if ((other.CompareTag("Player") == true) && (isTransitioning == false)) {
				Singleton.Get<SceneTransitionManager> ().LoadNextLevel ();
				controls.IsInControl = false;
				isTransitioning = true;
			}
		}
	}
}
