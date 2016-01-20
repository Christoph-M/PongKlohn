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
		userInterfaceScript.SetPlayerType (1, "KeyP1");
		userInterfaceScript.SetPlayerType (2, "Ai");

		StartCoroutine(userInterfaceScript.StartGame (2, 1));
	}

	public void Multiplayer() {
		userInterfaceScript.SetPlayerType (1, "KeyP1");
		userInterfaceScript.SetPlayerType (2, "KeyP2");

		userInterfaceScript.MainMenuSetActive (false);
		userInterfaceScript.CharacterSelectionMenuSetActive (true);
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
