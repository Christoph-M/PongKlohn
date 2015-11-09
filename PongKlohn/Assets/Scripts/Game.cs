using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class Game : MonoBehaviour {
	private PlayerInput playerInput;
	private GameMechanics gameMechanics;

	private string triggeredGoal;

	// Use this for initialization
	void Start () {
		playerInput = GetComponent<PlayerInput> ();
		gameMechanics = GetComponent<GameMechanics> ();

		if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f){
			triggeredGoal = "Goal_Red";
		} else {
			triggeredGoal = "Goal_Blue";
		}
	}

	public void setTriggeredGoal(string goal){
		triggeredGoal = goal;
	}

	void FixedUpdate(){
		playerInput.moveP1();
		playerInput.moveP2();
	}

	// Update is called once per frame
	void Update () {
		if (triggeredGoal == "Goal_Red"){
			gameMechanics.p1Shoot();
			
			gameMechanics.detectBallBlockP2();
		} else {
			gameMechanics.p2Shoot();
			
			gameMechanics.detectBallBlockP1();
		}

		gameMechanics.moveBall ();
	}
}
