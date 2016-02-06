using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Singleplayer : MonoBehaviour {
	public int playerStartCrystalCount = 1;
	public int minStartAiCrystalCount = 5;
	public int maxStartAiCrystalCount = 9;

	private MasterScript masterScript;
	private SceneHandler sceneHandlerScript;

	private int playerCrystalCount = -1;
	private List<int> aiStartCrystalCount = new List<int> { -1, -1, -1, -1, -1 };
	private List<int> aiCrystalCount = new List<int> { -1, -1, -1, -1, -1 };
	private bool playerStillAllive = true;
	private List<bool> aiStillAlive = new List<bool> { true, true, true, true, true };

	private int round = -1;
	private int match = 0;
	private int winner = -1;
	private bool roundContinues = true;

	void Start () {
		masterScript = GameObject.FindObjectOfType (typeof(MasterScript)) as MasterScript;
		sceneHandlerScript = GameObject.FindObjectOfType (typeof(SceneHandler)) as SceneHandler;

		playerCrystalCount = playerStartCrystalCount;

		round = 1;

		for (int i = 0; i < aiCrystalCount.Count; ++i) {
			aiCrystalCount[i] = Random.Range (minStartAiCrystalCount, maxStartAiCrystalCount);
			aiStartCrystalCount[i] = aiCrystalCount [i];
		}
	}

	public void StartRound(int sceneUL) {
		++match;

		if (match <= 0) {
			roundContinues = true;
		} else {
			roundContinues = false;
		}

		masterScript.SetCharacter (2, match + 1);
		masterScript.SetCrystal(2, Random.Range(1, 3));

		StartCoroutine (sceneHandlerScript.StartGame((int)MasterScript.Scene.gameScene, sceneUL));
	}

	public void EndRound(int scene) {
		++round;

		if (playerStillAllive) {
			StartCoroutine (sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.spMap, scene));
		} else {
			StartCoroutine (sceneHandlerScript.LoadMenu ((int)MasterScript.Scene.mainMenu, scene));
		}
	}

	public void UpdateRound() {
		if (winner == 1) {
			++playerCrystalCount;

			for (int i = 0; i < aiCrystalCount.Count; ++i) {
				--aiCrystalCount[i];
			}
		} else {
			roundContinues = false;

			aiCrystalCount[winner - 2] += 2;
			--playerCrystalCount;

			for (int i = 0; i < aiCrystalCount.Count; ++i) {
				--aiCrystalCount[i];
			}
		}

		if (playerCrystalCount <= 0) playerStillAllive = false;

		for (int i = 0; i < aiStillAlive.Count; ++i) {
			if (aiCrystalCount [i] <= 0) aiStillAlive [i] = false;
		}
	}

	public void SetWinner(int w) {
		winner = w;
	}

	public int GetCrystalCount(int player) {
		if (player == 1) {
			return playerCrystalCount;
		} else {
			return aiCrystalCount [player - 2];
		}
	}

	public int GetStartCrystalCount(int player) {
		if (player == 1) {
			return playerStartCrystalCount;
		} else {
			return aiStartCrystalCount [player - 2];
		}
	}

	public void SetStartCrystalCount(int player, int count) {
		if (player == 1) {
			playerStartCrystalCount = count;
		} else {
			aiStartCrystalCount [player - 2] = count;
		}
	}

	public int GetRound() {
		return round;
	}

	public bool RoundContinues() {
		return roundContinues;
	}

	public void SaveGame() {
		Ini.IniWriteValue ("Player", "playerStartCrystalCount", playerStartCrystalCount.ToString ());
		Ini.IniWriteValue ("Player", "playerCrystalCount", playerCrystalCount.ToString ());
		Ini.IniWriteValue ("Player", "playerStillAllive", playerStillAllive.ToString ());


		for (int i = 0; i < aiStartCrystalCount.Count; ++i) {
			Ini.IniWriteValue ("Ai", "aiStartCrystalCount " + i , aiStartCrystalCount[i].ToString ());
		}

		for (int i = 0; i < aiCrystalCount.Count; ++i) {
			Ini.IniWriteValue ("Ai", "aiCrystalCount " + i , aiCrystalCount[i].ToString ());
		}

		for (int i = 0; i < aiStillAlive.Count; ++i) {
			Ini.IniWriteValue ("Ai", "aiStillAlive " + i , aiStillAlive[i].ToString ());
		}


		Ini.IniWriteValue ("Game", "round", round.ToString ());
		Ini.IniWriteValue ("Game", "match", match.ToString ());
		Ini.IniWriteValue ("Game", "winner", winner.ToString ());
		Ini.IniWriteValue ("Game", "roundContinues", roundContinues.ToString ());
	}

	public void LoadGame() {
		Debug.Log ("Load Start");
		playerStartCrystalCount = int.Parse (Ini.IniReadValue ("Player", "playerStartCrystalCount"));
		playerCrystalCount = int.Parse (Ini.IniReadValue ("Player", "playerCrystalCount"));
		playerStillAllive = bool.Parse (Ini.IniReadValue ("Player", "playerStillAllive"));
		Debug.Log ("Still alive");

		for (int i = 0; i < aiStartCrystalCount.Count; ++i) {
			aiStartCrystalCount[i] = int.Parse (Ini.IniReadValue ("Ai", "aiStartCrystalCount " + i));
		}

		for (int i = 0; i < aiCrystalCount.Count; ++i) {
			aiCrystalCount[i] = int.Parse (Ini.IniReadValue ("Ai", "aiCrystalCount " + i));
		}

		for (int i = 0; i < aiStillAlive.Count; ++i) {
			aiStillAlive[i] = bool.Parse (Ini.IniReadValue ("Ai", "aiStillAlive " + i));
		}


		round = int.Parse (Ini.IniReadValue ("Game", "round"));
		match = int.Parse (Ini.IniReadValue ("Game", "match"));
		winner = int.Parse (Ini.IniReadValue ("Game", "winner"));
		roundContinues = bool.Parse (Ini.IniReadValue ("Game", "roundContinues"));
	}
}
