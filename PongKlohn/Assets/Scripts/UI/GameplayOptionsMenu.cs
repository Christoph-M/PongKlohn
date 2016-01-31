using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameplayOptionsMenu : MonoBehaviour {
	public GameObject gameplayOptionsMenu;


	private MasterScript masterScript;
	private SceneHandler sceneHandlerScript;

	void Start () {
		masterScript = GameObject.FindObjectOfType (typeof(MasterScript)) as MasterScript;
		sceneHandlerScript = GameObject.FindObjectOfType (typeof(SceneHandler)) as SceneHandler;
	}

	public void AiHandicap() {

	}

	public void EnableGamepad() {
		if (gameplayOptionsMenu.transform.FindChild ("Toggle").GetComponent<Toggle> ().isOn) {

		} else {

		}
	}

	public void GamepadBindings() {

	}

	public void KeyboardBindings() {

	}

	public void Back() {
		
	}
}
