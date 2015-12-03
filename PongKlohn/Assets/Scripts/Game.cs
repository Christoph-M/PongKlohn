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
	public float playerSpeed = 15.0f;
	public float dashSpeed = 5.0f;
	public int playerHealth = 100;
	public int playerDamage = 10;
	[Header("Ball")]
	public float minBallSpeed = 10.0f;
	public float maxBallSpeed = 100.0f;
	public float ballSpeedUpStep = 5.0f;

	
	private UserInterface uiScript;
	private Transform projectile;

	private const int p1 = 1;
	private const int p2 = 2;

	private float ballSpeed;
	private int player1Score = 0;
	private int player2Score = 0;

	void Start() {
		uiScript = GameObject.FindObjectOfType (typeof(UserInterface)) as UserInterface;

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

	void LateUpdate() {		
		player1.speed = playerSpeed;
		player2.speed = playerSpeed;
		player1.dashSpeed = dashSpeed;
		player2.dashSpeed = dashSpeed;
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

	public void Player1Scored() {
		if (player2.health > 0) {
			player2.health -= playerDamage;
		}
		
		if (player2.health <= 0) {
			player2.health = 0;
			
			++player1Score;
			
			this.EndRound(p1);
		}
	}

	public void Player2Scored() {
		if (player1.health > 0) {
			player1.health -= playerDamage;
		}
		
		if (player1.health <= 0) {
			player1.health = 0;

			++player2Score;

			this.EndRound(p2);
		}
	}

	public void EndRound(int p){
		if (gameRound >= maxGameRounds) {
			uiScript.GetComponent<MatchUI> ().MatchEnd (p);

			this.EnablePlayers(false);

			if (p == 1) {
//				Application.LoadLevel(2);
			} else {
//				Application.LoadLevel(3);
			}
		} else {
			uiScript.GetComponent<MatchUI> ().RoundEnd (p);
			
			player1.health = playerHealth;
			player2.health = playerHealth;

			this.EnablePlayers(false);
			
			++gameRound;
		}
	}
}
