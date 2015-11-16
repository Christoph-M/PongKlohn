using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class Game : MonoBehaviour
{
	public Player player1;
	public Player player2;

	private GameMechanics gameMechanics;

	private bool turn;

	void Start()
	{
		gameMechanics = GetComponent<GameMechanics> ();

		player1.SetInputAxis("HorizontalP1","VerticalP1");
		player2.SetInputAxis("HorizontalP2","VerticalP2");
		
		if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f){
			turn = true;
		} else {
			turn = false;
		}
	}
	
	public void setTurn(bool goal){
		turn = goal;
	}

	void Update()
	{
		if (turn){
			gameMechanics.p1Shoot();
		} else {
			gameMechanics.p2Shoot();
		}
		
		gameMechanics.moveBall ();
	}
}


/*public class Game : MonoBehaviour {
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

		//gameMechanics.moveBall ();
	}
}*/
