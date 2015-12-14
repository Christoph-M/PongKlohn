using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : UserInterface {
	public GameObject startScreen;
	public GameObject mainMenu;
	public GameObject matchMenu;

	public float textPulseDuration = 1.0f;
	
	
	private Color textPulse;
	private Color outlinePulse = Color.black;
	
	private float t = 0.0f;
	private bool one = false;
	
	void Start() {
		textPulse = startScreen.GetComponentInChildren<Text> ().color;
	}
	
	void Update () {
		if (Input.anyKeyDown && startScreen.transform.FindChild ("Text").gameObject.activeSelf) {
			startScreen.transform.FindChild ("Text").gameObject.SetActive(false);
			startScreen.transform.FindChild ("Press_Start").gameObject.SetActive(false);
			mainMenu.SetActive(true);
		}
	}
	
	void LateUpdate() {
		if (startScreen.transform.FindChild ("Text").gameObject.activeSelf) {
			textPulse = Color.Lerp (startScreen.GetComponentInChildren<Text> ().color, Color.clear, t);
			outlinePulse = Color.Lerp (Color.black, Color.clear, t);
			startScreen.transform.FindChild ("Press_Start").GetComponent<Text> ().color = textPulse;
			startScreen.transform.FindChild ("Press_Start").GetComponent<Outline> ().effectColor = outlinePulse;
		
			if (!one) {
				t += Time.deltaTime / textPulseDuration;
				if (t >= 1)
					one = true;
			} else if (one) {
				t -= Time.deltaTime / textPulseDuration;
				if (t <= 0)
					one = false;
			}
		}
	}

	public void SetVolume() {
		AudioListener.volume = mainMenu.transform.FindChild ("Volume_Slider").GetComponent<Slider> ().value;
	}

	public void MatchSettings() {
		mainMenu.SetActive(false);
		matchMenu.SetActive(true);
	}
	
	public void StartGame() {
		Application.LoadLevel(1);
	}
	
	public void ExitGame() {
		Application.Quit ();
	}
}
