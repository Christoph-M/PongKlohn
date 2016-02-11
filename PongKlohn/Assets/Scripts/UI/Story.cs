using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Story : MonoBehaviour {
	private MasterScript masterScript;
	private Singleplayer singleplayerScript;

	private MovieTexture video;

	void Start () {
		masterScript = GameObject.FindObjectOfType (typeof(MasterScript)) as MasterScript;
		singleplayerScript = GameObject.FindObjectOfType (typeof(Singleplayer)) as Singleplayer;

		video = (MovieTexture)GetComponent<Image> ().material.mainTexture;

		StartCoroutine (this.PlayStory ());
	}
	
	private IEnumerator PlayStory() {
		yield return new WaitForSeconds (1.0f);

		video.Play ();
		GetComponent<AudioSource> ().Play ();

		while (true) {
			if (!video.isPlaying || Input.anyKeyDown) {
				masterScript.GetComponent<AudioSource> ().Play ();
				video.Stop ();
				singleplayerScript.StartMatch ((int)MasterScript.Scene.story);
			}

			yield return new WaitForSeconds (0.01f);
		}
	}
}
