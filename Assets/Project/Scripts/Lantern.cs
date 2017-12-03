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
		Universe universe;

		public Universe lastUnivese;

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

		// Use this for initialization
		void Start () {
			PlayerCollider.AddLantern (this);
			lastUnivese = universe;
		}
	}
}