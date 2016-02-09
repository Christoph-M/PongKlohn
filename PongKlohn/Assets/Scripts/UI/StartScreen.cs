using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class StartScreen : MonoBehaviour {
	public GameObject startScreen;
	public Image backgroundVideo;

	public float textPulseDuration = 1.0f;
	

	private MasterScript masterScript;
	private SceneHandler sceneHandlerScript;

	private MovieTexture video;

	private Image eightBall;
	private Image ga;
	private Image pressStart;
	private Image backplane;
	
	private float t = 0.0f;
	private bool one = false;
	
	void Start() {
		masterScript = GameObject.FindObjectOfType (typeof(MasterScript)) as MasterScript;
		sceneHandlerScript = GameObject.FindObjectOfType (typeof(SceneHandler)) as SceneHandler;

		video = (MovieTexture)backgroundVideo.material.mainTexture;

		eightBall = startScreen.transform.FindChild("8Ball").GetComponent<Image>();
		ga = startScreen.transform.FindChild("Game_Academy").GetComponent<Image>();
		pressStart = startScreen.transform.FindChild("Press_Start").GetComponent<Image>();

		StartCoroutine(this.StartUp ());
	}
	
	void Update () {
		if (Input.anyKeyDown /*&& pressStart.gameObject.activeSelf*/) {
			StartCoroutine(sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.mainMenu, (int)MasterScript.Scene.startScreen));
		}

	}

	private IEnumerator PressStart() {
		video.Play ();
		GetComponent<AudioSource> ().Play ();

		while (true) {
			t = 0.0f;

			while (t <= textPulseDuration / 1.2f) {
				pressStart.CrossFadeAlpha (255, textPulseDuration / 2, false);

				t += Time.deltaTime;

				yield return new WaitForSeconds (0.05f * Time.deltaTime);
			}

			t = 0.0f;

			while (t <= textPulseDuration / 1.2f) {
				pressStart.CrossFadeAlpha (0, textPulseDuration / 2, false);

				t += Time.deltaTime;

				yield return new WaitForSeconds (0.05f * Time.deltaTime);
			}

			yield return new WaitForSeconds (0.05f * Time.deltaTime);
		}
	}

	private IEnumerator StartUp() {
		yield return new WaitForSeconds (0.2f);
		t = 0.0f;

		while (t <= 1.0f) {
			eightBall.CrossFadeAlpha (255, 1.0f, false);

			t += Time.deltaTime;

			yield return new WaitForSeconds (0.05f);
		}

		yield return new WaitForSeconds (1.0f);
		t = 0.0f;

		while (t <= 0.5f) {
			eightBall.CrossFadeAlpha (0, 0.5f, false);

			t += Time.deltaTime;

			yield return new WaitForSeconds (0.05f);
		}

		eightBall.gameObject.SetActive (false);
		ga.gameObject.SetActive (true);
		t = 0.0f;

		while (t <= 1.0f) {
			ga.CrossFadeAlpha (255, 1.0f, false);

			t += Time.deltaTime;

			yield return new WaitForSeconds (0.05f);
		}

		t = 0.0f;
		yield return new WaitForSeconds (0.7f);

		while (t <= 0.5f) {
			ga.CrossFadeAlpha (0, 0.5f, false);

			t += Time.deltaTime;

			yield return new WaitForSeconds (0.05f);
		}

		ga.gameObject.SetActive (false);
		pressStart.gameObject.SetActive (true);
		backgroundVideo.gameObject.SetActive (true);

		StartCoroutine (this.PressStart ());
	}
}
