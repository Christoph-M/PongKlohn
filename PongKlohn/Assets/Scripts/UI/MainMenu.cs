using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public GameObject mainMenu;
	public GameObject firstSelectElement;


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
		masterScript.SetPlayerType (1, "KeyP1");
		masterScript.SetPlayerType (2, "Ai");

		masterScript.LoadScene (masterScript.scenes[(int)MasterScript.Scene.spLoop], false);

//		StartCoroutine(sceneHandlerScript.StartGame ((int)MasterScript.Scene.gameScene, (int)MasterScript.Scene.mainMenu));
	}

	public void Multiplayer() {
		masterScript.SetPlayerType (1, "KeyP1");
		masterScript.SetPlayerType (2, "KeyP2");

		StartCoroutine(sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.characterSelect, (int)MasterScript.Scene.mainMenu));
	}

	public void OptionsMenu() {
		StartCoroutine(sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.optionsMenu, (int)MasterScript.Scene.mainMenu));
	}

	public void Credits() {

	}
	
	public void Quit() {
		Application.Quit ();
	}
}
