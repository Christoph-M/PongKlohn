using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioOptionsMenu : UserInterface {
	public GameObject audioOptionsMenu;


	private UserInterface userInterfaceScript;

	void Start () {
		userInterfaceScript = GetComponent<UserInterface> ();
	}

	public void Mute() {
		
	}

	public void MasterVolume() {
		AudioListener.volume = audioOptionsMenu.transform.FindChild ("Master_Volume").GetComponent<Slider> ().value;
	}

	public void SFXVolume() {

	}

	public void MusicVolume() {

	}

	public void Back() {
		userInterfaceScript.AudioOptionsMenuSetActive (false);
		userInterfaceScript.OptionsMenuSetActive (true);
	}
}
