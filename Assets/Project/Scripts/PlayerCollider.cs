using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare40 {
	[RequireComponent(typeof(Collider2D))]
	public class PlayerCollider : MonoBehaviour {
		static PlayerCollider instance;

		[SerializeField]
		[UnityEngine.Serialization.FormerlySerializedAs("camera")]
		Transform cameraPosition;
		[SerializeField]
		Rigidbody2D pauseSimulation;

		[Header("Lantern Layers")]
		[SerializeField]
		LayerMask collisionLayers;
		[SerializeField]
		LayerMask greenLanternLayers;
		[SerializeField]
		LayerMask purpleLanternLayers;

		[Header("Player Layers")]
		[SerializeField]
		string playerDefaultLayer = "Default";
		[SerializeField]
		string playerGreenLayer = "Green";
		[SerializeField]
		string playerPurpleTag = "Purple";
		[SerializeField]
		string playerOrangeTag = "Orange";

		Ray ray = new Ray();
		RaycastHit[] info = null;
		Universe lastUniverse = Universe.Default;

		readonly Dictionary<Collider, Lantern> allLanterns = new Dictionary<Collider, Lantern>();
		static readonly Dictionary<Universe, int> allPlayerLayers = new Dictionary<Universe, int>();
		static readonly Dictionary<Universe, LayerMask> allLanternLayers = new Dictionary<Universe, LayerMask>();

		public static void AddLantern(Lantern lantern) {
			if ((lantern != null) && (instance != null)) {
				instance.allLanterns.Add (lantern.Collider3d, lantern);
			}
		}

		void Awake() {
			instance = this;
			if (allPlayerLayers.Count <= 0) {
				allPlayerLayers.Add (Universe.Default, LayerMask.NameToLayer (playerDefaultLayer));
				allPlayerLayers.Add (Universe.Green, LayerMask.NameToLayer (playerGreenLayer));
				allPlayerLayers.Add (Universe.Purple, LayerMask.NameToLayer (playerPurpleTag));
				allPlayerLayers.Add (Universe.Orange, LayerMask.NameToLayer (playerOrangeTag));
			}
			if (allLanternLayers.Count <= 0) {
				allLanternLayers.Add (Universe.Green, greenLanternLayers);
				allLanternLayers.Add (Universe.Purple, purpleLanternLayers);
			}
		}

		void Update () {
			// Setup array, if not already
			if (info == null) {
				info = new RaycastHit[instance.allLanterns.Count];
			}

			// Setup player layers
			Universe defaultUniverse = Universe.Default;
			if (PlayerInventory.HoldingLantern != null) {
				defaultUniverse = PlayerInventory.HoldingLantern.Universe;
			}
			UpdateLayers (gameObject, collisionLayers, defaultUniverse, ref lastUniverse);

			// Setup lantern layers
			foreach (Lantern lantern in allLanterns.Values) {
				if (lantern.Universe != Universe.Orange) {
					UpdateLayers (lantern.Collider2d.gameObject, allLanternLayers[lantern.Universe], lantern.Universe, ref lantern.lastUnivese);
				}
			}
		}

		void UpdateLayers (GameObject updateObject, LayerMask layers, Universe defaultUniverse, ref Universe lastUniverse)
		{
			// Setup the ray
			Vector3 diff = updateObject.transform.position - cameraPosition.position;
			ray.origin = cameraPosition.position;
			ray.direction = diff;

			// Raycast for any 3D colliders
			int numHits = Physics.RaycastNonAlloc (ray, info, diff.sqrMagnitude, layers.value, QueryTriggerInteraction.Collide);

			// Discover the highest priority layer the player should be in
			Universe currentUniverse = defaultUniverse;
			Lantern currentLantern;
			for (int index = 0; index < numHits; ++index) {
				if (allLanterns.TryGetValue (info [index].collider, out currentLantern) == true) {
					if (((int)currentUniverse) < ((int)currentLantern.Universe)) {
						currentUniverse = currentLantern.Universe;
					}
				}
			}

			// Update the player collider's layer
			updateObject.layer = allPlayerLayers [currentUniverse];
			// Check if this is different from the last universe
			if (lastUniverse != currentUniverse) {

				// Update last universe
				lastUniverse = currentUniverse;
			}
		}
	}
}
