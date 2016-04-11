using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerDash : MonoBehaviour {
	public int maxDash = 50;
	public Text dashDisplay;

	public bool isDashing {
		get {
			return canDash() && Input.GetKey(KeyCode.Space);
		}
	}

	private int dashRemaining;
	private bool dashRecharging = false;

	void Start() {
		dashRemaining = maxDash;
	}

	void FixedUpdate() {
		if (isDashing) {
			dashRecharging = false;
			dashRemaining--;
		} else if (!dashRecharging && canRecharge()) {
			dashRecharging = true;
		} else if (dashRecharging && dashRemaining < maxDash) {
			dashRemaining++;

			if (dashRemaining == maxDash) {
				dashRecharging = false;
			}
		}
	}

	void Update() {
		dashDisplay.text = "Dash: " + dashRemaining.ToString();
	}

	private bool canDash() {
		return !dashRecharging && dashRemaining > 0;
	}

	private bool canRecharge() {
		return !canDash() || (!isDashing && dashRemaining < maxDash);
	}
}
