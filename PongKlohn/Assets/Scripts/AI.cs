using UnityEngine;
using System.Collections;

public class AI 
{
	private Game gameScript;
	private Transform ballPosition;
	private Vector2 moveAxis = Vector2.zero;
	private Transform player;
	private Vector3 resetPosition ;
	private Player character;
	// Use this for initialization
	public AI (Transform p) 
	{
		player = p;
		gameScript = GameObject.FindObjectOfType (typeof(Game)) as Game;
		resetPosition = player.position;
		//ballPosition = new Transform();
	}
	

	//returns the position of the ball
	private void GetBallPosition()
	{
		ballPosition = gameScript.GetProjectileTransform ();
	}

	//returns the vector2D-position, the AI is moving to
	public Vector2 GetMovementInput()
	{	
		//creates the moving Axis for the player depending on the y-coord of the ball (whether to go up or down)
		if (ballPosition != null) {
			if (player.transform.position.y < (ballPosition.transform.position.y - 0.35f)) 
			{
				moveAxis.y = 1;
			} else if (player.transform.position.y > (ballPosition.transform.position.y + 0.35f)) 
			{
				moveAxis.y = -1;
			}
		} 
		else 
		{
			if (player.transform.position.y < (resetPosition.y - 0.35f)) 
			{
				moveAxis.y = 1;
			} else if (player.transform.position.y > (resetPosition.y + 0.35f)) 
			{
				moveAxis.y = -1;
			}
			else{moveAxis.y = 0;}
		}
		return moveAxis;
	} 




	//option: whether the AI can attack or not
	public bool GetAttack(bool isAttacking)
	{
		GetBallPosition ();
		if (isAttacking) {return true;}
		else{return false;}
	}

	// whether the AI can block or not (includes Dashing)
	public bool GetBlock()
	{
		GetBallPosition ();
		if(ballPosition!=null )
		{
			if (Vector3.Distance (ballPosition.position, player.transform.position) < 5) {
				return true;
			}
		} 

		return false;
	}
}
