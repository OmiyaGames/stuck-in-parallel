using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OmiyaGames;

namespace LudumDare40 {
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(PlayerCharacter))]
	[RequireComponent(typeof(Animator))]
	public class PlayerHealth : MonoBehaviour {
		[SerializeField]
		int maxHealth = 4;
		[SerializeField]
		float damageInvincibilitySeconds = 1.5f;
		[SerializeField]
		string spikesTag = "spikes";
		[SerializeField]
		LevelExit exit;

		[Header("Knockback")]
		[SerializeField]
		PlayerCharacter controls;
		[SerializeField]
		float knockbackStrength = 100f;
		[SerializeField]
		float knockbackDurationSeconds = 0.4f;

		[Header("Animations")]
		[SerializeField]
		string invincibilityField = "IsInvincible";
		[SerializeField]
		RectTransform healthBar;

		Rigidbody2D body;
		Animator animator;
		float damageStartTime = -1f;
		float knockbackStartTime = -1f;
		int currentHealth = 0;
		Vector2 healthBarMax = new Vector2 (1f, 1f);
		bool isDead = false;

		public bool IsInvincible {
			get {
				return ((damageStartTime > 0) && ((Time.time - damageStartTime) < damageInvincibilitySeconds));
			}
		}

		public bool IsKnockbacked {
			get {
				return ((knockbackStartTime > 0) && ((Time.time - knockbackStartTime) < knockbackDurationSeconds));
			}
		}

		public int CurrentHealth {
			get {
				return currentHealth;
			}
			set {
				currentHealth = Mathf.Clamp (value, 0, maxHealth);
				if ((isDead == false) && (currentHealth == 0)) {
					exit.RestartLevel();
				}
				healthBarMax.x = currentHealth;
				healthBarMax.x /= maxHealth;
				healthBar.anchorMax = healthBarMax;
			}
		}

		void Start() {
			currentHealth = maxHealth;
			body = GetComponent<Rigidbody2D> ();
			animator = GetComponent<Animator> ();
		}

		void OnCollisionEnter2D(Collision2D info) {
			if (info.collider.CompareTag (spikesTag) == true) {
				// Knock the player away from the spikes
				Knockback (info);
				knockbackStartTime = Time.time;

				// Check if the player is invincible
				if (IsInvincible == false) {
					// If not, decrease health
					CurrentHealth -= 1;

					// Indicate damage time
					damageStartTime = Time.time;
				}
			}
		}

		// Update is called once per frame
		void Update () {
			if (knockbackStartTime > 0) {
				if (IsKnockbacked == false) {
					controls.IsInControl = true;
					knockbackStartTime = -1f;
				}
			}
			if (damageStartTime > 0) {
				if (IsInvincible == true) {
					animator.SetBool (invincibilityField, true);
				} else {
					animator.SetBool (invincibilityField, false);
					damageStartTime = -1f;
				}
			}
		}

		void Knockback(Collision2D info) {
			if ((info.contacts != null) && (info.contacts.Length > 0)) {
				body.velocity = info.contacts [0].normal * knockbackStrength;
				controls.IsInControl = false;
			}
		}
	}
}
