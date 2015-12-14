using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UserInterface : MonoBehaviour {
	public GameObject canvas;


	protected Game gameScript;


	private StartScreen startScreen;
	private MainMenu mainMenu;

	// Use this for initialization
	void Start () {
		gameScript = GameObject.FindObjectOfType (typeof(Game)) as Game;

		startScreen = GameObject.FindObjectOfType (typeof(StartScreen)) as StartScreen;
		mainMenu = GameObject.FindObjectOfType (typeof(MainMenu)) as MainMenu;
	}
	
	protected void StartScreenSetActive (bool active) {
		startScreen.startScreen.SetActive (active);
	}
	
	protected void MainMenuSetActive (bool active) {
		Debug.Log (mainMenu.mainMenu.activeSelf);
		mainMenu.mainMenu.SetActive (active);
	}
}
