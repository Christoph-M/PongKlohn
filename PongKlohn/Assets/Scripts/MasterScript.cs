using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MasterScript : MonoBehaviour {
	private List<int> characters = new List<int>();

	private List<int> crystals = new List<int>();

	private int activeMenu = 1;
	private bool inMatch = false;

	void Start () {
		this.LoadScene (1);
	}

	public void LoadScene(int scene) {
		SceneManager.LoadScene (scene, LoadSceneMode.Additive);
		StartCoroutine (SetActiveScene (scene));
	}

	private IEnumerator SetActiveScene(int scene) {
		yield return new WaitUntil(() => SceneManager.SetActiveScene(SceneManager.GetSceneAt(1)));
	}

	public void UnloadScene(int scene) {
		SceneManager.UnloadScene (scene);
	}

	public void SetCharacter(int character) {
		characters.Add (character);
	}

	public int GetCharacter(int player) {
		return characters [player - 1];
	}

	public void SetCrystal(int crystal) {
		crystals.Add (crystal);
	}

	public int GetCrystal(int player) {
		return crystals [player - 1];
	}

	public void SetActiveMenu(int menu) {
		activeMenu = menu;
	}

	public int GetActiveMenu() {
		return activeMenu;
	}

	public void SetInMatch(bool active) {
		inMatch = active;
	}

	public bool GetInMatch() {
		return inMatch;
	}
}
