using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MatchUI : UserInterface {
	void Start () {
		gameScript = GameObject.FindObjectOfType (typeof(Game)) as Game;
	}
	
	private Timer uiTimer = new Timer(7.0f);
	void Update() {
		this.RoundStart(uiTimer.UpdateTimer());
	}

	void LateUpdate() {
		canvas.transform.FindChild ("Player_1_Life").GetComponent<Text> ().text = "" + gameScript.player1.health;
		canvas.transform.FindChild ("Player_2_Life").GetComponent<Text> ().text = "" + gameScript.player2.health;
		canvas.transform.FindChild ("Player_1_Energy").GetComponent<Text> ().text = "" + gameScript.player1.power;
		canvas.transform.FindChild ("Player_2_Energy").GetComponent<Text> ().text = "" + gameScript.player2.power;
	}
	
	public void RoundEnd(int p){
		canvas.transform.FindChild ("Player_Win").GetComponent<Text> ().text = "Player " + p + " Wins\nRound " + gameScript.gameRound + "!";
		canvas.transform.FindChild ("Player_Win").gameObject.SetActive (true);
		canvas.transform.FindChild ("Player_1_Life").gameObject.SetActive (false);
		canvas.transform.FindChild ("Player_2_Life").gameObject.SetActive (false);
		canvas.transform.FindChild ("Player_1_Energy").gameObject.SetActive (false);
		canvas.transform.FindChild ("Player_2_Energy").gameObject.SetActive (false);
		
		uiTimer.SetTimer(10.0f);
	}

	public void MatchEnd(int p){
		canvas.transform.FindChild ("Player_Win").GetComponent<Text> ().text = "Player " + p + " Wins Match!";
		canvas.transform.FindChild ("Player_Win").gameObject.SetActive (true);
		canvas.transform.FindChild ("Player_1_Life").gameObject.SetActive (false);
		canvas.transform.FindChild ("Player_2_Life").gameObject.SetActive (false);
		canvas.transform.FindChild ("Player_1_Energy").gameObject.SetActive (false);
		canvas.transform.FindChild ("Player_2_Energy").gameObject.SetActive (false);
	}

	private void RoundStart(float t) {
		switch ((int)t) {
		case 7:
			canvas.transform.FindChild ("Player_Win").gameObject.SetActive (false);
			break;
		case 6:
			canvas.transform.FindChild ("Round").GetComponent<Text> ().text = "Round " + gameScript.gameRound;
			canvas.transform.FindChild ("Round").gameObject.SetActive (true);
			break;
		case 5:
			canvas.transform.FindChild ("Round").gameObject.SetActive (false);
			break;
		case 4:
			canvas.transform.FindChild ("Count_Down").GetComponent<Text> ().text = "2";
			canvas.transform.FindChild ("Count_Down").gameObject.SetActive (true);
			break;
		case 3:
			canvas.transform.FindChild ("Count_Down").GetComponent<Text> ().text = "1";
			break;
		case 2:
			canvas.transform.FindChild ("Count_Down").gameObject.SetActive (false);
			canvas.transform.FindChild ("FIGHT").gameObject.SetActive (true);
			break;
		case 1:
			canvas.transform.FindChild ("FIGHT").gameObject.SetActive (false);
			canvas.transform.FindChild ("Player_1_Life").gameObject.SetActive (true);
			canvas.transform.FindChild ("Player_2_Life").gameObject.SetActive (true);
			canvas.transform.FindChild ("Player_1_Energy").gameObject.SetActive (true);
			canvas.transform.FindChild ("Player_2_Energy").gameObject.SetActive (true);

			gameScript.EnablePlayers(true);

			break;
		}
	}
}
