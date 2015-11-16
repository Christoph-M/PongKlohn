using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class Game : MonoBehaviour {
	private PlayerInput playerInput;
	private GameMechanics gameMechanics;

	private bool turn;

	// Use this for initialization
	void Start () {
		playerInput = GetComponent<PlayerInput> ();
		gameMechanics = GetComponent<GameMechanics> ();

		if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f){
			turn = true;
		} else {
			turn = false;
		}
	}

	public void setTurn(bool goal){
		turn = goal;
	}

	void FixedUpdate(){
		playerInput.moveP1();
		playerInput.moveP2();
	}

	// Update is called once per frame
	void Update () {
		if (turn){
			gameMechanics.p1Shoot();
			
			//gameMechanics.detectBallBlockP2();
		} else {
			gameMechanics.p2Shoot();
			
			//gameMechanics.detectBallBlockP1();
		}

		gameMechanics.moveBall ();
	}
}
