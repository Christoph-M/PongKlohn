using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class LoseScreenMenu : MonoBehaviour {
	public GameObject loseScreenMenu;
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

		singleplayerScript.SetWinner (masterScript.GetCharacter(2));
		singleplayerScript.UpdateRound ();
	}

	public void Continue() {
		singleplayerScript.EndRound ((int)MasterScript.Scene.loseScreen);

		singleplayerScript.SaveGame ();
	}

	public void SaveAndExit() {
		if (!singleplayerScript.RoundContinues ()) {
			singleplayerScript.EndRound (-1, false);
		}

		singleplayerScript.SaveGame ();

		StartCoroutine(sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.mainMenu, (int)MasterScript.Scene.loseScreen));
	}
}
