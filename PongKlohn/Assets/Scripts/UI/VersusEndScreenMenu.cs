using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class VersusEndScreenMenu : MonoBehaviour {
	public GameObject versusEndScreenMenu;
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
		masterScript.GetComponent<AudioSource> ().Play ();
		StartCoroutine (sceneHandlerScript.StartGame ((int)MasterScript.Scene.gameScene, (int)MasterScript.Scene.versusEndScreen));
	}

	public void CharSelect() {
		masterScript.GetComponent<AudioSource> ().Play ();
		StartCoroutine(sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.characterSelect, (int)MasterScript.Scene.versusEndScreen));
	}

	public void Quit() {
		masterScript.GetComponent<AudioSource> ().Play ();
		StartCoroutine(sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.mainMenu, (int)MasterScript.Scene.versusEndScreen));
	}
}
