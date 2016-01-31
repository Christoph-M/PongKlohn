using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioOptionsMenu : MonoBehaviour {
	public GameObject audioOptionsMenu;


	private MasterScript masterScript;
	private SceneHandler sceneHandlerScript;

	void Start () {
		masterScript = GameObject.FindObjectOfType (typeof(MasterScript)) as MasterScript;
		sceneHandlerScript = GameObject.FindObjectOfType (typeof(SceneHandler)) as SceneHandler;
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
		
	}
}
