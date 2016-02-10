using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MatchUI : MonoBehaviour {
	public GameObject matchUI;
	public GameObject pauseUI;


	private MasterScript masterScript;
	private SceneHandler sceneHandlerScript;
	private Singleplayer singleplayerScript;

	private EventSystem eventSystem;

	private Game gameScript;

	private Image healthBarP1;
	private Image healthBarP2;
	private Image energyBarP1;
	private Image energyBarP2;

	private float hp1;
	private float hp2;
	private float ep1;
	private float ep2;

	private bool singleplayer;

	void Awake () {
		masterScript = GameObject.FindObjectOfType (typeof(MasterScript)) as MasterScript;
		sceneHandlerScript = GameObject.FindObjectOfType (typeof(SceneHandler)) as SceneHandler;

		singleplayer = (masterScript.GetPlayerType (2) == "Ai") ? true : false;
		if (singleplayer) singleplayerScript = GameObject.FindObjectOfType (typeof(Singleplayer)) as Singleplayer;

		eventSystem = EventSystem.current;

		gameScript = GameObject.FindObjectOfType (typeof(Game)) as Game;

		healthBarP1 = matchUI.transform.FindChild ("Health_P1").FindChild ("healthbar").GetComponent<Image> ();
		healthBarP2 = matchUI.transform.FindChild ("Health_P2").FindChild ("healthbar").GetComponent<Image> ();
		energyBarP1 = matchUI.transform.FindChild ("Energy_P1").FindChild ("specialbar").GetComponent<Image> ();
		energyBarP2 = matchUI.transform.FindChild ("Energy_P2").FindChild ("specialbar").GetComponent<Image> ();
	}
	
	private Timer uiTimer = new Timer(8.0f);
	float t = 0.0f;
	void Update() {
		t = uiTimer.UpdateTimer ();
		if (t >= 0.0f) {
			this.RoundStart (t);
		} else {
			if (Input.GetKeyDown (KeyCode.Escape)) this.PauseGame ();
		}
	}

	void LateUpdate() {
		hp1 = (float)((float)gameScript.GetPlayer (1).health / (float)gameScript.playerHealth);
		hp2 = (float)((float)gameScript.GetPlayer (2).health / (float)gameScript.playerHealth);
		ep1 = (float)((float)gameScript.GetPlayer (1).power / (float)gameScript.maxPlayerEnergy);
		ep2 = (float)((float)gameScript.GetPlayer (2).power / (float)gameScript.maxPlayerEnergy);

		healthBarP1.fillAmount = hp1;
		healthBarP2.fillAmount = hp2;
		energyBarP1.fillAmount = ep1;
		energyBarP2.fillAmount = ep2;
	}
	
	public void RoundEnd(int p){
		matchUI.transform.FindChild ("Player_Win").GetComponent<Text> ().text = "Player " + p + " Wins\nRound " + gameScript.gameRound + "!";
		matchUI.transform.FindChild ("Player_Win").gameObject.SetActive (true);
		matchUI.transform.FindChild ("Health_P1").gameObject.SetActive (false);
		matchUI.transform.FindChild ("Health_P2").gameObject.SetActive (false);
		matchUI.transform.FindChild ("Energy_P1").gameObject.SetActive (false);
		matchUI.transform.FindChild ("Energy_P2").gameObject.SetActive (false);
		
		uiTimer.SetTimer(11.0f);
	}

	public void MatchEnd(int p){
		matchUI.transform.FindChild ("Player_Win").GetComponent<Text> ().text = "Player " + p + " Wins Match!";
		matchUI.transform.FindChild ("Player_Win").gameObject.SetActive (true);
		matchUI.transform.FindChild ("Health_P1").gameObject.SetActive (false);
		matchUI.transform.FindChild ("Health_P2").gameObject.SetActive (false);
		matchUI.transform.FindChild ("Energy_P1").gameObject.SetActive (false);
		matchUI.transform.FindChild ("Energy_P2").gameObject.SetActive (false);
	}

	public void Continue() {
		this.PauseGame ();
	}

	public void Rematch() {
		StartCoroutine (sceneHandlerScript.StartGame ((int)MasterScript.Scene.gameScene, (int)MasterScript.Scene.gameScene, true));
	}

	public void MainMenu() {
		StartCoroutine (sceneHandlerScript.EndGame ((int)MasterScript.Scene.mainMenu));
	}

	public void SaveAndExit() {
		StartCoroutine (sceneHandlerScript.EndGame ((int)MasterScript.Scene.mainMenu));
	}

	private void RoundStart(float t) {
		switch ((int)t) {
		case 8:
			matchUI.transform.FindChild ("Player_Win").gameObject.SetActive (false);
			break;
		case 7:
			matchUI.transform.FindChild ("BackplaneR").gameObject.SetActive (true);
			matchUI.transform.FindChild ("Round_" + gameScript.gameRound).gameObject.SetActive(true);
			break;
		case 6:
			matchUI.transform.FindChild ("BackplaneR").gameObject.SetActive (false);
			matchUI.transform.FindChild ("Round_" + gameScript.gameRound).gameObject.SetActive (false);
			break;
		case 5:
			matchUI.transform.FindChild ("Backplane").gameObject.SetActive (true);
			matchUI.transform.FindChild ("2").gameObject.SetActive (true);
			break;
		case 4:
			matchUI.transform.FindChild ("2").gameObject.SetActive (false);
			matchUI.transform.FindChild ("1").gameObject.SetActive (true);
			break;
		case 3:
			matchUI.transform.FindChild ("Backplane").gameObject.SetActive (false);
			matchUI.transform.FindChild ("1").gameObject.SetActive (false);
			break;
		case 2:
			gameScript.ShakeScreen (1);
			matchUI.transform.FindChild ("Backplane").gameObject.SetActive (true);
			matchUI.transform.FindChild ("FIGHT").gameObject.SetActive (true);
			break;
		case 1:
			matchUI.transform.FindChild ("Backplane").gameObject.SetActive (false);
			matchUI.transform.FindChild ("FIGHT").gameObject.SetActive (false);
			matchUI.transform.FindChild ("Health_P1").gameObject.SetActive (true);
			matchUI.transform.FindChild ("Health_P2").gameObject.SetActive (true);
			matchUI.transform.FindChild ("Energy_P1").gameObject.SetActive (true);
			matchUI.transform.FindChild ("Energy_P2").gameObject.SetActive (true);

			gameScript.EnablePlayers (true);
			gameScript.EnableProjectile (true);

			break;
		}
	}

	private void PauseGame() {
		matchUI.SetActive (!matchUI.activeSelf);
		pauseUI.SetActive (!pauseUI.activeSelf);
		gameScript.EnablePlayers (!pauseUI.activeSelf);
		gameScript.GetProjectileTransform ().GetComponent<Move> ().enabled = !pauseUI.activeSelf;
		gameScript.GetProjectileTransform ().GetComponent<Ball> ().enabled = !pauseUI.activeSelf;

		if (singleplayer && pauseUI.activeSelf) {
			pauseUI.transform.FindChild ("Main_Menu").gameObject.SetActive (false);
			pauseUI.transform.FindChild ("Rematch").gameObject.SetActive (false);
			pauseUI.transform.FindChild ("Save_And_Exit").gameObject.SetActive (true);
		} else if (pauseUI.activeSelf) {
			pauseUI.transform.FindChild ("Save_And_Exit").gameObject.SetActive (false);
			pauseUI.transform.FindChild ("Rematch").gameObject.SetActive (true);
			pauseUI.transform.FindChild ("Main_Menu").gameObject.SetActive (true);
		}
	}
}
