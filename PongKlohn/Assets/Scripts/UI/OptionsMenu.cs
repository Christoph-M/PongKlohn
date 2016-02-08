using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class OptionsMenu : MonoBehaviour {
	public GameObject optionsMenu;
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

	public void HowTo() {
		StartCoroutine(sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.howToMenu, (int)MasterScript.Scene.optionsMenu));
	}

	public void Back() {
		StartCoroutine(sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.mainMenu, (int)MasterScript.Scene.optionsMenu));
	}
}
