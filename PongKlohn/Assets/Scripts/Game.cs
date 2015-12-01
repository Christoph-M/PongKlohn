using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Game : MonoBehaviour {
	public Player player1;
	public Player player2;
	public GameObject canvas;

	public float playerSpeed = 15.0f;
	public float dashSpeed = 5.0f;
	public float minBallSpeed = 10.0f;
	public float maxBallSpeed = 100.0f;
	public float ballSpeedUpStep = 5.0f;
	public int maxGameRounds = 3;


	private Transform projectile;
	
	private float ballSpeed;
	private int gameRound = 1;
	private int player1Score = 0;
	private int player2Score = 0;

	void Start() {
		player1.SetInputAxis("HorizontalP1", "VerticalP1", "ShootP1", "BlockP1");
		player2.SetInputAxis("HorizontalP2", "VerticalP2", "ShootP2", "BlockP2");

		ballSpeed = minBallSpeed;

		if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f) {
			player1.setTurn(true);
			player2.setTurn(false);
		} else {
			player1.setTurn(false);
			player2.setTurn(true);
		}
	}
	
	private float timeLeft = 6.0f;
	void Update() {
		if (timeLeft > 0) {
			timeLeft -= Time.deltaTime;

			switch ((int)timeLeft) {
			case 5:
				canvas.transform.FindChild ("Round_" + gameRound).gameObject.SetActive (true);
				break;
			case 4:
				canvas.transform.FindChild ("Round_" + gameRound).gameObject.SetActive (false);
				break;
			case 3:
				canvas.transform.FindChild ("2").gameObject.SetActive (true);
				break;
			case 2:
				canvas.transform.FindChild ("2").gameObject.SetActive (false);
				canvas.transform.FindChild ("1").gameObject.SetActive (true);
				break;
			case 1:
				canvas.transform.FindChild ("1").gameObject.SetActive (false);
				canvas.transform.FindChild ("FIGHT").gameObject.SetActive (true);
				break;
			case 0:
				canvas.transform.FindChild ("FIGHT").gameObject.SetActive (false);
				canvas.transform.FindChild ("Player_1_Score").gameObject.SetActive (true);
				canvas.transform.FindChild ("Player_2_Score").gameObject.SetActive (true);
				
				player1.enabled = true;
				player2.enabled = true;
				break;
			}
		}

		player1.speed = playerSpeed;
		player2.speed = playerSpeed;
		player1.dashSpeed = dashSpeed;
		player2.dashSpeed = dashSpeed;
	}

	void LateUpdate() {
		Debug.Log ("P1: " + player1Score + ". P2: " + player2Score);
		canvas.transform.FindChild ("Player_1_Score").GetComponent<Text> ().text = "" + player1Score;
		canvas.transform.FindChild ("Player_2_Score").GetComponent<Text> ().text = "" + player2Score;
	}
	
	public void SetTurn(bool turn) {
		if (turn){
			player1.setTurn(false);
			player2.setTurn(true);
		} else {
			player1.setTurn(true);
			player2.setTurn(false);
		}
	}

	public void SetProjectileTransform(Transform trans) { projectile = trans; }
	public Transform GetProjectileTransform() { return projectile; }
	
	public void BallSpeedUp(){
		ballSpeed += ballSpeedUpStep;

		if (ballSpeed > maxBallSpeed) ballSpeed = maxBallSpeed;
	}

	public float GetBallSpeed() { return ballSpeed; }
	public void ResetBallSpeed() { ballSpeed = minBallSpeed; }

	public void Player1Scored() { ++player1Score; }
	public void Player2Scored() { ++player2Score; }
}
