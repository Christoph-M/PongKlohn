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
	private string buff  = "B";
	private string powerShoot = "p";
	
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
			dash = "DashP1";
			buff = "BuffP1";
			powerShoot = "PowerShootP1";
		}
		else if(player == "Player2")
		{
			isAiPlayer = false;
			xAxis = "HorizontalP2";
			yAxis = "VerticalP2";
			shoot = "ShootP2";
			block = "BlockP2";
			dash = "DashP2";
			buff = "BuffP2";
			powerShoot = "PowerShootP2";
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
	
	public Vector2 UpdateMovementRaw () 
	{
		//Debug.Log("jea");
		if(isAiPlayer)
		{
			return ai.GetMovementInput();///////////////////////
		}
		
		else
		{
			//Debug.Log("YippiJea");
			return new Vector2(Input.GetAxisRaw(xAxis),Input.GetAxisRaw(yAxis));
		}
	}
	
	
	public bool IsActionKeyActive(bool canShoot)
	{
		if(isAiPlayer)
		{
			if(!ai.GetBlock() || !ai.GetAttack(canShoot))
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
			Debug.Log("AI Shoot:"+ att);
			return ai.GetAttack(att);////////////////
		}
		
		else
		{
			//Debug.Log("player Shoot:"+ att);
			if(Input.GetAxis(shoot) != 0 && att)
			{
				return true;
			}
			return false;
		}
	}
	
	public bool IsBuffActive()
	{
		if(isAiPlayer)
		{
			Debug.Log("AI Shoot:");
			return ai.GetBuff();////////////////
		}
		
		else
		{
			//Debug.Log("player Shoot:"+ att);
			if(Input.GetAxis(buff))
			{
				return true;
			}
			return false;
		}
	}
	
	public bool IsDashActive()
	{
		if(isAiPlayer)
		{
			Debug.Log("AI Dash:"+ att);
			return ai.GetDash();////////////////
		}
		
		else
		{
			//Debug.Log("player Shoot:"+ att);
			if(Input.GetAxis(dash))
			{
				return true;
			}
			return false;
		}
	}
	
	public bool IsPowerShootActive()
	{
		if(isAiPlayer)
		{
			Debug.Log("AI Power Shoot:");
			return ai.GetAttack();////////////////
		}
		
		else
		{
			//Debug.Log("player Shoot:"+ att);
			if(Input.GetAxis(powerShoot))
			{
				return true;
			}
			return false;
		}
	}
}
