using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class CharacterSelectionMenu : UserInterface {
	public GameObject characterSelectionMenu;

	public int maxCharacters = 2;


	private UserInterface userInterfaceScript;

	private EventSystem eventSystem;

	private Transform player1;
	private Transform player2;

	private const int p1 = 1;
	private const int p2 = 2;

	private List<int> characters = new List<int> { 1, 1 };
	private List<int> crystals = new List<int> { -1, -1 };

	private bool p1Ready = false;
	private bool p2Ready = false;

	void Start() {
		userInterfaceScript = GetComponent<UserInterface> ();

		eventSystem = EventSystem.current;

		eventSystem.SetSelectedGameObject(GameObject.FindGameObjectWithTag ("FirstSelectedUI"));

		player1 = characterSelectionMenu.transform.FindChild ("Player1");
		player2 = characterSelectionMenu.transform.FindChild ("Player2");

		StartCoroutine (SetGame ());
	}

	public void Player1SelectRight() {
		int old = characters[p1 - 1];

		if (characters[p1 - 1] < maxCharacters) {
			++characters[p1 - 1];
		} else {
			characters[p1 - 1] = 1;
		}

		this.SwitchCharacter (p1, characters[p1 - 1], old);
	}

	public void Player1SelectLeft() {
		int old = characters[p1 - 1];

		if (characters[p1 - 1] > 1) {
			--characters[p1 - 1];
		} else {
			characters[p1 - 1] = maxCharacters;
		}

		this.SwitchCharacter (p1, characters[p1 - 1], old);
	}

	public void SelectPlayer1() {
		this.PlayerSelect (p1);
	}

	public void Crystal1OneEnter() {
		this.CrystalText (p1, "Crystal One");
	}

	public void Crystal1TwoEnter() {
		this.CrystalText (p1, "Crystal Two");
	}

	public void Crystal1ThreeEnter() {
		this.CrystalText (p1, "Crystal Three");
	}

	public void CrystalP1(int crystal) {
		crystals[p1 - 1] = crystal;

		this.ReadyActive (p1, true);
	}

	public void Player1Ready() {
		GameObject glow = player1.FindChild ("Glow").gameObject;

		p1Ready = !p1Ready;

		glow.SetActive (p1Ready);
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
		this.PlayerSelect (p2);
	}

	public void Crystal2OneEnter() {
		this.CrystalText (p2, "Crystal One");
	}

	public void Crystal2TwoEnter() {
		this.CrystalText (p2, "Crystal Two");
	}

	public void Crystal2ThreeEnter() {
		this.CrystalText (p2, "Crystal Three");
	}

	public void CrystalP2(int crystal) {
		crystals[p2 - 1] = crystal;

		this.ReadyActive (p2, true);
	}

	public void Player2Ready() {
		GameObject glow = player2.FindChild ("Glow").gameObject;

		p2Ready = !p2Ready;

		glow.SetActive (p2Ready);
	}

	public void Back() {
		userInterfaceScript.MainMenuSetActive (true);
		userInterfaceScript.CharacterSelectionMenuSetActive (false);
	}


	private void SwitchCharacter(int player, int character, int old) {
		GameObject charOld = (player == 1) ? player1.FindChild("Character_" + old).gameObject : player2.FindChild("Character_" + old).gameObject;
		GameObject charNew = (player == 1) ? player1.FindChild("Character_" + character).gameObject : player2.FindChild("Character_" + character).gameObject;

		charOld.SetActive (false);
		charNew.SetActive (true);
	}

	private void PlayerSelect(int player) {
		GameObject sButtons = (player == 1) ? player1.FindChild ("Select_Buttons").gameObject : player2.FindChild ("Select_Buttons").gameObject;
		GameObject cButtons = (player == 1) ? player1.FindChild ("Crystal_Buttons").gameObject : player2.FindChild ("Crystal_Buttons").gameObject;

		if (!cButtons.activeSelf) {
			sButtons.SetActive (false);
			cButtons.SetActive (true);

			if (crystals[player - 1] > 0) {
				this.ReadyActive (player, true);
			}
		} else {
			sButtons.SetActive (true);
			cButtons.SetActive (false);
			this.ReadyActive (player, false);
			this.CrystalText (player, "");
		}
	}

	private void CrystalText(int player, string text) {
		Text textBox = (player == 1) ? player1.FindChild ("Text_Box").GetComponent<Text> () : player2.FindChild ("Text_Box").GetComponent<Text> ();

		textBox.text = text;
	}

	private void ReadyActive(int player, bool enabled) {
		GameObject readyButton = (player == 1) ? player1.FindChild ("Ready").gameObject : player2.FindChild ("Ready").gameObject;

		if (!enabled) readyButton.GetComponent<Toggle> ().isOn = false;

		readyButton.SetActive (enabled);
	}

	private IEnumerator SetGame() {
		yield return new WaitUntil (() => p1Ready && p2Ready);

		this.DisableMenu ();

		userInterfaceScript.SetPlayer (characters[p1 - 1], crystals[p1 - 1]);
		userInterfaceScript.SetPlayer (characters[p2 - 1], crystals[p2 - 1]);

		yield return new WaitForSeconds (3);

		StartCoroutine(userInterfaceScript.StartGame (5, 4));

		yield return 0;
	}

	private void DisableMenu() {
		characterSelectionMenu.transform.FindChild ("Back").gameObject.SetActive (false);

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
