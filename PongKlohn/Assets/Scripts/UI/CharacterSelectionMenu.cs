using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterSelectionMenu : UserInterface {
	public GameObject characterSelectionMenu;

	public int maxCharacters = 2;


	private UserInterface userInterfaceScript;

	private Transform player1;
	private Transform player2;

	private const int p1 = 1;
	private const int p2 = 2;

	private int characterP1 = 1;
	private int characterP2 = 1;

	void Start() {
		userInterfaceScript = GetComponent<UserInterface> ();

		player1 = characterSelectionMenu.transform.FindChild ("Player1");
		player2 = characterSelectionMenu.transform.FindChild ("Player2");
	}

	public void Player1SelectRight() {
		int old = characterP1;

		if (characterP1 < maxCharacters) {
			++characterP1;
		} else {
			characterP1 = 1;
		}

		this.SwitchCharacter (p1, characterP1, old);
	}

	public void Player1SelectLeft() {
		int old = characterP1;

		if (characterP1 > 1) {
			--characterP1;
		} else {
			characterP1 = maxCharacters;
		}

		this.SwitchCharacter (p1, characterP1, old);
	}

	public void SelectPlayer1() {
		this.PlayerSelect (p1);
	}

	public void Crystal1OneEnter() {
		this.CrystalText (p1, "Crystal 1");
	}

	public void Crystal1TwoEnter() {
		this.CrystalText (p1, "Crystal 2");
	}

	public void Crystal1ThreeEnter() {
		this.CrystalText (p1, "Crystal 3");
	}

	public void Crystal1One() {

	}

	public void Crystal1Two() {

	}

	public void Crystal1Three() {

	}

	public void Player2SelectRight() {
		int old = characterP2;

		if (characterP2 < maxCharacters) {
			++characterP2;
		} else {
			characterP2 = 1;
		}

		this.SwitchCharacter (p2, characterP2, old);
	}

	public void Player2SelectLeft() {
		int old = characterP2;

		if (characterP2 > 1) {
			--characterP2;
		} else {
			characterP2 = maxCharacters;
		}

		this.SwitchCharacter (p2, characterP2, old);
	}

	public void SelectPlayer2() {
		this.PlayerSelect (p2);
	}

	public void Crystal2OneEnter() {
		this.CrystalText (p2, "Crystal 1");
	}

	public void Crystal2TwoEnter() {
		this.CrystalText (p2, "Crystal 2");
	}

	public void Crystal2ThreeEnter() {
		this.CrystalText (p2, "Crystal 3");
	}

	public void Crystal2One() {

	}

	public void Crystal2Two() {

	}

	public void Crystal2Three() {

	}

	public void Back() {
		userInterfaceScript.characterSelectionMenuSetActive (false);
		userInterfaceScript.MainMenuSetActive (true);
	}


	private void SwitchCharacter(int player, int character, int old) {
		GameObject charOld = (player == 1) ? player1.FindChild("Character_" + old).gameObject : player2.FindChild("Character_" + old).gameObject;
		GameObject charNew = (player == 1) ? player1.FindChild("Character_" + character).gameObject : player2.FindChild("Character_" + character).gameObject;

		charOld.SetActive (false);
		charNew.SetActive (true);
	}

	private void PlayerSelect(int player) {
		GameObject glow = (player == 1) ? player1.FindChild ("Glow").gameObject : player2.FindChild ("Glow").gameObject;
		GameObject sButtons = (player == 1) ? player1.FindChild ("Select_Buttons").gameObject : player2.FindChild ("Select_Buttons").gameObject;
		GameObject cButtons = (player == 1) ? player1.FindChild ("Crystal_Buttons").gameObject : player2.FindChild ("Crystal_Buttons").gameObject;

		if (!glow.activeSelf) {
			glow.SetActive (true);
			sButtons.SetActive (false);
			cButtons.SetActive (true);
		} else {
			glow.SetActive (false);
			sButtons.SetActive (true);
			cButtons.SetActive (false);
			this.CrystalText (player, "");
		}
	}

	private void CrystalText(int player, string text) {
		Text textBox = (player == 1) ? player1.FindChild ("Text_Box").GetComponent<Text> () : player2.FindChild ("Text_Box").GetComponent<Text> ();

		textBox.text = text;
	}
}
