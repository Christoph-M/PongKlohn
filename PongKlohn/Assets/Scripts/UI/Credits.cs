using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Credits : MonoBehaviour {
	public GameObject howToMenu;
	public GameObject firstSelectElement;
	public Transform credits;
	public float speed;


	private MasterScript masterScript;
	private SceneHandler sceneHandlerScript;

	private EventSystem eventSystem;

	private Vector3 startPos;
	private Vector3 endPos = new Vector3(0.0f, 3821f, 0.0f);

	private float startTime;

	void Start() {
		masterScript = GameObject.FindObjectOfType (typeof(MasterScript)) as MasterScript;
		sceneHandlerScript = GameObject.FindObjectOfType (typeof(SceneHandler)) as SceneHandler;

		eventSystem = EventSystem.current;

		eventSystem.SetSelectedGameObject(firstSelectElement);

		startPos = credits.position;

		startTime = Time.time;

		StartCoroutine (ScrollNames ());
	}

	void Update () {
		if (Input.anyKeyDown) {
			masterScript.GetComponent<AudioSource> ().Play ();
			StartCoroutine(sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.mainMenu, (int)MasterScript.Scene.credits));
		}
	}

	private IEnumerator ScrollNames() {
		while (true) {
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / Vector3.Distance (startPos, endPos);

			credits.position = Vector3.Lerp (startPos, endPos, fracJourney);

			yield return new WaitForSeconds (0.05f * Time.deltaTime);
		}
	}
}
