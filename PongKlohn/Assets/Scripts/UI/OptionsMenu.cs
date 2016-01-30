using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class OptionsMenu : UserInterface {
	public GameObject optionsMenu;


	private UserInterface userInterfaceScript;

	private EventSystem eventSystem;

	void Start() {
		userInterfaceScript = GetComponent<UserInterface> ();

		eventSystem = EventSystem.current;

		eventSystem.SetSelectedGameObject(GameObject.FindGameObjectWithTag ("FirstSelectedUI"));
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
		userInterfaceScript.MainMenuSetActive (true);
		userInterfaceScript.OptionsMenuSetActive (false);
	}
}
