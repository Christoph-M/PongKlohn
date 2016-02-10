using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MasterScript : MonoBehaviour {
	public List<GameObject> players;
	public GameObject projectiles;
	[Space(10)]
	public List<string> scenes;
	[Space(10)]
	public List<GameObject> assets;

	public List<AnimationCurve> curves;


	private EventSystem eventSystem;

	private List<int> characters = new List<int>{ 1, 2 };

	private List<int> crystals = new List<int>{ 1, 1 };

	private List<string> playerType = new List<string>{ "ConP1", "ConP2" };

	private GameObject projectile;

	void Start () {
		eventSystem = EventSystem.current;

		this.LoadScene (scenes[(int)Scene.startScreen]);
	}

	public void LoadScene(string scene, bool setActive = true) {
		if (!SceneManager.GetSceneByName (scene).isLoaded) {
			SceneManager.LoadScene (scene, LoadSceneMode.Additive);
			if (setActive) StartCoroutine (SetActiveScene (scene));
		}
	}

	private IEnumerator SetActiveScene(string scene) {
		yield return new WaitUntil(() => SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene)));
	}

	public void UnloadScene(string scene) {
		SceneManager.UnloadScene (scene);
	}

	public void SetCharacter(int player, int character) {
		characters[player - 1] = character;
	}

	public int GetCharacter(int player) {
		return characters [player - 1];
	}

	public void SetCrystal(int player, int crystal) {
		crystals[player - 1] =  crystal;
	}

	public int GetCrystal(int player) {
		return crystals [player - 1];
	}

	public void SetPlayerType(int player, string type) {
		playerType [player - 1] = type;
	}

	public string GetPlayerType(int player) {
		return playerType [player - 1];
	}

//	public static string GetPlayerTypeS(int player) {
//		return playerType [player - 1];
//	}

	public enum Scene {
		master = 0,
		spLoop = 1,
		startScreen = 2,
		mainMenu = 3,
		spMenu = 4,
		optionsMenu = 5,
		howToMenu = 6,
		characterSelect = 7,
		spMap = 8,
		story = 9,
		winScreen = 10,
		loseScreen = 11,
		versusEndScreen = 12,
		gameScene = 13,
		player = 14,
		balls = 15,
		credits = 16
	}
}
