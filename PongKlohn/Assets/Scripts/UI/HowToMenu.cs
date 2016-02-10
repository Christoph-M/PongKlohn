using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class HowToMenu : MonoBehaviour {
	public GameObject howToMenu;
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

	public void Back() {
		masterScript.GetComponent<AudioSource> ().Play ();
		StartCoroutine(sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.optionsMenu, (int)MasterScript.Scene.howToMenu));
	}
}
