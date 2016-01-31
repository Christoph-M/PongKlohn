﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UserInterface : MonoBehaviour {
//	public GameObject canvas;
//
//
//	protected Game gameScript;
//
//	protected const int startScreen = 1;
//	protected const int mainMenu = 2;
//	protected const int optionsMenu = 3;
//	protected const int videoOptions = 4;
//	protected const int audioOptions = 5;
//	protected const int gameOptions = 6;
//	protected const int charSelect = 7;
//
//
//	private MasterScript masterScript;
//	private StartScreen startScreenScript;
//	private MainMenu mainMenuScript;
//	private OptionsMenu optionsMenuScript;
//	private VideoOptionsMenu videoOptionsMenuScript;
//	private AudioOptionsMenu audioOptionsMenuScript;
//	private GameplayOptionsMenu gameplayOptionsMenuScript;
//	private CharacterSelectionMenu characterSelectionMenuScript;
//	private MatchUI matchUIScript;
//	private WinScreenMenu winScreenMenuScript;
//
//	void Start () {
//		masterScript = GameObject.FindObjectOfType (typeof(MasterScript)) as MasterScript;
//
//		gameScript = GameObject.FindObjectOfType (typeof(Game)) as Game;
//
//		startScreenScript = GetComponent<StartScreen>();
//		mainMenuScript = GetComponent<MainMenu>();
//		optionsMenuScript = GetComponent<OptionsMenu>();
//		videoOptionsMenuScript = GetComponent<VideoOptionsMenu> ();
//		audioOptionsMenuScript = GetComponent<AudioOptionsMenu> ();
//		gameplayOptionsMenuScript = GetComponent<GameplayOptionsMenu> ();
//		characterSelectionMenuScript = GetComponent<CharacterSelectionMenu> ();
//		matchUIScript = GetComponent<MatchUI> ();
//		winScreenMenuScript = GetComponent<WinScreenMenu> ();
//
////		if (!masterScript.GetInMatch()) {
////			switch (masterScript.GetActiveMenu ()) {
////			case startScreen:
////				this.StartScreenSetActive (true); break;
////			case mainMenu:
////				this.MainMenuSetActive (true); break;
////			case optionsMenu:
////				this.OptionsMenuSetActive (true); break;
////			case videoOptions:
////				this.VideoOptionsMenuSetActive (true); break;
////			case audioOptions:
////				this.AudioOptionsMenuSetActive (true); break;
////			case gameOptions:
////				this.GameplayOptionsMenuSetActive (true); break;
////			case charSelect:
////				this.CharacterSelectionMenuSetActive (true); break;
////			case 8:
////				break;
////			default:
////				break;
////			}
////		}
//	}
//	
//	public void StartScreenSetActive (bool active) {
//		if (active) {
//			masterScript.LoadScene ("StartScreen");
//		} else {
//			masterScript.UnloadScene ("StartScreen");
//		}
//
////		startScreenScript.startScreen.transform.FindChild ("Text").gameObject.SetActive(active);
////		startScreenScript.startScreen.transform.FindChild ("Backplane").gameObject.SetActive(active);
////		startScreenScript.startScreen.transform.FindChild ("Press_Start").gameObject.SetActive(active);
//	}
//	
//	public void MainMenuSetActive (bool active) {
//		if (active) {
//			masterScript.LoadScene ("MainMenu");
//		} else {
//			masterScript.UnloadScene ("MainMenu");
//		}
//
////		mainMenuScript.mainMenu.SetActive (active);
//	}
//
//	public void OptionsMenuSetActive (bool active) {
//		if (active) {
//			masterScript.LoadScene ("OptionsMenu");
//		} else {
//			masterScript.UnloadScene ("OptionsMenu");
//		}
//
////		optionsMenuScript.optionsMenu.SetActive (active);
//	}
//
//	public void VideoOptionsMenuSetActive (bool active) {
////		videoOptionsMenuScript.videoOptionsMenu.SetActive (active);
//	}
//
//	public void AudioOptionsMenuSetActive (bool active) {
////		audioOptionsMenuScript.audioOptionsMenu.SetActive (active);
//	}
//
//	public void GameplayOptionsMenuSetActive (bool active) {
////		gameplayOptionsMenuScript.gameplayOptionsMenu.SetActive (active);
//	}
//
//	public void CharacterSelectionMenuSetActive (bool active) {
//		if (active) {
//			masterScript.LoadScene ("CharacterSelect");
//		} else {
//			masterScript.UnloadScene ("CharacterSelect");
//		}
//
////		characterSelectionMenuScript.characterSelectionMenu.SetActive (active);
//	}
//
//	public void MatchUISetActive (bool active) {
//		matchUIScript.matchUI.SetActive (active);
//	}
//
//	public void WinScreenMenuSetActive (bool active) {
//		winScreenMenuScript.winScreenMenu.SetActive (active);
//	}
//
//	public void SetPlayer(int character, int crystal) {
//		masterScript.SetCharacter (character);
//		masterScript.SetCrystal (crystal);
//	}
//
//	public void SetPlayerType(int player, string type) {
//		masterScript.SetPlayerType (player, type);
//	}
//
//	public void SetActiveMenu(int menu) {
//		masterScript.SetActiveMenu (menu);
//	}
//
//	public string GetScene(int i) {
//		return masterScript.scenes [i];
//	}
//
//	public IEnumerator StartGame(string sceneL, string sceneUL) {
//		masterScript.SetInMatch(true);
//		masterScript.SetActiveMenu (mainMenu);
//
//		masterScript.LoadScene (sceneL);
//
//		yield return new WaitUntil(() => SceneManager.GetSceneByName(sceneL).isLoaded);
//
//		masterScript.UnloadScene (sceneUL);
//	}
//
//	public IEnumerator EndGame(string scene) {
//		masterScript.SetInMatch(false);
//
//		masterScript.LoadScene (scene);
//
//		yield return new WaitUntil(() => SceneManager.GetSceneByName(scene).isLoaded);
//
//		masterScript.UnloadScene (this.GetScene(6));
//	}
}
