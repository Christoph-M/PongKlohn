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
	public Text textPlusOne;
	public Text textMinusOne;
	public GameObject tournamentWin;
	public GameObject tournamentLose;
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

				temple [i].transform.FindChild ("1").gameObject.SetActive (false);
				temple [i].transform.FindChild ("0").gameObject.SetActive (true);
			}
		}

		StartCoroutine (this.UnlockSpecial ());
	}

	public void Crystal(int i) {
		masterScript.SetCrystal (1, i);
		crystalInfoText = i;
		masterScript.GetComponent<AudioSource> ().Play ();
	}

	public void ChangeText(int i) {
		if (i > 0 && crystalSelect [i - 1].interactable) {
			switch (i) {
			case 1:
				crystalText.text = "Gawamba"; break;
			case 2:
				crystalText.text = "Iko"; break;
			case 3:
				crystalText.text = "Zarp"; break;
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
		masterScript.GetComponent<AudioSource> ().Play ();
		singleplayerScript.StartMatch ((int)MasterScript.Scene.spMap);
	}

	public void Back() {
		masterScript.GetComponent<AudioSource> ().Play ();
		StartCoroutine (sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.mainMenu, (int)MasterScript.Scene.spMap));
	}


	private IEnumerator UnlockSpecial() {
		bool showUnlock = false;

		for (int i = 0; i < crystalUnlocked.Count; ++i) {
			bool b = bool.Parse (Ini.IniReadValue ("Other", "unlock" + i + "HasBeenShown"));

			if (!b && singleplayerScript.GetCrystalCount(1) > 0) showUnlock = true;
		}

		if (singleplayerScript.GetNewUnlock () || showUnlock) {
			for (int i = crystalUnlocked.Count - 1; i >= 0; --i) {
				if (crystalUnlocked [i] && !bool.Parse (Ini.IniReadValue ("Other", "unlock" + i + "HasBeenShown"))) {
					yield return new WaitForSeconds (1.0f);

					crystalUnlock [i].SetActive (true);

					Ini.IniWriteValue ("Other", "unlock" + i + "HasBeenShown", "True");
					yield return new WaitUntil (() => Input.anyKeyDown);

					crystalUnlock [i].SetActive (false);
					break;
				}
			}
		}

		if (singleplayerScript.GetCrystalCount (1) != int.Parse (crystalCount [0].text)) {
			yield return StartCoroutine (this.UpdateCrystalCount ());
		} else {
			this.EnableButtons ();
		}

		eventSystem.SetSelectedGameObject (firstSelectElement);

		yield return 0;
	}

	private IEnumerator UpdateCrystalCount() {
		yield return new WaitForSeconds (1.0f);
		int enemiesAlive = 5;

		for (int i = 0; i < crystalCount.Count; ++i) {
			int crystalCnt = singleplayerScript.GetCrystalCount (i + 1);

			if (int.Parse (crystalCount [i].text) != crystalCnt) {
				if (int.Parse (crystalCount [i].text) < crystalCnt) {
					crystalCountUp.transform.position = crystalCount [i].transform.parent.position;
					textPlusOne.transform.position = new Vector3 (crystalCount [i].transform.parent.position.x, crystalCount [i].transform.parent.position.y + 1.0f, crystalCount [i].transform.parent.position.z);
					crystalCountUp.SetActive (true);
					textPlusOne.enabled = true;
				} else if (int.Parse (crystalCount [i].text) > crystalCnt) {
					crystalCountDown.transform.position = crystalCount [i].transform.parent.position;
					textMinusOne.transform.position = new Vector3 (crystalCount [i].transform.parent.position.x, crystalCount [i].transform.parent.position.y + 1.0f, crystalCount [i].transform.parent.position.z);
					crystalCountDown.SetActive (true);
					textMinusOne.enabled = true;
				}

				crystalCount [i].text = "" + crystalCnt;

				if (crystalCnt <= 0) {
					crystalCount [i].text = "" + 0;
					crystalCount [i].color = new Color32 (169, 0, 0, 255);
					--enemiesAlive;

					temple [i].transform.FindChild ("1").gameObject.SetActive (false);
					temple [i].transform.FindChild ("0").gameObject.SetActive (true);
				}

				singleplayerScript.SetStartCrystalCount (i + 1, crystalCnt);

				float t = 0.0f;
				Vector3 start = Vector3.zero;
				Vector3 end = Vector3.zero;
				if (crystalCountUp.activeSelf) {
					start = textPlusOne.transform.position;
					end = new Vector3(textPlusOne.transform.position.x, textPlusOne.transform.position.y + 3.0f, textPlusOne.transform.position.z);
				}
				if (crystalCountDown.activeSelf) {
					start = textMinusOne.transform.position;
					end = new Vector3(textMinusOne.transform.position.x, textMinusOne.transform.position.y + 3.0f, textMinusOne.transform.position.z);
				}

				while (t <= 0.7f) {
					if (crystalCountUp.activeSelf) {
						crystalCountUp.GetComponent<Image> ().CrossFadeAlpha (0, 0.5f, false);
						textPlusOne.transform.position = Vector3.Lerp (start, end, t);
						textPlusOne.CrossFadeAlpha (0, 0.7f, false);
					}
					if (crystalCountDown.activeSelf) {
						crystalCountDown.GetComponent<Image> ().CrossFadeAlpha (0, 0.5f, false);
						textMinusOne.transform.position = Vector3.Lerp (start, end, t);
						textMinusOne.CrossFadeAlpha (0, 0.7f, false);
					}

					t += Time.deltaTime;

					yield return new WaitForSeconds (0.05f * Time.deltaTime);
				}

				crystalCountUp.SetActive (false);
				textPlusOne.enabled = false;
				crystalCountDown.SetActive (false);
				textMinusOne.enabled = false;
			} else if (int.Parse (crystalCount [i].text) == 0) {
				--enemiesAlive;
			}

			if (singleplayerScript.GetCrystalCount (1) <= 0) {
				tournamentLose.SetActive (true);

				yield return new WaitUntil (() => Input.anyKeyDown);
				masterScript.GetComponent<AudioSource> ().Play ();

				singleplayerScript.DeleteGame ();

				StartCoroutine(sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.spMenu, (int)MasterScript.Scene.spMap));

				yield break;
			}

			if (enemiesAlive <= 0) {
				yield return new WaitForSeconds (1.0f);

				for (int f = 1; f < temple.Count; ++f) {
					temple [f].transform.FindChild ("2").gameObject.SetActive (true);
					temple [f].transform.FindChild ("1").gameObject.SetActive (false);
					temple [f].transform.FindChild ("0").gameObject.SetActive (false);

					crystalCount [i].text = "1";

					crystalCount [i].color = new Color32 (169, 0, 0, 255);

					yield return new WaitForSeconds (0.5f);
				}

				yield return new WaitForSeconds (1.0f);

				tournamentWin.SetActive (true);

				yield return new WaitUntil (() => Input.anyKeyDown);
				masterScript.GetComponent<AudioSource> ().Play ();

				StartCoroutine(sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.credits, (int)MasterScript.Scene.spMap));

				yield break;
			}
		}

		singleplayerScript.SaveGame ();

		this.EnableButtons ();
	}

	private void EnableButtons() {
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
