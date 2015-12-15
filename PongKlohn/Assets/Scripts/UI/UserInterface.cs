using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UserInterface : MonoBehaviour {
	public GameObject canvas;


	protected Game gameScript;


	private StartScreen startScreenScript;
	private MainMenu mainMenuScript;
	private OptionsMenu optionsMenuScript;
	private VideoOptionsMenu videoOptionsMenuScript;
	private AudioOptionsMenu audioOptionsMenuScript;
	private GameplayOptionsMenu gameplayOptionsMenuScript;

	// Use this for initialization
	void Start () {
		gameScript = GameObject.FindObjectOfType (typeof(Game)) as Game;

		startScreenScript = GetComponent<StartScreen>();
		mainMenuScript = GetComponent<MainMenu>();
		optionsMenuScript = GetComponent<OptionsMenu>();
		videoOptionsMenuScript = GetComponent<VideoOptionsMenu> ();
		audioOptionsMenuScript = GetComponent<AudioOptionsMenu> ();
		gameplayOptionsMenuScript = GetComponent<GameplayOptionsMenu> ();
	}
	
	public void StartScreenSetActive (bool active) {
		startScreenScript.startScreen.transform.FindChild ("Text").gameObject.SetActive(active);
		startScreenScript.startScreen.transform.FindChild ("Press_Start").gameObject.SetActive(active);
	}
	
	public void MainMenuSetActive (bool active) {
		mainMenuScript.mainMenu.SetActive (active);
	}

	public void OptionsMenuSetActive (bool active) {
		optionsMenuScript.optionsMenu.SetActive (active);
	}

	public void VideoOptionsMenuSetActive (bool active) {
		videoOptionsMenuScript.videoOptionsMenu.SetActive (active);
	}

	public void AudioOptionsMenuSetActive (bool active) {
		audioOptionsMenuScript.audioOptionsMenu.SetActive (active);
	}

	public void GameplayOptionsMenuSetActive (bool active) {
		gameplayOptionsMenuScript.gameplayOptionsMenu.SetActive (active);
	}
}
