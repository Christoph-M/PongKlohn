using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class CharacterSelectionMenu : MonoBehaviour {
	public GameObject characterSelectionMenu;
	public GameObject firstSelectElement;

	public int maxCharacters = 2;


	private MasterScript masterScript;
	private SceneHandler sceneHandlerScript;

	private EventSystem eventSystem;

	private Transform player1;
	private Transform player2;

	private const int p1 = 1;
	private const int p2 = 2;

	private List<int> selectedCharacters = new List<int> { -1, -1, -1, -1, -1, -1 };
	private List<int> characters = new List<int> { 1, 1 };
	private List<int> crystals = new List<int> { -1, -1, -1, -1, -1, -1 };

	private bool p1Ready = false;
	private bool p2Ready = false;

	void Start() {
		masterScript = GameObject.FindObjectOfType (typeof(MasterScript)) as MasterScript;
		sceneHandlerScript = GameObject.FindObjectOfType (typeof(SceneHandler)) as SceneHandler;

		eventSystem = EventSystem.current;

		eventSystem.SetSelectedGameObject(firstSelectElement);

		player1 = characterSelectionMenu.transform.FindChild ("Player1");
		player2 = characterSelectionMenu.transform.FindChild ("Player2");

		StartCoroutine (EnableStart ());
	}

	public void SelectRight(int p) {
		int old = characters[p - 1];

		if (characters[p - 1] < maxCharacters) {
			++characters[p - 1];
		} else {
			characters[p - 1] = 1;
		}

		this.SwitchCharacter (p, characters[p - 1], old);
	}

	public void SelectLeft(int p) {
		int old = characters[p - 1];

		if (characters[p - 1] > 1) {
			--characters[p - 1];
		} else {
			characters[p - 1] = maxCharacters;
		}

		this.SwitchCharacter (p, characters[p - 1], old);
	}

	public void SelectPlayer1() {
		if (characters [p1 - 1] != selectedCharacters [p2 - 1]) {
			GameObject glow = player1.FindChild ("Glow").gameObject;

			this.PlayerSelect (p1);

			glow.SetActive (p1Ready);
		}
	}

	public void CrystalP1(int i) {
		p1Ready = !p1Ready;
		if (crystals [p1 - 1] == i) {
			crystals [p1 - 1] = -1;
		} else {
			crystals [p1 - 1] = i;
		}
	}

	public void Player2SelectRight() {
		int old = characters[p2 - 1];

		if (characters[p2 - 1] < maxCharacters) {
			++characters[p2 - 1];
		} else {
			characters[p2 - 1] = 1;
		}

		this.SwitchCharacter (p2, characters[p2 - 1], old);
	}

	public void Player2SelectLeft() {
		int old = characters[p2 - 1];

		if (characters[p2 - 1] > 1) {
			--characters[p2 - 1];
		} else {
			characters[p2 - 1] = maxCharacters;
		}

		this.SwitchCharacter (p2, characters[p2 - 1], old);
	}

	public void SelectPlayer2() {
		if (characters [p2 - 1] != selectedCharacters [p1 - 1]) {
			GameObject glow = player2.FindChild ("Glow").gameObject;

			this.PlayerSelect (p2);

			glow.SetActive (p2Ready);
		}
	}

	public void CrystalP2(int i) {
		p2Ready = !p2Ready;
		if (crystals [p2 - 1] == i) {
			crystals [p2 - 1] = -1;
		} else {
			crystals [p2 - 1] = i;
		}
	}

	public void Back() {
		StartCoroutine(sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.mainMenu, (int)MasterScript.Scene.characterSelect));
	}

	public void StartGame() {
		StartCoroutine (this.SetGame ());
	}


	private void SwitchCharacter(int player, int character, int old) {
		GameObject charOld = (player == 1) ? player1.FindChild("Player_" + old).gameObject : player2.FindChild("Player_" + old).gameObject;
		GameObject charNew = (player == 1) ? player1.FindChild("Player_" + character).gameObject : player2.FindChild("Player_" + character).gameObject;

		charOld.SetActive (false);
		charNew.SetActive (true);
	}

	private void PlayerSelect(int player) {
		GameObject sButtons = (player == 1) ? player1.FindChild ("Select_Buttons").gameObject : player2.FindChild ("Select_Buttons").gameObject;
		GameObject cButtons = (player == 1) ? player1.FindChild ("Crystal_Buttons").gameObject : player2.FindChild ("Crystal_Buttons").gameObject;

		if (selectedCharacters [player - 1] < 0) {
			selectedCharacters [player - 1] = characters [player - 1];
		} else {
			selectedCharacters [player - 1] = -1;
		}

		if (!cButtons.activeSelf) {
			sButtons.SetActive (false);
			cButtons.SetActive (true);
		} else {
			sButtons.SetActive (true);

			foreach (Toggle t in cButtons.GetComponentsInChildren<Toggle>()) {
				t.isOn = false;
			}

			crystals [player - 1] = -1;

			cButtons.SetActive (false);
			this.SetReady (player, false);
		}
	}

	private void SetReady(int player, bool enabled) {
		if (player == 1) {
			p1Ready = enabled;
		} else {
			p2Ready = enabled;
		}
	}

	private IEnumerator EnableStart() {
		while (true) {
			yield return new WaitUntil (() => p1Ready && p2Ready);
			Debug.Log ("P1: char " + selectedCharacters [0] + ", crystal " + crystals [0] + "\nP2: char " + selectedCharacters [1] + ", crystal " + crystals [1]);
			characterSelectionMenu.transform.FindChild ("Start").GetComponent<Button>().interactable = true;

			yield return new WaitUntil (() => !p1Ready || !p2Ready);

			characterSelectionMenu.transform.FindChild ("Start").GetComponent<Button>().interactable = false;
		}
	}

	private IEnumerator SetGame() {
		this.DisableMenu ();

		masterScript.SetCharacter (p1, selectedCharacters [p1 - 1]);
		masterScript.SetCrystal (p1, crystals[p1 - 1]);
		masterScript.SetCharacter (p2, selectedCharacters [p2 - 1]);
		masterScript.SetCrystal (p2, crystals[p2 - 1]);

		Debug.Log ("P1: char " + selectedCharacters [0] + ", crystal " + crystals [0] + "\nP2: char " + selectedCharacters [1] + ", crystal " + crystals [1]);

		yield return new WaitForSeconds (3);

		StartCoroutine(sceneHandlerScript.StartGame ((int)MasterScript.Scene.gameScene, (int)MasterScript.Scene.characterSelect));
	}

	private void DisableMenu() {
		foreach (Button button in characterSelectionMenu.GetComponentsInChildren<Button>()) {
			button.interactable = false;
		}

		foreach (EventTrigger eTrigger in characterSelectionMenu.GetComponentsInChildren<EventTrigger>()) {
			eTrigger.enabled = false;
		}

		foreach (Toggle toggle in characterSelectionMenu.GetComponentsInChildren<Toggle>()) {
			toggle.interactable = false;
		}
	}
}
