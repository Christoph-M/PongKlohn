using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : UserInterface {
	public GameObject mainMenu;


	private UserInterface userInterfaceScript;

	void Start() {
		userInterfaceScript = GetComponent<UserInterface> ();
	}

	public void Singleplayer() {
		Application.LoadLevel(2);
	}

	public void Multiplayer() {
		userInterfaceScript.MainMenuSetActive (false);
		userInterfaceScript.characterSelectionMenuSetActive (true);
	}

	public void Arcade() {

	}

	public void OptionsMenu() {
		userInterfaceScript.MainMenuSetActive(false);
		userInterfaceScript.OptionsMenuSetActive(true);
	}

	public void Credits() {

	}
	
	public void Quit() {
		Application.Quit ();
	}
}
