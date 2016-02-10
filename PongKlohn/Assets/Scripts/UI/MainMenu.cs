using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {
	public GameObject mainMenu;
	public GameObject firstSelectElement;

	public List<Image> canes;


	private MasterScript masterScript;
	private SceneHandler sceneHandlerScript;

	private EventSystem eventSystem;

	void Start() {
		masterScript = GameObject.FindObjectOfType (typeof(MasterScript)) as MasterScript;
		sceneHandlerScript = GameObject.FindObjectOfType (typeof(SceneHandler)) as SceneHandler;

		eventSystem = EventSystem.current;

		eventSystem.SetSelectedGameObject(firstSelectElement);
	}

	public void Singleplayer() {
		masterScript.GetComponent<AudioSource> ().Play ();
		masterScript.SetPlayerType (1, "KeyP1");
		masterScript.SetPlayerType (2, "Ai");

		StartCoroutine (sceneHandlerScript.StartSingleplayer ((int)MasterScript.Scene.spLoop, (int)MasterScript.Scene.mainMenu));
	}

	public void Multiplayer() {
		masterScript.GetComponent<AudioSource> ().Play ();
		masterScript.SetPlayerType (1, "KeyP1");
		masterScript.SetPlayerType (2, "KeyP2");

		StartCoroutine(sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.characterSelect, (int)MasterScript.Scene.mainMenu));
	}

	public void OptionsMenu() {
		masterScript.GetComponent<AudioSource> ().Play ();
		StartCoroutine(sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.optionsMenu, (int)MasterScript.Scene.mainMenu));
	}

	public void Credits() {
		masterScript.GetComponent<AudioSource> ().Play ();
		StartCoroutine(sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.credits, (int)MasterScript.Scene.mainMenu));
	}
	
	public void Quit() {
		masterScript.GetComponent<AudioSource> ().Play ();
		Application.Quit ();
	}

	public void UpdateCane(int i) {
		switch (i) {
		case 0:
			foreach (Image cane in canes) {
				cane.enabled = false;
			}
			break;
		case 1:
			canes [i - 1].enabled = true; break;
		case 2:
			canes [i - 1].enabled = true; break;
		case 3:
			canes [i - 1].enabled = true; break;
		case 4:
			canes [i - 1].enabled = true; break;
		case 5:
			canes [i - 1].enabled = true; break;
		default:
			break;
		}
	}
}
