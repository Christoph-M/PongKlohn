using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class WinScreenMenu : MonoBehaviour {
	public GameObject winScreenMenu;
	public GameObject firstSelectElement;


	private MasterScript masterScript;
	private SceneHandler sceneHandlerScript;

	private EventSystem eventSystem;

	void Start () {
		masterScript = GameObject.FindObjectOfType (typeof(MasterScript)) as MasterScript;
		sceneHandlerScript = GameObject.FindObjectOfType (typeof(SceneHandler)) as SceneHandler;

		eventSystem = EventSystem.current;

		eventSystem.SetSelectedGameObject(firstSelectElement);
	}
	
	public void Rematch() {
		StartCoroutine (sceneHandlerScript.StartGame ((int)MasterScript.Scene.gameScene, (int)MasterScript.Scene.winScreen));
	}

	public void CharSelect() {
		StartCoroutine(sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.characterSelect, (int)MasterScript.Scene.winScreen));
	}

	public void Quit() {
		StartCoroutine(sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.mainMenu, (int)MasterScript.Scene.winScreen));
	}
}
