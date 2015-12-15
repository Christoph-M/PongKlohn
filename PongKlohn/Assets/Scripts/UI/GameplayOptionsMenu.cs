using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameplayOptionsMenu : MonoBehaviour {
	public GameObject gameplayOptionsMenu;


	private UserInterface userinterfaceScript;

	void Start () {
		userinterfaceScript = GetComponent<UserInterface> ();
	}

	public void AiHandicap() {

	}

	public void EnableGamepad() {
		if (gameplayOptionsMenu.transform.FindChild ("Toggle").GetComponent<Toggle> ().isOn) {

		} else {

		}
	}

	public void GamepadBindings() {

	}

	public void KeyboardBindings() {

	}

	public void Back() {
		userinterfaceScript.GameplayOptionsMenuSetActive (false);
		userinterfaceScript.OptionsMenuSetActive (true);
	}
}
