﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Game : MonoBehaviour {
	[Header("GameObject References")]
	public Player player1;
	public Player player2;
	
	[Header("Game")]
	public int gameRound = 1;
	public int maxGameRounds = 3;

	[Header("Player")]
	public string player1Typ = "Player1"; 
	public string player2Typ = "Player2";
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

	[Header("Ball")]
	public float minBallSpeed = 10.0f;
	public float maxBallSpeed = 100.0f;
	public float ballSpeedUpStep = 5.0f;

	[Header("Timer")]
	public float blockTime = 0.2f;


	private UserInterface uiScript;
	private Transform projectile;

	private const int p1 = 1;
	private const int p2 = 2;

	private float ballSpeed;
	private int player1Score = 0;
	private int player2Score = 0; 

	void Start() {
		uiScript = GameObject.FindObjectOfType (typeof(UserInterface)) as UserInterface;

		player1.SetPlayer(player1Typ);
		player2.SetPlayer(player2Typ);
		player1.health = playerHealth;
		player2.health = playerHealth;
		player1.power = playerEnergy;
		player2.power = playerEnergy;
		

		ballSpeed = minBallSpeed;

		if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f) {
			player1.setTurn(true);
			player2.setTurn(false);
		} else {
			player1.setTurn(false);
			player2.setTurn(true);
		}
	}

	void LateUpdate() {		
		StartCoroutine(UpdatePlayer(playerSpeed, dashSpeed, blockTime, dashEnergyCost));
	}
	
	IEnumerator UpdatePlayer(float playerSpee, float dashSpee, float blockTim, int dashEnergyCos){
		player1.speed = playerSpee;
		player2.speed = playerSpee;
		player1.dashSpeed = dashSpee;
		player2.dashSpeed = dashSpee;
		player1.blockTime = blockTim;
		player2.blockTime = blockTim;
		player1.dashEnergyCost = dashEnergyCos;
		player2.dashEnergyCost = dashEnergyCos;
		
		yield return 0;
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

	public void EnablePlayers(bool b) { player1.enabled = b; 
								  		player2.enabled = b; }

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

	IEnumerator EndRound(int p){
		if (gameRound >= maxGameRounds) {
			int winner = (player1Score > player2Score) ? p1 : p2;
			uiScript.GetComponent<MatchUI> ().MatchEnd (winner);

			this.EnablePlayers(false);

			yield return new WaitForSeconds(5);

			if (winner == 1) {
				Application.LoadLevel(0);
//				Application.LoadLevel(2);
			} else {
				Application.LoadLevel(0);
//				Application.LoadLevel(3);
			}
		} else {
			uiScript.GetComponent<MatchUI> ().RoundEnd (p);
			
			player1.health = playerHealth;
			player2.health = playerHealth;
			
			if (p == 1) {
				this.SetTurn(true);
			} else {
				this.SetTurn(false);
			}

			this.EnablePlayers(false);
			
			++gameRound;
		}
	}
}
