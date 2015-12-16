using UnityEngine;
using System.Collections;

public class AI 
{
	private Game gameScript;
	private Transform ballTransform;
	private Vector2 moveAxis = Vector2.zero;
	private Transform playerTransform;
	private Vector3 resetPosition ;
    private float playerDistance;
    public float minimumPlayerDistance = 12.0f;
    public float maximumPlayerDistance = 18.0f;
    private float playerBallDistance;
    public float blockBallDistance = 3.75f;
    private Vector3 nullPosition = Vector3.zero;
    //will be used to stop the characters from trembling by giving worldspace for tolerance
    private float stopTrembling = 0.35f;
    private float ballSpeed;

	private Player character;
    private GameObject leftPlayer;
    private GameObject rightPlayer;
	// Use this for initialization
	public AI (Transform p) 
	{
		playerTransform = p;
		gameScript = GameObject.FindObjectOfType (typeof(Game)) as Game;
		resetPosition = playerTransform.position;
        SetLeftRightPlayer();
        

	}
	
    
    private void SetballSpeed()
    {
        if (ballTransform != null)
        {
            ballSpeed = gameScript.GetBallSpeed();
        }
        else
        {
            ballSpeed = gameScript.minBallSpeed;

        }
    }
	//returns the position of the ball
	private void GetBallPosition()
	{

		ballTransform = gameScript.GetProjectileTransform ();
	}

	//returns the vector2D-position, the AI is moving to
	public Vector2 GetMovementInput()
	{
            SetPlayerMinMaxDistance();
            SetPlayerDistance();
            SetPlayerBallDistance();

			//creates the moving Axis for the player depending on the y-coord of the ball (whether to go up or down)                 
			if (ballTransform != null) 
            {
                if (playerTransform.transform.position.y < (ballTransform.transform.position.y - stopTrembling)) 
                {
				    moveAxis.y = 1;
			    }
                else if (playerTransform.transform.position.y > (ballTransform.transform.position.y + stopTrembling))
                {
				    moveAxis.y = -1;
			    }
			}
            
            else 
            {
                if (playerTransform.transform.position.y < (resetPosition.y - stopTrembling)) 
                {
				moveAxis.y = 1;
			    }
                else if (playerTransform.transform.position.y > (resetPosition.y + stopTrembling))
                {
				moveAxis.y = -1;
			    }
			    else
                {
                    moveAxis.y = 0;
                }
            }

           
           
            //manipulates the moving axis on the x-axis to reach critical balls
            if (ballTransform != null)
            {
                if (playerBallDistance < 8.0f && ((ballTransform.transform.position.y - playerTransform.transform.position.y) > 3.5f) || (ballTransform.transform.position.y - playerTransform.transform.position.y) < -3.5f)
                {
				    if(playerTransform == rightPlayer.transform)
				    {
					    moveAxis.x = 1;
				    }
				    else if(playerTransform== leftPlayer.transform)
				    {
					    moveAxis.x = -1;
				    }
                }
                else
                {
                    //creates the moving Axis for the player on the x-axis depending on measuring-result of the player-distance
                    if (Vector3.Distance(playerTransform.position, nullPosition) > 5.0f)
                    {
                        if (playerDistance < minimumPlayerDistance)
                        {
                            if (playerTransform == rightPlayer.transform)
                            {
                                moveAxis.x = 1;
                            }
                            else if (playerTransform == leftPlayer.transform)
                            {
                                moveAxis.x = -1;
                            }
                        }
                        else if (playerDistance > maximumPlayerDistance)
                        {
                            if (playerTransform == rightPlayer.transform)
                            {
                                moveAxis.x = -1;
                            }
                            else if (playerTransform == leftPlayer.transform)
                            {
                                moveAxis.x = 1;
                            }
                        }

                        else { moveAxis.x = 0; }
                    }
                }
 //                if(ballTransform.transform.position.x < 0.0f){moveAxis.x = -1;}
                if(playerTransform == rightPlayer.transform)
                {
                    if (ballTransform.transform.position.x > rightPlayer.transform.position.x) { moveAxis.x = 1; }
                }
                else if(playerTransform== leftPlayer.transform)
                {
                    if (ballTransform.transform.position.x < leftPlayer.transform.position.x) { moveAxis.x = -1; }
                }
            
                   
            
            }  
            
            else
            {
                if (playerTransform.transform.position.x < (resetPosition.x - stopTrembling))
                {
                    moveAxis.x = 1;
                }
                else if (playerTransform.transform.position.x > (resetPosition.x + stopTrembling))
                {
                    moveAxis.x = -1;
                }
                else
                {
                    moveAxis.x = 0;
                }
            }
           
		return moveAxis;
	}

    private void SetPlayerBallDistance()
    {
        if (ballTransform != null)
        {
            playerBallDistance = Vector3.Distance(playerTransform.transform.position, ballTransform.transform.position);
        }
    }

    //calculates the distance between the to players
    private void SetPlayerDistance()
    {  
        playerDistance = Vector3.Distance(leftPlayer.transform.position,  rightPlayer.transform.position);
    }

    
    private void SetPlayerMinMaxDistance()
    {
        SetballSpeed();
        if(ballSpeed <=10)
        {
            minimumPlayerDistance = 12.0f;
            maximumPlayerDistance = 16.0f;
        }
        else if(ballSpeed > 10 && ballSpeed <= 20.0f)
        {
            minimumPlayerDistance = 15.0f;
            maximumPlayerDistance = 19.0f;
        }
        else if(ballSpeed > 20.0f && ballSpeed < 30.0f)
        {
            minimumPlayerDistance = 18.0f;
            maximumPlayerDistance = 22.0f;
        }
        else if(ballSpeed >= 30.0f && ballSpeed < 40.0f)
        {
            minimumPlayerDistance = 20.0f;
            maximumPlayerDistance = 24.0f;
        }
        else if (ballSpeed >= 40.0f && ballSpeed < 50.0f)
        {
            minimumPlayerDistance = 23.0f;
            maximumPlayerDistance = 27.0f;
        }
        else if (ballSpeed >= 50.0f)
        {
            minimumPlayerDistance = 25.0f;
            maximumPlayerDistance = 30.0f;
        }

        //assigns the left and right player by tag
    }public void SetLeftRightPlayer()
    {
        rightPlayer = GameObject.FindGameObjectWithTag("Player2");
        leftPlayer = GameObject.FindGameObjectWithTag("Player1");
        
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
		GetBallPosition();
		if(ballTransform!=null )
		{ 
			if (Vector3.Distance (ballTransform.position, playerTransform.transform.position) < blockBallDistance)
            {
				return true;
			}
		} 

		return false;
	}

    public bool GetDash()
    {
        if (ballTransform != null)
        {
            if (playerTransform == leftPlayer.transform)
            {
                if (playerTransform.position.x + 2.0f <= ballTransform.position.x && Vector3.Distance(playerTransform.position, ballTransform.position) >= blockBallDistance)
                {
                    return true;
                }
                
            }
            

            else if (playerTransform == rightPlayer.transform)
            {
                if (playerTransform.position.x + -2.0f >= ballTransform.position.x && Vector3.Distance(playerTransform.position, ballTransform.position) >= blockBallDistance)
                {
                    return true;
                }
                
            }
           
        }

        return false;   
    }

    public bool GetBuff()
    {
        return true;
    }


       
    
}
