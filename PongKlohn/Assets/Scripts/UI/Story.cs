using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Story : MonoBehaviour {
	private Singleplayer singleplayerScript;

	private MovieTexture video;

	void Start () {
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
				singleplayerScript.StartMatch ((int)MasterScript.Scene.story);
			}

			yield return new WaitForSeconds (0.05f);
		}
	}
}
