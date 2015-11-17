using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class Game : MonoBehaviour {
	public Player player1;
	public Player player2;

	public float setPlayerSpeed = 15.0f;
	public float setBallSpeed   = 10.0f;
	public float setBallSpawnDistance = 1.2f;
	public static float playerSpeed = 15.0f;
	public static float ballSpeed   = 10.0f;
	public static float ballSpawnDistance = 1.2f;

	private bool turn;

	void Start() {
		player1.SetInputAxis("HorizontalP1", "VerticalP1", "ShootP1", "BlockP1");
		player2.SetInputAxis("HorizontalP2", "VerticalP2", "ShootP2", "BlockP2");
		
		if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f) {
			turn = player1.setTurn(true);
			player2.setTurn(false);
		} else {
			turn = player1.setTurn(false);
			player2.setTurn(true);
		}
	}
	
	public void setTurn(bool goal) {
		if (turn){
			turn = player1.setTurn(false);
			player2.setTurn(true);
		} else {
			turn = player1.setTurn(true);
			player2.setTurn(false);
		}
	}

	void Update() {
		player1.moveBall ();
		player2.moveBall ();
	}
	
	void LateUpdate(){
		playerSpeed = setPlayerSpeed;
		ballSpeed = setBallSpeed;
		ballSpawnDistance = setBallSpawnDistance;
	}
}
