using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class WinScreenMenu : MonoBehaviour {
	public GameObject winScreenMenu;
	public GameObject firstSelectElement;


	private MasterScript masterScript;
	private SceneHandler sceneHandlerScript;
	private Singleplayer singleplayerScript;

	private EventSystem eventSystem;

	void Start () {
		masterScript = GameObject.FindObjectOfType (typeof(MasterScript)) as MasterScript;
		sceneHandlerScript = GameObject.FindObjectOfType (typeof(SceneHandler)) as SceneHandler;
		singleplayerScript = GameObject.FindObjectOfType (typeof(Singleplayer)) as Singleplayer;

		eventSystem = EventSystem.current;

		eventSystem.SetSelectedGameObject(firstSelectElement);

		singleplayerScript.SetWinner (1);
	}
	
	public void Continue() {
		if (singleplayerScript.RoundContinues ()) {
			singleplayerScript.StartMatch ((int)MasterScript.Scene.winScreen);
		} else {
			singleplayerScript.UpdateRound ();
			singleplayerScript.EndRound ((int)MasterScript.Scene.winScreen);
		}
	}

	public void SaveAndExit() {
		singleplayerScript.SaveGame ();

		StartCoroutine(sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.mainMenu, (int)MasterScript.Scene.winScreen));
	}
}
