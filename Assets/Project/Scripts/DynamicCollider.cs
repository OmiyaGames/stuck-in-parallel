using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare40 {
	[RequireComponent(typeof(Collider2D))]
	public class DynamicCollider : MonoBehaviour {
		static readonly Dictionary<Collider, DynamicCollider> allColliders = new Dictionary<Collider, DynamicCollider>();
		static readonly Dictionary<Universe, int> allPlayerLayers = new Dictionary<Universe, int>();

		[SerializeField]
		Lantern parentLantern;
		[SerializeField]
		Collider collider3D = null;

		[Header("Player Layers")]
		[SerializeField]
		string playerDefaultLayer = "Player Default";
		[SerializeField]
		string playerGreenLayer = "Player Green";
		[SerializeField]
		string playerPurpleTag = "Player Purple";
		[SerializeField]
		string playerOrangeTag = "Player Orange";

		Collider2D changeLayer;
		readonly HashSet<Lantern> enteredZones = new HashSet<Lantern>();

		public static DynamicCollider GetScript(Collider collider) {
			DynamicCollider returnScript = null;
			if (allColliders.ContainsKey (collider) == true) {
				returnScript = allColliders [collider];
			}
			return returnScript;
		}

		void Awake() {
			allColliders.Clear ();
		}

		void Start() {
			allColliders.Add (collider3D, this);
			if (allPlayerLayers.Count <= 0) {
				allPlayerLayers.Add (Universe.Default, LayerMask.NameToLayer (playerDefaultLayer));
				allPlayerLayers.Add (Universe.Green, LayerMask.NameToLayer (playerGreenLayer));
				allPlayerLayers.Add (Universe.Purple, LayerMask.NameToLayer (playerPurpleTag));
				allPlayerLayers.Add (Universe.Orange, LayerMask.NameToLayer (playerOrangeTag));
			}
		}

		public void OnEnterLantern(Lantern lantern) {
			if (enteredZones.Contains (lantern) == false) {
				enteredZones.Add (lantern);
				UpdateLayer ();
			}
		}

		public void OnExitLantern(Lantern lantern) {
			if (enteredZones.Contains (lantern) == true) {
				enteredZones.Remove (lantern);
				UpdateLayer ();
			}
		}

		void UpdateLayer() {
			// Retrieve the default universe
			Universe currentUniverse = Universe.Default;
			if (parentLantern != null) {
				currentUniverse = parentLantern.Universe;
			} else if (PlayerInventory.HoldingLantern != null) {
				currentUniverse = PlayerInventory.HoldingLantern.Universe;
			}

			// Go through all the zones we're in
			foreach(Lantern lantern in enteredZones) {
				if (((int)currentUniverse) < ((int)lantern.Universe)) {
					currentUniverse = lantern.Universe;
				}
			}

			// Update the player collider's layer
			gameObject.layer = allPlayerLayers [currentUniverse];
		}
	}
}