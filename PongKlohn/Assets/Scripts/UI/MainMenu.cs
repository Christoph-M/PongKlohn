using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : UserInterface {
	public GameObject mainMenu;
	public GameObject optionsMenu;
	public GameObject videoOptionsMenu;
	public GameObject audioOptionsMenu;
	public GameObject gameplayOptionsMenu;

	public void SetVolume() {
		AudioListener.volume = mainMenu.transform.FindChild ("Volume_Slider").GetComponent<Slider> ().value;
	}

	public void OptionsMenu() {
		mainMenu.SetActive(false);
		optionsMenu.SetActive(true);
	}
	
	public void StartGame() {
		Application.LoadLevel(1);
	}
	
	public void ExitGame() {
		Application.Quit ();
	}
}
