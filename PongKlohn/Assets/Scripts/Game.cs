using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class Game : MonoBehaviour {

	public Transform[] assets;

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
		playerInput.moveP1(gameMechanics.detectCollisionP1());
		playerInput.moveP2(gameMechanics.detectCollisionP2());
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

[CustomEditor(typeof(Game))]
public class GameEditor : Editor
{
	[Range (1,100)]
	public int sice = 1;
	public Game game = new Game();

	private void OnSceneGUI()
	{		
		game.assets= new Transform[sice];
	}
	
}
