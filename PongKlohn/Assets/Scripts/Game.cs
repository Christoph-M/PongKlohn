﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Game : MonoBehaviour {
	[Header("Gameobject References")]
	public Player player1;
	public Player player2;
	public GameObject canvas;
	
	[Header("Game")]
	public int maxGameRounds = 3;
	[Header("Player")]
	public float playerSpeed = 15.0f;
	public float dashSpeed = 5.0f;
	public int playerHealth = 100;
	public int playerDamage = 10;
	[Header("Ball")]
	public float minBallSpeed = 10.0f;
	public float maxBallSpeed = 100.0f;
	public float ballSpeedUpStep = 5.0f;


	private Transform projectile;
	
	private float ballSpeed;
	private int gameRound = 1;

	void Start() {
		player1.SetPlayer("Player1");
		player2.SetPlayer("ai");
		player1.health = playerHealth;
		player2.health = playerHealth;

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
				canvas.transform.FindChild ("Round").GetComponent<Text> ().text = "Round " + gameRound;
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
		canvas.transform.FindChild ("Player_1_Life").GetComponent<Text> ().text = "" + player1.health;
		canvas.transform.FindChild ("Player_2_Life").GetComponent<Text> ().text = "" + player2.health;
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

	public void Player1Scored() {
		if (player2.health > 0) {
			player2.health -= playerDamage;
		}
		
		if (player2.health <= 0) {
			player2.health = 0;
			
			canvas.transform.FindChild ("Player_Win").GetComponent<Text> ().text = "Player 1 Wins";
			canvas.transform.FindChild ("Player_Win").gameObject.SetActive (true);
			
			player1.enabled = false;
			player2.enabled = false;

			++gameRound;
		}
	}

	public void Player2Scored() {
		if (player1.health > 0) {
			player1.health -= playerDamage;
		}
		
		if (player1.health <= 0) {
			player1.health = 0;
			
			canvas.transform.FindChild ("Player_Win").GetComponent<Text> ().text = "Player 2 Wins";
			canvas.transform.FindChild ("Player_Win").gameObject.SetActive (true);
			
			player1.enabled = false;
			player2.enabled = false;
			
			++gameRound;
		}
	}
}
