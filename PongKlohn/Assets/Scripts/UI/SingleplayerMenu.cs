using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SingleplayerMenu : MonoBehaviour {
	public GameObject winScreenMenu;
	public GameObject firstSelectElement;
	public Button continueButton;


	private MasterScript masterScript;
	private SceneHandler sceneHandlerScript;
	private Singleplayer singleplayerScript;

	private EventSystem eventSystem;

	void Start () {
		masterScript = GameObject.FindObjectOfType (typeof(MasterScript)) as MasterScript;
		sceneHandlerScript = GameObject.FindObjectOfType (typeof(SceneHandler)) as SceneHandler;
		singleplayerScript = GameObject.FindObjectOfType (typeof(Singleplayer)) as Singleplayer;

		eventSystem = EventSystem.current;

		if (!bool.Parse (Ini.IniReadValue ("Other", "deleted"))) {
			continueButton.interactable = true;
			eventSystem.SetSelectedGameObject (continueButton.gameObject);
		} else {
			eventSystem.SetSelectedGameObject(firstSelectElement);
		}
	}

	public void Continue() {
		singleplayerScript.LoadGame ();

		StartCoroutine(sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.spMap, (int)MasterScript.Scene.spMenu));
	}

	public void NewGame() {
		singleplayerScript.DeleteGame ();

		StartCoroutine(sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.story, (int)MasterScript.Scene.spMenu));
	}

	public void Back() {
		StartCoroutine(sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.mainMenu, (int)MasterScript.Scene.spMenu));
	}
}
