using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour {
	[Header("Block Shake")]
	public float magnitudeBlock = 1.0f;
	public float intensityBlock = 1.0f;
	public float timeBlock = 0.2f;

	[Header("Goal Shake")]
	public float magnitudeGoal = 1.0f;
	public float intensityGoal = 1.0f;
	public float timeGoal = 0.5f;

	[Header("Bounce Shake")]
	public float magnitudeBounce = 1.0f;
	public float intensityBounce = 1.0f;
	public float timeBounce = 0.2f;

	[Header("Special Shake")]
	public float magnitudeSpecial = 1.0f;
	public float intensitySpecial = 1.0f;
	public float timeSpecial = 0.2f;

	[Header("Buff Shake")]
	public float magnitudeBuff = 1.0f;
	public float intensityBuff = 1.0f;
	public float timeBuff = 0.2f;


	private int mode = -1;

	void Update() {
		switch (mode) {
			case 0:
				this.BlockShake (); break;
			case 1:
				this.GoalShake (); break;
			case 2:
				this.BounceShake (); break;
			case 3:
				this.SpecialShake (); break;
			case 4:
				this.BuffShake (); break;
			default:
				break;
		}
	}


	public void BlockScreenShake(int p) {
		StartCoroutine(this.ShakeScreen(0, timeBlock));
	}

	public void GoalScreenShake() {
		StartCoroutine (this.ShakeScreen (1, timeGoal));
	}

	public void BounceScreenShake() {
		StartCoroutine (this.ShakeScreen (2, timeBounce));
	}

	public void SpecialScreenShake() {
		StartCoroutine (this.ShakeScreen (3, timeSpecial));
	}

	public void BuffScreenShake() {
		StartCoroutine (this.ShakeScreen (4, timeBuff));
	}


	private IEnumerator ShakeScreen(int m, float t) {
		mode = m;

		yield return new WaitForSeconds (t);

		mode = -1;
	}

	private void BlockShake() {
		float heightx = this.PerlinNoise (magnitudeBlock, intensityBlock);

		this.transform.position = new Vector3(heightx, this.transform.position.y, this.transform.position.z);
	}

	private void GoalShake() {
		float heightx = this.PerlinNoise (magnitudeGoal, intensityGoal, 0.0f);
		float heighty = this.PerlinNoise (magnitudeGoal, intensityGoal, 1.0f);

		this.transform.position = new Vector3(heightx, heighty, this.transform.position.z);
	}

	private void BounceShake() {
		float heightx = this.PerlinNoise (magnitudeBounce, intensityBounce, 0.0f);
		float heighty = this.PerlinNoise (magnitudeBounce, intensityBounce, 1.0f);

		this.transform.position = new Vector3(heightx, heighty, this.transform.position.z);
	}

	private void SpecialShake() {
		float heightx = this.PerlinNoise (magnitudeSpecial, intensitySpecial, 0.0f);
		float heighty = this.PerlinNoise (magnitudeSpecial, intensitySpecial, 1.0f);

		this.transform.position = new Vector3(heightx, heighty, this.transform.position.z);
	}

	private void BuffShake() {
		float heightx = this.PerlinNoise (magnitudeBuff, intensityBuff, 0.0f);
		float heighty = this.PerlinNoise (magnitudeBuff, intensityBuff, 1.0f);

		this.transform.position = new Vector3(heightx, heighty, this.transform.position.z);
	}

	private float PerlinNoise(float mag, float intens, float y = 0.0f) {
		return mag * Mathf.PerlinNoise (Time.time * intens, y);
	}
}
