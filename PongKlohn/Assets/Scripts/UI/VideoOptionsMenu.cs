using UnityEngine;
using System.Collections;

public class VideoOptionsMenu : MonoBehaviour {
	public GameObject videoOptionsMenu;


	private MasterScript masterScript;
	private SceneHandler sceneHandlerScript;

	void Start () {
		masterScript = GameObject.FindObjectOfType (typeof(MasterScript)) as MasterScript;
		sceneHandlerScript = GameObject.FindObjectOfType (typeof(SceneHandler)) as SceneHandler;
	}

	public void Back() {
		
	}
}
