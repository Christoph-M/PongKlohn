﻿using UnityEngine;
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

	private List<string> playerType = new List<string>{ "KeyP1", "KeyP2" };

	private GameObject projectile;

	void Start () {
		eventSystem = EventSystem.current;

		this.LoadScene (scenes[1]);
	}

	public void LoadScene(string scene, bool setActive = true) {
		SceneManager.LoadScene (scene, LoadSceneMode.Additive);
		if (setActive) StartCoroutine (SetActiveScene (scene));
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

	public enum Scene {
		master = 0,
		startScreen = 1,
		mainMenu = 2,
		optionsMenu = 3,
		characterSelect = 4,
		winScreen = 5,
		gameScene = 6,
		player = 7,
		balls = 8
	}
}
