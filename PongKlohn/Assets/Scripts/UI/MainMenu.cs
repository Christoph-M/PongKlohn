using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public GameObject mainMenu;


	private MasterScript masterScript;
	private SceneHandler sceneHandlerScript;

	private EventSystem eventSystem;

	void Start() {
		masterScript = GameObject.FindObjectOfType (typeof(MasterScript)) as MasterScript;
		sceneHandlerScript = GameObject.FindObjectOfType (typeof(SceneHandler)) as SceneHandler;

		eventSystem = EventSystem.current;

		eventSystem.SetSelectedGameObject(GameObject.FindGameObjectWithTag ("FirstSelectedUI"));
	}

	public void Singleplayer() {
		masterScript.SetPlayerType (1, "KeyP1");
		masterScript.SetPlayerType (2, "Ai");

		StartCoroutine(sceneHandlerScript.StartGame (sceneHandlerScript.GetScene(6), sceneHandlerScript.GetScene(2)));
	}

	public void Multiplayer() {
		masterScript.SetPlayerType (1, "KeyP1");
		masterScript.SetPlayerType (2, "KeyP2");

		StartCoroutine(sceneHandlerScript.LoadMenu (sceneHandlerScript.GetScene(4), sceneHandlerScript.GetScene(2)));
	}

	public void OptionsMenu() {
		StartCoroutine(sceneHandlerScript.LoadMenu (sceneHandlerScript.GetScene(3), sceneHandlerScript.GetScene(2)));
	}

	public void Credits() {

	}
	
	public void Quit() {
		Application.Quit ();
	}
}
