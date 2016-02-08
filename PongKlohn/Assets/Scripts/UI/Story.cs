using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Story : MonoBehaviour {

	private Singleplayer singleplayerScript;

	void Start () {
		singleplayerScript = GameObject.FindObjectOfType (typeof(Singleplayer)) as Singleplayer;

		StartCoroutine (this.PlayStory ());
	}
	
	private IEnumerator PlayStory() {
		yield return new WaitForSeconds (5);

		singleplayerScript.StartMatch ((int)MasterScript.Scene.story);
	}
}
