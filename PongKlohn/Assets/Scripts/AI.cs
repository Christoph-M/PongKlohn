using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI 
{
	private Game gameScript;
	private Transform ballTransform;
	private Vector2 moveAxis = Vector2.zero;
	private Transform playerTransform;
	private Vector3 resetPosition ;
    private float playerDistance;
    public float minimumPlayerDistanceReset = 30.0f;
    public float maximumPlayerDistanceReset = 35.0f;
    public float minimumPlayerDistance;
    public float maximumPlayerDistance;
    public float playerDistanceJump = 5.0f;
    private float playerBallDistance;
    public float blockBallDistance = 8f;
    private Vector3 nullPosition = Vector3.zero;
    //will be used to stop the characters from trembling by giving worldspace for tolerance
    private float stopTrembling = 0.5f;
    private float ballSpeed =0f;

	private Player character;
    private GameObject leftPlayer;
    private GameObject rightPlayer;

	public enum State {defensiv, agressiv, neutral};
	public static State state;
	private  List<Vector2> bounceArray;
	private Vector2 targetVector = new Vector2(0, 0);
	private Vector2 originVector = new Vector2(0,0);
    private int percentage;
    private int aiStrength;

    public static bool newTargetVectorCountLeft = false;
    public static bool newTargetVectorCountRight = false;
    // Use this for initialization
    public AI (Transform p) 
	{
        minimumPlayerDistance = minimumPlayerDistanceReset;
        maximumPlayerDistance = maximumPlayerDistanceReset;
		playerTransform = p;
		gameScript = GameObject.FindObjectOfType (typeof(Game)) as Game;
		resetPosition = playerTransform.position;
        SetLeftRightPlayer();
        state = State.neutral;
        aiStrength =1; //gameScript.aiStrength;
        percentage = GetPercentageOnX(aiStrength);

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

    private void resetMinMaxPlayerDistance()
    {
            minimumPlayerDistance = minimumPlayerDistanceReset;
            maximumPlayerDistance = maximumPlayerDistanceReset;
    }
	//returns the position of the ball
	private void GetBallTransform()
	{

		ballTransform = gameScript.GetProjectileTransform ();
	}

    private int GetPercentageOnX(int strength)
    {
        

        percentage = (StatePercentage() / 100) * strength;
        return percentage;
    }

    private int StatePercentage()
    {
        if (state == State.agressiv)
            return 80;
        else if (state == State.defensiv)
            return 30;
        else if (state == State.neutral)
            return 55;
        else
            return 0;

    }

    private void SetAiState()
    {
        int aiHealth;
        int enemyHealth;
        int player1life = GameObject.FindGameObjectWithTag("Player1").GetComponent<Player>().health;
        int player2life = GameObject.FindGameObjectWithTag("Player2").GetComponent<Player>().health;
        if (playerTransform == leftPlayer.transform)
        {
            aiHealth = player1life;
            enemyHealth = player2life;
        }
        else
        {
            enemyHealth = player1life;
            aiHealth = player2life;
        }
        if (aiHealth > enemyHealth)
            state = State.agressiv;
        else if (aiHealth < enemyHealth - 25)
            state = State.defensiv;
        else if (aiHealth >= enemyHealth - 25 && aiHealth < enemyHealth)
            state = State.neutral;
    }
    private void CalculateTargetVector ()
	{
        percentage = GetPercentageOnX(aiStrength);
        bounceArray = new List<Vector2>();
        targetVector = new Vector2(0, 0);
        originVector = new Vector2(0, 0);
        Vector2 origin2 = new Vector2(0,0);
        Vector2 target2 = new Vector2(0,0);
        float lengthX = 0f;
        if(ballTransform!= null)
        bounceArray = ballTransform.gameObject.GetComponent<Ball>().GetPath();
        for (int i = 0; i <= (bounceArray.Count - 1); i++)
        {
            if (bounceArray[i] != null)
            { 
               // targetVector = AimTarget(bounceArray[i], originVector, percentage);
                lengthX += AimTarget(bounceArray[i], originVector, percentage, false).x;
                
                
                
            }
            else if (i < (bounceArray.Count - 1))
            {
                if ((playerTransform.position.x <= bounceArray[i].x && playerTransform.position.x >= bounceArray[(i + 1)].x) || (playerTransform.position.x >= bounceArray[i].x && playerTransform.position.x <= bounceArray[(i + 1)].x))
                {
                    originVector = bounceArray[i]; //- bounceArray [i];
                    if (playerTransform == rightPlayer.transform &&  originVector.x < 0f)
                        originVector = AimTarget(bounceArray[i+1], bounceArray[i], percentage, true);
                    else if(playerTransform == leftPlayer.transform && originVector.x > 0f)
                        originVector = AimTarget(bounceArray[i+1], bounceArray[i], percentage, true);
                }
            }
            
        }

        for (int i = 0; i <= (bounceArray.Count - 1); i++)
        {
            if (bounceArray.Count == 1)
            {
                target2 = bounceArray[i];
                originVector = AimTarget(target2,ballTransform.position, percentage, true);
            }
            else {
                if (playerTransform == rightPlayer.transform)
                {
                    if (i < bounceArray.Count - 1)
                    {
                        if (bounceArray[i].x <= lengthX && bounceArray[(i + 1)].x >= lengthX)
                        {
                            Debug.Log("geht rein");
                            target2 = bounceArray[(i + 1)];

                        }
                    }

                }
                else if (playerTransform == leftPlayer.transform)
                {
                    if (i < bounceArray.Count - 1)
                    {
                        if (bounceArray[i].x >= lengthX && bounceArray[(i + 1)].x <= lengthX)
                        {
                            Debug.Log("geht rein");
                            target2 = bounceArray[(i + 1)];

                        }
                    }

                }
            }
        }

        targetVector = AimTarget(target2, originVector, percentage,false);
        ResetUntilTurn();
        Debug.Log(playerTransform.gameObject +" "+ targetVector);



    }

    private void ResetUntilTurn()
    {
         if (playerTransform == leftPlayer.transform)
                {
            if (targetVector.x > Vector2.zero.x)
            {
                targetVector = resetPosition;
            }
                }
                else if (playerTransform == rightPlayer.transform)
                {
                    if (targetVector.x < Vector2.zero.x)
                    {
                        targetVector = resetPosition;
                        Debug.Log("reset right player");
                    }
                }
    }
	private Vector2 AimTarget(Vector2 targetVector, Vector2 originVector, float percentage, bool zeroOnX)
	{
        Vector2 banana = new Vector2();
        
        banana = targetVector - originVector;
      
        float m = banana.y / banana.x;
        
        banana.x = (banana.x / 100) * percentage + originVector.x;

        if (zeroOnX)
            banana.x = 0f + originVector.x;
        
        banana.y = m * banana.x;

        return banana;
            
        
		//mSteigung = targetVector.y / targetVector.x;
		//targetVector.x = 0f;
		//targetVector.y = m * targetVector.x;
	

    }
    private void MoveToTargetVector()
	{
        if (ballTransform == null)
        {
            targetVector = resetPosition;
        }
       
        if (Vector2.Distance(playerTransform.position, targetVector) > stopTrembling)
        {
            if (playerTransform.position.y < targetVector.y)
            {

                moveAxis.y = 1;
            }
            else if (playerTransform.transform.position.y > targetVector.y)
            {
                moveAxis.y = -1;
            }
            else
            {
                moveAxis.y = 0;
            }
            if (playerTransform == rightPlayer.transform)
            {
                if (targetVector.x > rightPlayer.transform.position.x) { moveAxis.x = 1; }
                else if (targetVector.x < rightPlayer.transform.position.x) { moveAxis.x = -1; }
                else { moveAxis.x = 0; }
            }
            else if (playerTransform == leftPlayer.transform)
            {
                if (targetVector.x < leftPlayer.transform.position.x) { moveAxis.x = -1; }
                else if (targetVector.x > rightPlayer.transform.position.x) { moveAxis.x = 1; }
                else { moveAxis.x = 0; }
            }
        }
        else moveAxis = Vector2.zero;
            
        

       
        


    }


	//returns the vector2D-position, the AI is moving to
	public Vector2 GetMovementInput()
	{
            GetBallTransform();
            SetPlayerBallDistance();
            SetPlayerDistance();
            SetPlayerMinMaxDistance();
            SetAiState();


        if (playerTransform == rightPlayer.transform && newTargetVectorCountRight == true)
        {
			CalculateTargetVector ();
            newTargetVectorCountRight = false;
		}
       else if (playerTransform == leftPlayer.transform && newTargetVectorCountLeft == true)
        {
            CalculateTargetVector();
            newTargetVectorCountLeft = false;
        }

        

        MoveToTargetVector();
    	return moveAxis;
	}

    private void ReActiveAi()
    {
        //creates the moving axis for the player depending on the y-coord of the ball (whether to go up or down)                 
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
            //creates the moving axis for the player on the x-axis depending on measuring-result of the player-distance
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
            else
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

            if (playerBallDistance < 8.0f && ((ballTransform.transform.position.y - playerTransform.transform.position.y) > 4.5f) || (ballTransform.transform.position.y - playerTransform.transform.position.y) < -4.5f)
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

            //                if(balltransform.transform.position.x < 0.0f){moveaxis.x = -1;}
            if (playerTransform == rightPlayer.transform)
            {
                if (ballTransform.transform.position.x > rightPlayer.transform.position.x) { moveAxis.x = 1; }
            }
            else if (playerTransform == leftPlayer.transform)
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
        resetMinMaxPlayerDistance();
        SetballSpeed();
        if(ballSpeed > 10f)
        {
            minimumPlayerDistance += playerDistanceJump;
            maximumPlayerDistance += playerDistanceJump;
        }
        else if(ballSpeed >= 20.0f)
        {
            minimumPlayerDistance += playerDistanceJump;
            maximumPlayerDistance += playerDistanceJump;
        }
        else if(ballSpeed >= 30.0f)
        {
            minimumPlayerDistance += playerDistanceJump;
            maximumPlayerDistance += playerDistanceJump;
        }
        else if (ballSpeed >= 40.0f)
        {
            minimumPlayerDistance += playerDistanceJump;
            maximumPlayerDistance += playerDistanceJump;
        }
        else if (ballSpeed >= 50.0f)
        {
            minimumPlayerDistance += playerDistanceJump;
            maximumPlayerDistance += playerDistanceJump;
        }

        //assigns the left and right player by tag
    }
    public void SetLeftRightPlayer()
    {
        rightPlayer = GameObject.FindGameObjectWithTag("Player2");
        leftPlayer = GameObject.FindGameObjectWithTag("Player1");
        
    }


	//option: whether the AI can attack or not
	public bool GetAttack(bool isAttacking)
	{
		
		if (isAttacking) {return true;}
		else{return false;}
	}

	// whether the AI can block or not (includes Dashing)
	public bool GetBlock()
	{
		GetBallTransform();
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
