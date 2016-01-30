using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MainMenu : UserInterface {
	public GameObject mainMenu;


	private UserInterface userInterfaceScript;

	private EventSystem eventSystem;

	void Start() {
		userInterfaceScript = GetComponent<UserInterface> ();

		eventSystem = EventSystem.current;

		eventSystem.SetSelectedGameObject(GameObject.FindGameObjectWithTag ("FirstSelectedUI"));
	}

	public void Singleplayer() {
		userInterfaceScript.SetPlayerType (1, "KeyP1");
		userInterfaceScript.SetPlayerType (2, "Ai");

		StartCoroutine(userInterfaceScript.StartGame (5, 4));
	}

	public void Multiplayer() {
		userInterfaceScript.SetPlayerType (1, "KeyP1");
		userInterfaceScript.SetPlayerType (2, "KeyP2");

		userInterfaceScript.CharacterSelectionMenuSetActive (true);
		userInterfaceScript.MainMenuSetActive (false);
	}

	public void OptionsMenu() {
		userInterfaceScript.OptionsMenuSetActive(true);
		userInterfaceScript.MainMenuSetActive(false);
	}

	public void Credits() {

	}
	
	public void Quit() {
		Application.Quit ();
	}
}
