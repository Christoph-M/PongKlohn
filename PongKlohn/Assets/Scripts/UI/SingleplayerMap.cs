using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class SingleplayerMap : MonoBehaviour {
	public GameObject singleplayerMapMenu;
	public GameObject firstSelectElement;
	public GameObject crystalCountUp;
	public GameObject crystalCountDown;
	public List<Text> crystalCount;


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

		foreach (Button button in singleplayerMapMenu.GetComponentsInChildren<Button>()) {
			button.interactable = false;
		}

		StartCoroutine (this.UpdateCrystalCount ());
	}

	public void Next() {
		singleplayerScript.StartRound ((int)MasterScript.Scene.spMap);
	}

	public void Back() {
		StartCoroutine (sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.mainMenu, (int)MasterScript.Scene.spMap));
	}

	private IEnumerator UpdateCrystalCount() {
		for (int i = 0; i < crystalCount.Count; ++i) {
			crystalCount [i].text = "" + singleplayerScript.GetStartCrystalCount (i + 1);
		}

		yield return new WaitForSeconds (3.0f);

		for (int i = 0; i < crystalCount.Count; ++i) {
			int crystalCnt = singleplayerScript.GetCrystalCount (i + 1);

			if (int.Parse (crystalCount [i].text) < crystalCnt) {
				crystalCountUp.transform.position = crystalCount [i].transform.parent.position;
				crystalCountUp.SetActive (true);
			} else if (int.Parse (crystalCount [i].text) > crystalCnt) {
				crystalCountDown.transform.position = crystalCount [i].transform.parent.position;
				crystalCountDown.SetActive (true);
			}

			crystalCount [i].text = "" + crystalCnt;

			if (crystalCnt <= 0) {
				crystalCount [i].text = "" + 0;
				crystalCount [i].color = new Color (169, 0, 0, 255);
			}

			singleplayerScript.SetStartCrystalCount (i + 1, crystalCnt);

			yield return new WaitForSeconds (0.5f);

			crystalCountUp.SetActive (false);
			crystalCountDown.SetActive (false);
		}

		foreach (Button button in singleplayerMapMenu.GetComponentsInChildren<Button>()) {
			button.interactable = true;
		}
	}
}
