using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsMenu : UserInterface {
	public GameObject optionsMenu;


	private UserInterface userInterfaceScript;

	void Start() {
		userInterfaceScript = GetComponent<UserInterface> ();
	}

	public void Video() {
		userInterfaceScript.OptionsMenuSetActive (false);
		userInterfaceScript.VideoOptionsMenuSetActive (true);
	}

	public void Audio() {
		userInterfaceScript.OptionsMenuSetActive (false);
		userInterfaceScript.AudioOptionsMenuSetActive (true);
	}

	public void Gameplay() {
		userInterfaceScript.OptionsMenuSetActive (false);
		userInterfaceScript.GameplayOptionsMenuSetActive (true);
	}

	public void Back() {
		userInterfaceScript.OptionsMenuSetActive (false);
		userInterfaceScript.MainMenuSetActive (true);
	}
}
