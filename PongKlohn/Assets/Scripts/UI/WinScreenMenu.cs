using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class WinScreenMenu : MonoBehaviour {
	public GameObject winScreenMenu;


	private MasterScript masterScript;
	private SceneHandler sceneHandlerScript;

	private EventSystem eventSystem;

	void Start () {
		masterScript = GameObject.FindObjectOfType (typeof(MasterScript)) as MasterScript;
		sceneHandlerScript = GameObject.FindObjectOfType (typeof(SceneHandler)) as SceneHandler;

		eventSystem = EventSystem.current;

		eventSystem.SetSelectedGameObject(GameObject.FindGameObjectWithTag ("FirstSelectedUI"));
	}
	
	public void Rematch() {
		StartCoroutine (sceneHandlerScript.StartGame (sceneHandlerScript.GetScene(6), sceneHandlerScript.GetScene(5)));
	}

	public void CharSelect() {
		StartCoroutine(sceneHandlerScript.LoadMenu (sceneHandlerScript.GetScene(4), sceneHandlerScript.GetScene(5)));
	}

	public void Quit() {
		StartCoroutine(sceneHandlerScript.LoadMenu (sceneHandlerScript.GetScene(2), sceneHandlerScript.GetScene(5)));
	}
}
