using UnityEngine;
using System.Collections;

public class InputControl
{
	AI ai;
	private string xAxis = "x";
	private string yAxis = "y";
	private string shoot = "s";
	private string block = "b";
	private string dash  = "d";
	
	bool isAiPlayer = false;
	
	public InputControl(string player,Transform playerTransform)
	{
		if(player == "Player1")
		{
			isAiPlayer = false;
			xAxis = "HorizontalP1";
			yAxis = "VerticalP1";
			shoot = "ShootP1";
			dash = "BlockP1";
		}
		else if(player == "Player2")
		{
			isAiPlayer = false;
			xAxis = "HorizontalP2";
			yAxis = "VerticalP2";
			shoot = "ShootP2";
			dash = "BlockP2";
		}
		else if(player == "Ai")
		{
			isAiPlayer =true;
			ai = new AI(playerTransform);
		}
		else
		{
			isAiPlayer = true;
			ai = new AI(playerTransform);
		}
	}
	
	public Vector2 UpdateMovement () 
	{
		if(isAiPlayer)
		{
			return ai.UpdateMovementInput();///////////////////////
		}
		
		else
		{
			return Vector2(Input.GetAxis(xAxis),Input.GetAxis(yAxis));
		}
	}
	
	public bool IsBlockKeyActive()
	{
		if(isAiPlayer)
		{
			return ai.feuern;////////////////
		}
		
		else
		{
			if(Input.GetAxis(dash)!=0)
			{
				return true;
			}
			return false;
		}
	}
	
	public bool IsFireKeyActive()
	{
		if(isAiPlayer)
		{
			return ai.feuern;////////////////
		}
		
		else
		{
			if(Input.GetAxis(shoot) =! 0)
			{
				return true;
			}
			return false;
		}
	}	
}
