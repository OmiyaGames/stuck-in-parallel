using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare40 {
	public enum Universe {
		Default,
		Green,
		Purple,
		Orange
	}

	public class Lantern : MonoBehaviour {
		[SerializeField]
		Collider collider3d;
		[SerializeField]
		Collider2D collider2d;
		[SerializeField]
		Collider2D trigger2d;
		[SerializeField]
		Rigidbody2D body;
		[SerializeField]
		Universe universe;

		public Universe lastUnivese;
		bool inInventory = false;

		public Collider Collider3d {
			get {
				return collider3d;
			}
		}

		public Collider2D Collider2d {
			get {
				return collider2d;
			}
		}

		public Universe Universe {
			get {
				return universe;
			}
		}

		public bool IsInInventory {
			get {
				return inInventory;
			}
			set {
				if (inInventory != value) {
					inInventory = value;
					collider2d.enabled = !inInventory;
					trigger2d.enabled = !inInventory;
					if (inInventory == true) {
						body.bodyType = RigidbodyType2D.Static;
					} else {
						body.bodyType = RigidbodyType2D.Dynamic;
					}
				}
			}
		}

		// Use this for initialization
		void Start () {
			PlayerCollider.AddLantern (this);
			lastUnivese = universe;
		}
	}
}