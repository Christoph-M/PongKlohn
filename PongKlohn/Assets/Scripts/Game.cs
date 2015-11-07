using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class Game : MonoBehaviour 
{

	public Transform[] assets;
	
	public Transform figure1;
	public Transform figure2;
	public Transform sphereO;
	public Transform sphereC;
	//public Collider goal_red;
	//public Collider goal_blue;

	
	
	public float playerSpeed = 5;
	public float ballSpeed = 30;
	public float collisionDistance = 0.6f;

	public Player player1;
	Player player2;
	Ball projectile;
	//Level goal_Red;
	//Level goal_Blue;

	string triggeredGoal;

	// Use this for initialization
	void Start () {
		
		player1 = new Player (figure1);
		player2 = new Player (figure2);
		//goal_Red = new Level (goal_red);
		//goal_Blue = new Level (goal_blue);

		if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f){
			triggeredGoal = "Goal_Red";
		} else {
			triggeredGoal = "Goal_Blue";
		}
	}

	public void setTriggeredGoal(string goal){
		triggeredGoal = goal;
	}
	
	// Update is called once per frame
	void Update () 
	{
		RaycastHit hit;

		if (Input.anyKey) {
			if (Input.GetKey ("w") && !Physics.Raycast(player1.GetProjectilePositin(), Vector3.forward, out hit, collisionDistance)) {	// Player 1 Hoch
				player1.Move (Vector3.forward, playerSpeed);
			}

			if (Input.GetKey ("s") && !Physics.Raycast(player1.GetProjectilePositin(), Vector3.back, out hit, collisionDistance)) {	// Player 1 Runter
				player1.Move (Vector3.back, playerSpeed);
			}

			if (Input.GetKey ("a") && !Physics.Raycast(player1.GetProjectilePositin(), Vector3.left, out hit, collisionDistance)) {	// Player 1 Links
				player1.Move (Vector3.left, playerSpeed);
			}

			if (Input.GetKey ("d") && !Physics.Raycast(player1.GetProjectilePositin(), Vector3.right, out hit, collisionDistance)) {	// Player 1 Rechts
				player1.Move (Vector3.right, playerSpeed);
			}
			
			

			if (Input.GetKey ("up") && !Physics.Raycast(player2.GetProjectilePositin(), Vector3.forward, out hit, collisionDistance)) {	// Player 2 Rechts
				player2.Move (Vector3.forward, playerSpeed);
			}
		
			if (Input.GetKey ("down") && !Physics.Raycast(player2.GetProjectilePositin(), Vector3.back, out hit, collisionDistance)) {	// Player 2 Runter
				player2.Move (Vector3.back, playerSpeed);
			}
		
			if (Input.GetKey ("left") && !Physics.Raycast(player2.GetProjectilePositin(), Vector3.left, out hit, collisionDistance)) {	// Player 2 Links
				player2.Move (Vector3.left, playerSpeed);
			}
		
			if (Input.GetKey ("right") && !Physics.Raycast(player2.GetProjectilePositin(), Vector3.right, out hit, collisionDistance)) {	// Player 2 Rechts
				player2.Move (Vector3.right, playerSpeed);
			}
			
			if (triggeredGoal == "Goal_Red"){
				Quaternion angle = Quaternion.identity;
				angle = figure1.rotation;
				
				if (Input.GetKeyDown("space") && !sphereC){
					if(Input.GetKey("w"))
					{
						angle=Quaternion.AngleAxis(45.0f, Vector3.up);
					}
					if(Input.GetKey("s"))
					{
						angle=Quaternion.AngleAxis(135.0f, Vector3.up);
					}
					
					//angle += player1.GetProjectileRotation();
					sphereC = Instantiate(sphereO, figure1.position, angle) as Transform;

					sphereC.name = "Projectile";
					projectile = new Ball (sphereC);
				}
				
				if (Input.GetKeyDown("right shift") && Physics.Raycast(player2.GetProjectilePositin(), Vector3.left, out hit, 2)){
					triggeredGoal = "Goal_Blue";
					Vector3 direction = Vector3.Reflect(sphereC.forward, figure2.InverseTransformVector(Vector3.forward));
					sphereC.rotation = Quaternion.LookRotation(direction);
				}
			} else {
				Quaternion angle = Quaternion.identity;
				angle = figure2.rotation;
				
				if (Input.GetKeyDown("right ctrl") && !sphereC){
					if(Input.GetKey("up"))
					{
						angle=Quaternion.AngleAxis(-45.0f, Vector3.up);
					}
					if(Input.GetKey("down"))
					{
						angle=Quaternion.AngleAxis(-135.0f, Vector3.up);
					}
					
					//angle += player2.GetProjectileRotation();
					sphereC = Instantiate(sphereO, figure2.position, angle) as Transform;

					sphereC.name = "Projectile";
					projectile = new Ball (sphereC);
				}
				
				if (Input.GetKeyDown("left shift") && Physics.Raycast(player1.GetProjectilePositin(), Vector3.right, out hit, 2)){
					triggeredGoal = "Goal_Red";
					Vector3 direction = Vector3.Reflect(sphereC.forward, figure1.InverseTransformVector(Vector3.forward));
					sphereC.rotation = Quaternion.LookRotation(direction);
				}
			}
		}

		if (sphereC) {
			projectile.move (ballSpeed);
		}
		
		
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
