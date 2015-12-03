﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MatchUI : UserInterface {

	// Use this for initialization
	void Start () {
		gameScript = GameObject.FindObjectOfType (typeof(Game)) as Game;
	}
	
	private float timeLeft = 6.0f;
	void Update() {
		if (timeLeft > 0) {
			timeLeft -= Time.deltaTime;
			
			this.RoundStart(timeLeft);
		}
	}

	void LateUpdate() {
		canvas.transform.FindChild ("Player_1_Life").GetComponent<Text> ().text = "" + gameScript.player1.health;
		canvas.transform.FindChild ("Player_2_Life").GetComponent<Text> ().text = "" + gameScript.player2.health;
	}
	
	public void RoundEnd(int i){
		canvas.transform.FindChild ("Player_Win").GetComponent<Text> ().text = "Player " + i + " Wins";
		canvas.transform.FindChild ("Player_Win").gameObject.SetActive (true);
		canvas.transform.FindChild ("Player_1_Life").gameObject.SetActive (false);
		canvas.transform.FindChild ("Player_2_Life").gameObject.SetActive (false);
		
		timeLeft = 8.0f;
	}

	private void RoundStart(float t) {
		switch ((int)t) {
		case 6:
			canvas.transform.FindChild ("Player_Win").gameObject.SetActive (false);
			break;
		case 5:
			canvas.transform.FindChild ("Round").GetComponent<Text> ().text = "Round " + gameScript.gameRound;
			canvas.transform.FindChild ("Round").gameObject.SetActive (true);
			break;
		case 4:
			canvas.transform.FindChild ("Round").gameObject.SetActive (false);
			break;
		case 3:
			canvas.transform.FindChild ("Count_Down").GetComponent<Text> ().text = "2";
			canvas.transform.FindChild ("Count_Down").gameObject.SetActive (true);
			break;
		case 2:
			canvas.transform.FindChild ("Count_Down").GetComponent<Text> ().text = "1";
			break;
		case 1:
			canvas.transform.FindChild ("Count_Down").gameObject.SetActive (false);
			canvas.transform.FindChild ("FIGHT").gameObject.SetActive (true);
			break;
		case 0:
			canvas.transform.FindChild ("FIGHT").gameObject.SetActive (false);
			canvas.transform.FindChild ("Player_1_Life").gameObject.SetActive (true);
			canvas.transform.FindChild ("Player_2_Life").gameObject.SetActive (true);

			gameScript.EnablePlayers();

			break;
		}
	}
}
