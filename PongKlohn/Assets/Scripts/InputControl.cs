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
			block = "BlockP1";
		}
		else if(player == "Player2")
		{
			isAiPlayer = false;
			xAxis = "HorizontalP2";
			yAxis = "VerticalP2";
			shoot = "ShootP2";
			block = "BlockP2";
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
		//Debug.Log("jea");
		if(isAiPlayer)
		{
			return ai.GetMovementInput();///////////////////////
		}
		
		else
		{
			//Debug.Log("YippiJea");
			return new Vector2(Input.GetAxis(xAxis),Input.GetAxis(yAxis));
		}
	}
	public bool IsActionKeyActive()
	{
		if(isAiPlayer)
		{
			if(!ai.GetBlock() || !ai.GetAttack(true))
			{
				return true;
			}
		}
		else
		{
			if(Input.GetAxis(block)!= 0 || Input.GetAxis(shoot) != 0)
			{
				return true;
			}
		}
	
		return false;
	}
	
	public bool IsBlockKeyActive()
	{
		if(isAiPlayer)
		{
			return ai.GetBlock();////////////////
		}
		
		else
		{
			if(Input.GetAxis(block)!=0)
			{
				return true;
			}
			return false;
		}
	}
	
	public bool IsFireKeyActive(bool att)
	{
		if(isAiPlayer)
		{
			return ai.GetAttack(att);////////////////
		}
		
		else
		{
			if(Input.GetAxis(shoot) != 0 && att)
			{
				return true;
			}
			return false;
		}
	}	
}
