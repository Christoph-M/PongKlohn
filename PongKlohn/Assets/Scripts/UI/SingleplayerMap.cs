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
	public GameObject tournamentWin;
	public Text crystalText;
	public List<GameObject> temple;
	public List<Text> crystalCount;
	public List<Toggle> crystalSelect;
	public List<GameObject> crystalUnlock;


	private MasterScript masterScript;
	private SceneHandler sceneHandlerScript;
	private Singleplayer singleplayerScript;

	private EventSystem eventSystem;

	private List<bool> crystalUnlocked;

	private int crystalInfoText = -1;

	void Start () {
		masterScript = GameObject.FindObjectOfType (typeof(MasterScript)) as MasterScript;
		sceneHandlerScript = GameObject.FindObjectOfType (typeof(SceneHandler)) as SceneHandler;
		singleplayerScript = GameObject.FindObjectOfType (typeof(Singleplayer)) as Singleplayer;

		eventSystem = EventSystem.current;

		crystalUnlocked = singleplayerScript.GetCrystalUnlockStatus ();

		for (int i = 0; i < crystalCount.Count; ++i) {
			crystalCount [i].text = "" + singleplayerScript.GetStartCrystalCount (i + 1);
			if (int.Parse (crystalCount [i].text) <= 0) {
				crystalCount [i].text = "" + 0;
				crystalCount [i].color = new Color32 (169, 0, 0, 255);
			}
		}

		StartCoroutine (this.UnlockSpecial ());
	}

	public void Crystal(int i) {
		masterScript.SetCrystal (1, i);
		crystalInfoText = i;
	}

	public void ChangeText(int i) {
		if (i > 0 && crystalSelect [i - 1].interactable) {
			switch (i) {
			case 1:
				crystalText.text = "Crystal One"; break;
			case 2:
				crystalText.text = "Crystal Two"; break;
			case 3:
				crystalText.text = "Crystal Three"; break;
			default:
				break;
			}
		} else if (i > 0) {
			crystalText.text = "Locked";
		} else if (i == 0) {
			this.ChangeText (crystalInfoText);
		}
	}

	public void Next() {
		singleplayerScript.StartMatch ((int)MasterScript.Scene.spMap);
	}

	public void Back() {
		StartCoroutine (sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.mainMenu, (int)MasterScript.Scene.spMap));
	}


	private IEnumerator UnlockSpecial() {
		if (singleplayerScript.GetNewUnlock ()) {
			for (int i = crystalUnlocked.Count - 1; i >= 0; --i) {
				if (crystalUnlocked [i]) {
					yield return new WaitForSeconds (1.0f);

					crystalUnlock [i].SetActive (true);

					yield return new WaitUntil (() => Input.anyKeyDown);

					crystalUnlock [i].SetActive (false);
					break;
				}
			}
		}

		eventSystem.SetSelectedGameObject(firstSelectElement);

		yield return StartCoroutine (this.UpdateCrystalCount ());
	}

	private IEnumerator UpdateCrystalCount() {
		singleplayerScript.SaveGame ();

		yield return new WaitForSeconds (3.0f);
		int enemiesAlive = 5;

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
				crystalCount [i].color = new Color32 (169, 0, 0, 255);
				--enemiesAlive;
			}

			singleplayerScript.SetStartCrystalCount (i + 1, crystalCnt);

			yield return new WaitForSeconds (0.5f);

			crystalCountUp.SetActive (false);
			crystalCountDown.SetActive (false);

			if (enemiesAlive <= 0) {
				yield return new WaitForSeconds (1.0f);

				tournamentWin.SetActive (true);

				yield return new WaitUntil (() => Input.anyKeyDown);

				tournamentWin.SetActive (false);
			}
		}

		foreach (Button button in singleplayerMapMenu.GetComponentsInChildren<Button>()) {
			button.interactable = true;
		}

		for (int i = 0; i < crystalSelect.Count; ++i) {
			if (crystalUnlocked [i]) {
				crystalSelect [i].interactable = true;
				if (masterScript.GetCrystal (1) == i + 1) crystalSelect [i].isOn = true;
			}
		}
	}
}
