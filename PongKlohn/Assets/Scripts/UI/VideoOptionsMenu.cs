using UnityEngine;
using System.Collections;

public class VideoOptionsMenu : UserInterface {
	public GameObject videoOptionsMenu;


	private UserInterface userInterfaceScript;

	void Start () {
		userInterfaceScript = GetComponent<UserInterface> ();
	}

	public void Back() {
		userInterfaceScript.VideoOptionsMenuSetActive (false);
		userInterfaceScript.OptionsMenuSetActive (true);
	}
}
