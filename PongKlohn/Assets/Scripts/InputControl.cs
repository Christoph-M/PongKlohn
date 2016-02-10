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
    private string xAxis2 = "x";
    private string yAxis2 = "y";
    private string shoot2 = "s";
    private string block2 = "b";
    private string dash2 = "d";
    private string buff2 = "B";
    private string powerShoot2 = "p";

    bool isAiPlayer = false;
	
	public InputControl(GameObject Player_XX,string playerTyp)
	{
        if (playerTyp == "Ai")
        {
            isAiPlayer = true;
            ai = new AI(Player_XX.transform);
        }
        else
        {
            if (Player_XX.tag == "Player1")
		    {
			    isAiPlayer = false;
			    xAxis = "HorizontalP1";
			    yAxis = "VerticalP1";
			    shoot = "ShootP1";
			    block = "BlockP1";
			    dash = "DashP1";
			    buff = "BuffP1";
			    powerShoot = "PowerShootP1";
                xAxis2 = "ControllerHorizontalP1";
                yAxis2 = "ControllerVerticalP1";
                shoot2 = "ShootControler1";
                block2 = "BlockControler1";
                dash2 = "DashControler1";
                buff2 = "BuffControler1";
                powerShoot2 = "PowerShootControler1";
            }
		
		    else if(Player_XX.tag == "Player2")
		    {
			    isAiPlayer = false;
			    xAxis = "HorizontalP2";
			    yAxis = "VerticalP2";
			    shoot = "ShootP2";
			    block = "BlockP2";
			    dash = "DashP2";
			    buff = "BuffP2";
                xAxis2 = "ControllerHorizontalP2";
                yAxis2 = "ControllerVerticalP2";
                powerShoot = "PowerShootP2";
                shoot2 = "ShootControler2";
                block2 = "BlockControler2";
                dash2 = "DashControler2";
                buff2 = "BuffControler2";
                powerShoot2 = "PowerShootControler2";
            }
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
            if(Input.GetAxis(xAxis2)!=0 || Input.GetAxis(yAxis2)!=0)
            {
                return new Vector2(Input.GetAxisRaw(xAxis2), Input.GetAxisRaw(yAxis2));
            }
			return new Vector2(Input.GetAxisRaw(xAxis),Input.GetAxisRaw(yAxis));
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
            if (Input.GetAxis(xAxis2) != 0 || Input.GetAxis(yAxis2) != 0)
            {
                return new Vector2(Input.GetAxisRaw(xAxis2), Input.GetAxisRaw(yAxis2));
            }
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
            if (Input.GetAxisRaw(block2) != 0)
            {
                return true;
            }
            if (Input.GetAxisRaw(block)!=0)
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
            if (Input.GetAxisRaw(shoot2) != 0)
            {
                return true;
            }
            if (Input.GetAxisRaw(shoot) != 0 && att)
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
            if (Input.GetAxisRaw(buff2) != 0)
            {
                return true;
            }
            if (Input.GetAxisRaw(buff) != 0)
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
            if (Input.GetAxisRaw(dash2) != 0)
            {
                return true;
            }
            if (Input.GetAxisRaw(dash) != 0)
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
			return ai.GetPowerShoot();////////////////
		}
		
		else
		{
            //Debug.Log("player Shoot:"+ att);
            if (Input.GetAxisRaw(powerShoot2) != 0f)
            {
                return true;
            }
            if (Input.GetAxisRaw(powerShoot) != 0f)
			{
				return true;
			}
			return false;
		}
	}

	public bool IsAi() {
		return isAiPlayer;
	}
}
