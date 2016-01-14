using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartScreen : UserInterface {
	public GameObject startScreen;

	public float textPulseDuration = 1.0f;
	

	private UserInterface userInterfaceScript;

	private Image pressStart;
	private Image backplane;
	
	private float t = 0.0f;
	private bool one = false;
	
	void Start() {
		userInterfaceScript = GetComponent<UserInterface> ();

		pressStart = startScreen.transform.FindChild("Press_Start").GetComponent<Image>();
		backplane = startScreen.transform.FindChild("Backplane").GetComponent<Image>();
	}
	
	void Update () {
		if (Input.anyKeyDown && startScreen.transform.FindChild ("Text").gameObject.activeSelf) {
			userInterfaceScript.StartScreenSetActive (false);
			userInterfaceScript.MainMenuSetActive(true);
		}
	}

	void LateUpdate() {
		if (t < textPulseDuration && !one) {
			pressStart.CrossFadeAlpha (0, textPulseDuration - 0.8f, false);
			backplane.CrossFadeAlpha (0, textPulseDuration - 0.8f, false);

			t += Time.deltaTime / textPulseDuration;
			if (t >= 1)
				one = true;
		} else if (one) {
			pressStart.CrossFadeAlpha (255, (textPulseDuration - 0.8f) * 1000, false);
			backplane.CrossFadeAlpha (255, (textPulseDuration - 0.8f) * 1000, false);

			t -= Time.deltaTime / textPulseDuration;
			if (t <= 0)
				one = false;
		}
	}
}
