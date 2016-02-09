using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneHandler : MonoBehaviour {

	private MasterScript masterScript;

	void Start() {
		masterScript = this.GetComponent<MasterScript> ();
	}

	public IEnumerator LoadMenu(int menuL, int menuUL)
	{
		masterScript.LoadScene (this.GetScene(menuL));

		yield return new WaitUntil(() => SceneManager.GetSceneByName(this.GetScene(menuL)).isLoaded);

		masterScript.UnloadScene (this.GetScene(menuUL));
	}

	public IEnumerator StartGame(int sceneL, int sceneUL, bool unloadFirst = false) {
		if (unloadFirst) {
			masterScript.UnloadScene (this.GetScene (sceneUL));

			yield return new WaitUntil(() => !SceneManager.GetSceneByName(this.GetScene(sceneL)).isLoaded);
		}

		masterScript.LoadScene (this.GetScene(sceneL), false);
		masterScript.LoadScene (this.GetScene((int)MasterScript.Scene.player), false);
		masterScript.LoadScene (this.GetScene((int)MasterScript.Scene.balls), false);

		yield return new WaitUntil(() => SceneManager.GetSceneByName(this.GetScene(sceneL)).isLoaded && 
										 SceneManager.GetSceneByName(this.GetScene((int)MasterScript.Scene.player)).isLoaded &&
										 SceneManager.GetSceneByName(this.GetScene((int)MasterScript.Scene.balls)).isLoaded);

		if (!unloadFirst) masterScript.UnloadScene (this.GetScene(sceneUL));
	}

	public IEnumerator EndGame(int scene) {
		masterScript.LoadScene (this.GetScene(scene));

		yield return new WaitUntil(() => SceneManager.GetSceneByName(this.GetScene(scene)).isLoaded);

		masterScript.UnloadScene (this.GetScene((int)MasterScript.Scene.gameScene));
		masterScript.UnloadScene (this.GetScene((int)MasterScript.Scene.player));
		masterScript.UnloadScene (this.GetScene((int)MasterScript.Scene.balls));
	}
	
	public IEnumerator StartSingleplayer(int sceneL, int sceneUL) {
		masterScript.LoadScene (this.GetScene(sceneL), false);
		masterScript.LoadScene (this.GetScene ((int)MasterScript.Scene.spMenu));

		yield return new WaitUntil(() => SceneManager.GetSceneByName(this.GetScene (sceneL)).isLoaded &&
										 SceneManager.GetSceneByName(this.GetScene ((int)MasterScript.Scene.spMenu)).isLoaded);

		masterScript.UnloadScene (this.GetScene (sceneUL));
	}

	public string GetScene(int i) {
		return masterScript.scenes [i];
	}
}
