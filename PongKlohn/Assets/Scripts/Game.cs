using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
	[Header("GameObject References")]
	public List<GameObject> charactersP1 = new List<GameObject>();
	public List<GameObject> charactersP2 = new List<GameObject>();
	[Space(10)]
	public MatchUI uiScript;
	public ScreenShake screenShakeScript;
	
	[Header("Game")]
	public int gameRound = 1;
	public int maxGameRounds = 3;

	[Header("Player")]
	public Vector3 player1Spawn = new Vector3(-21.0f, 0.0f, -0.23f);
	public Vector3 player2Spawn = new Vector3(21.0f, 0.0f, -0.23f);
	[Space(10)]
	public float playerSpeed = 15.0f;
	public float dashSpeed = 5.0f;
	public int dashEnergyCost = 10;
	[Space(10)]
	public int playerHealth = 100;
	public int playerDamage = 10;
	public int wallDamage = 1;
	[Space(10)]
	public int playerEnergy = 0;
	public int maxPlayerEnergy = 100;
	public int energyGain = 10;
	[Header("AI")]
	public int aiStrength = 50;

	[Header("Ball")]
	public float minBallSpeed;
	public float maxBallSpeed;
	public AnimationCurve ballSpeedUpCurve = AnimationCurve.EaseInOut(0.0f, 10.0f, 1.0f, 100.0f);
	public float ballSpeedUpStep = 5.0f;
	public float catchSpeedDec = 2.0f;

	[Header("Timer")]
	public float blockTime = 0.2f;


	private MasterScript masterScript;
	private SceneHandler sceneHandlerScript;
	private Transform projectile;

	private Player player1;
	private Player player2;

	private const int p1 = 1;
	private const int p2 = 2;

	private float ballSpeed;
	private float ballSpeedAtTime;
	private int player1Score = 0;
	private int player2Score = 0;

	void Awake() {
		masterScript = GameObject.FindObjectOfType (typeof(MasterScript)) as MasterScript;
		sceneHandlerScript = GameObject.FindObjectOfType (typeof(SceneHandler)) as SceneHandler;

		StartCoroutine (SpawnGameObjects ());
		

		minBallSpeed = ballSpeedUpCurve.Evaluate (0.0f);
		maxBallSpeed = ballSpeedUpCurve.Evaluate (1.0f);
		ballSpeed = minBallSpeed;
	}

	void LateUpdate() {		
		StartCoroutine(UpdatePlayer(playerSpeed, dashSpeed, blockTime, dashEnergyCost));
	}
	
	IEnumerator UpdatePlayer(float playerSpeed, float dashSpeed, float blockTime, int dashEnergyCost){
		player1.speed = playerSpeed;
		player2.speed = playerSpeed;
		player1.dashSpeed = dashSpeed;
		player2.dashSpeed = dashSpeed;
		player1.blockTime = blockTime;
		player2.blockTime = blockTime;
		player1.dashEnergyCost = dashEnergyCost;
		player2.dashEnergyCost = dashEnergyCost;
		
		yield return 0;
	}
	
	public void SetTurn(bool turn) {
//		if (turn){
//			player1.setTurn(false);
//			player2.setTurn(true);
//		} else {
//			player1.setTurn(true);
//			player2.setTurn(false);
//		}
	}

	public void SetProjectileTransform(Transform trans) { projectile = trans; AI.SetNewTargetVectorCount (); }
	public Transform GetProjectileTransform() { return projectile; }

	public void BallSpeedUp(){
		ballSpeedAtTime += ballSpeedUpStep / (maxBallSpeed - minBallSpeed);
		ballSpeed = ballSpeedUpCurve.Evaluate(ballSpeedAtTime);

		if (ballSpeed > maxBallSpeed) {
			ballSpeed = maxBallSpeed;
			ballSpeedAtTime = 1.0f;
		}
	}

	public float GetBallSpeed() { return ballSpeed; }

	public void DecreaseBallSpeed() {
		ballSpeedAtTime -= catchSpeedDec / (maxBallSpeed - minBallSpeed);
		ballSpeed -= catchSpeedDec;

		if (ballSpeed < minBallSpeed) {
			ballSpeed = minBallSpeed;
			ballSpeedAtTime = 0.0f;
		}
	}

	public void ResetBallSpeed() { ballSpeed = minBallSpeed; ballSpeedAtTime = 0.0f; }

	public Player GetPlayer(int player) {
		if (player == 1) {
			return player1;
		} else {
			return player2;
		}
	}

	public void EnablePlayers(bool b) { player1.enabled = b; 
								  		player2.enabled = b; }

	private bool enableProjectile = false;
	public void EnableProjectile() { enableProjectile = true; }

	public void Player1Scored(bool isWall) {
		if (player2.health > 0) {
			if (isWall) {
				player2.health -= wallDamage;
			} else {
				player2.health -= playerDamage;
			}
		}
		
		if (player2.health <= 0) {
			player2.health = 0;
			
			++player1Score;

			StartCoroutine(this.EndRound(p1));
		}
	}

	public void Player2Scored(bool isWall) {
		if (player1.health > 0) {
			if (isWall) {
				player1.health -= wallDamage;
			} else {
				player1.health -= playerDamage;
			}
		}
		
		if (player1.health <= 0) {
			player1.health = 0;

			++player2Score;

			StartCoroutine(this.EndRound(p2));
		}
	}

	public void Player1AddEnergy() {
		if (player1.power < maxPlayerEnergy) {
			player1.power += energyGain;
		} 

		if (player1.power >= maxPlayerEnergy) {
			player1.power = maxPlayerEnergy;
		}
	}

	public void Player2AddEnergy() {
		if (player2.power < maxPlayerEnergy) {
			player2.power += energyGain;
		} 

		if (player2.power >= maxPlayerEnergy) {
			player2.power = maxPlayerEnergy;
		}
	}

	public void ShakeScreen(int type = -1, int p = -1) {
		if (screenShakeScript) {
			switch (type) {
			case 0:
				screenShakeScript.BlockScreenShake (p);
				break;
			case 1:
				screenShakeScript.GoalScreenShake ();
				break;
			case 2:
				screenShakeScript.BounceScreenShake ();
				break;
			case 3:
				screenShakeScript.SpecialScreenShake ();
				break;
			case 4:
				screenShakeScript.BuffScreenShake ();
				break;
			default:
				break;
			}
		}
	}


	private IEnumerator SpawnGameObjects() {
		int charP1 = masterScript.GetCharacter (1) - 1;
		int charP2 = masterScript.GetCharacter (2) - 1;

		// Wait until player scene is active
		yield return new WaitUntil(() => SceneManager.SetActiveScene(SceneManager.GetSceneByName(masterScript.scenes[(int)MasterScript.Scene.player])));

		GameObject p1 = Instantiate (charactersP1 [charP1], player1Spawn, new Quaternion ()) as GameObject;
		GameObject p2 = Instantiate (charactersP2 [charP2], player2Spawn, new Quaternion (0.0f, 0.0f, 180.0f, 0.0f)) as GameObject;
		Transform pEmpty = GameObject.FindGameObjectWithTag ("PlayerEmpty").transform;

		player1 = p1.GetComponent<Player> ();
		player2 = p2.GetComponent<Player> ();

		player1.tag = "Player1";
		player2.tag = "Player2";
		player1.name = "Player_01";
		player2.name = "Player_02";
		player1.transform.SetParent (pEmpty);
		player2.transform.SetParent (pEmpty);
		player2.InvertMotion = true;
		Debug.Log ("1: " + masterScript.GetPlayerType (1) + ", 2: " + masterScript.GetPlayerType (2));
		player1.SetPlayer(masterScript.GetPlayerType(1));
		player2.SetPlayer(masterScript.GetPlayerType(2));

		player1.health = playerHealth;
		player2.health = playerHealth;
		player1.power = playerEnergy;
		player2.power = playerEnergy;

		// Wait until balls scene is active
		yield return new WaitUntil(() => SceneManager.SetActiveScene(SceneManager.GetSceneByName(masterScript.scenes[(int)MasterScript.Scene.balls])));

		GameObject projectiles = Instantiate (masterScript.projectiles, Vector3.zero, Quaternion.identity) as GameObject;

		projectiles.name = "Projectile";

		if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f) {
			projectiles.tag = "BallP1";

			float angle = UnityEngine.Random.Range (0.0f, 45.0f);
			projectiles.transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0,0,1));
		} else {
			projectiles.tag = "BallP2";

			float angle = UnityEngine.Random.Range (180.0f, 225.0f);
			projectiles.transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0,0,1));
		}

		// Wait until game scene is active
		yield return new WaitUntil(() => SceneManager.SetActiveScene(SceneManager.GetSceneByName(masterScript.scenes[(int)MasterScript.Scene.gameScene])));

		this.enabled = true;
		uiScript.enabled = true;

		yield return new WaitUntil (() => enableProjectile);

		projectiles.GetComponent<Move> ().enabled = true;
		projectiles.GetComponent<Ball> ().enabled = true;
	}

	private IEnumerator EndRound(int p){
		if (gameRound >= maxGameRounds) {
			int winner = (player1Score > player2Score) ? p1 : p2;
			uiScript.GetComponent<MatchUI> ().MatchEnd (winner);

			this.EnablePlayers (false);

			StartCoroutine(sceneHandlerScript.EndGame ((int)MasterScript.Scene.winScreen));

			yield return 0;

//			if (winner == 1) {
//				masterScript.LoadScene (1);
//
//				yield return new WaitUntil(() => SceneManager.GetSceneAt(1).isLoaded);
//
//				masterScript.UnloadScene (scene);
//			} else {
//				masterScript.LoadScene (1);
//
//				yield return new WaitUntil(() => SceneManager.GetSceneAt(1).isLoaded);
//
//				masterScript.UnloadScene (scene);
//			}
		} else {
			uiScript.GetComponent<MatchUI> ().RoundEnd (p);
			
			player1.health = playerHealth;
			player2.health = playerHealth;
			
			if (p == 1) {
				this.SetTurn (true);
			} else {
				this.SetTurn (false);
			}

			this.EnablePlayers (false);
			
			++gameRound;
		}
	}
}
