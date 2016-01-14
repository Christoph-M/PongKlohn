using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinScreenMenu : UserInterface {
	public GameObject winScreenMenu;


	private UserInterface userInterfaceScript;

	void Start () {
		userInterfaceScript = GetComponent<UserInterface> ();
	}
	
	public void Rematch() {
		StartCoroutine (userInterfaceScript.StartGame (2, 2));
	}

	public void CharSelect() {
		userInterfaceScript.SetActiveMenu (charSelect);

		StartCoroutine (userInterfaceScript.EndGame (2));
	}

	public void Quit() {
		StartCoroutine (userInterfaceScript.EndGame (2));
	}
}
