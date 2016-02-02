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
		if(player == "KeyP1")
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
		
		else if(player == "KeyP2")
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
		
		else if(player == "ConP1")
		{
			isAiPlayer = false;
			xAxis = "ControllerHorizontalP1";
			yAxis = "ControllerVerticalP1";
			shoot = "ShootControler1";
			block = "BlockControler1";
			dash = "DashControler1";
			buff = "BuffControler1";
			powerShoot = "PowerShootControler1";
		}
		
		else if(player == "ConP2")
		{
			isAiPlayer = false;
			xAxis = "ControllerHorizontalP2";
			yAxis = "ControllerVerticalP2";
			shoot = "ShootControler2";
			block = "BlockControler2";
			dash = "DashControler2";
			buff = "BuffControler2";
			powerShoot = "PowerShootControler2";
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
	
	public bool IsBlockKeyActive()
	{
		if(isAiPlayer)
		{
			return ai.GetBlock();////////////////
		}
		
		else
		{
			if(Input.GetAxisRaw(block)!=0)
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
			//Debug.Log("player Shoot:"+ att);
			if(Input.GetAxisRaw(shoot) != 0 && att)
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
			return ai.GetBuff();////////////////
		}
		
		else
		{
			//Debug.Log("player Shoot:"+ att);
			if(Input.GetAxisRaw(buff) != 0)
			{
                //Debug.Log("buff is active funst: "+ Input.GetAxisRaw(buff));
				return true;
			}
			return false;
		}
	}
	
	public bool IsDashActive()
	{
		if(isAiPlayer)
		{
			return ai.GetDash();////////////////
		}
		
		else
		{
			//Debug.Log("player Shoot:"+ att);
			if(Input.GetAxisRaw(dash) != 0)
			{
				return true;
			}
			return false;
		}
	}
	
	public bool IsPowerShootActive(bool att)
	{
		if(isAiPlayer)
		{
			return ai.GetAttack(att);////////////////
		}
		
		else
		{
			//Debug.Log("player Shoot:"+ att);
			if(Input.GetAxisRaw(powerShoot) != 0f && att)
			{
				return true;
			}
			return false;
		}
	}
}
