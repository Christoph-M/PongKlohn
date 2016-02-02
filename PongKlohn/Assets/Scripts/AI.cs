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
    public float blockBallDistance = 6f;
    private Vector3 nullPosition = Vector3.zero;
    //will be used to stop the characters from trembling by giving worldspace for tolerance
    private float stopTrembling = 1.75f;
    private float ballSpeed =0f;

	private Player character;
    private GameObject leftPlayer;
    private GameObject rightPlayer;
    private Player enemyPlayer;

	public enum State {defensiv, agressiv, neutral, lastSave};
	public State state;

    public enum AIType {Mastermind, MixUp, ReActive}

	//private  List<Vector2> bounceArray;
	private Vector2 targetVector = new Vector2(0, 0);
	private Vector2 originVector = new Vector2(0,0);
    private float percentage;
    private float aiStrength;

    private bool buffAsk = true;
    //public bool boolAsk;

    
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
        SetEnemyPlayer();
        state = State.neutral;
		aiStrength = (float)gameScript.aiStrength;
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

    private float GetPercentageOnX(float strength)
    {
       // Debug.Log("strength" + strength);
        float percent;
        if (strength > 100)
            strength = 100;

		percent = StatePercentage()* (strength/100f);
        //Debug.Log("percent: " + percent);
        return percent;
    }

    private float StatePercentage()
    {
        if (state == State.agressiv)
            return 25f;
        else if (state == State.defensiv)
            return 50;
        else if (state == State.neutral)
            return 80f;
        else if (state == State.lastSave)
            return 100f;
        else
        {
            Debug.Log("Cannot read State");
            return 0f;
        }

    }

    public static void SetNewTargetVectorCount()
    {
        newTargetVectorCountRight = true;
        newTargetVectorCountLeft = true;
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

        List<Vector2> bounceList;
        
        Vector2 origin2 = new Vector2(0,0);
        Vector2 target2 = new Vector2(0,0);
		bounceList = ballTransform.gameObject.GetComponent<Ball>().GetPath();
       
        Debug.Log("bouncearray.length: " + bounceList.Count);
        float lengthX = 0f;

            foreach (Vector2 target in bounceList)
            {
                target2 = target;
            //Debug.Log("target: " + target);
            }
        //Debug.Log("State Percentage" + StatePercentage());
        //Debug.Log("Percentage: " + percentage);
		//Debug.Log("target2: " + target2);
		lengthX = target2.x * (percentage/100f);

        
        //Debug.Log("lengthX: " + lengthX);

            for (int i = 0; i < (bounceList.Count - 1); i++)
            {
				if (lengthX < 0f) {
					if (bounceList [i + 1] != null) {
						if (bounceList [i + 1].x <= lengthX && bounceList [i].x >= lengthX) {
							target2 = bounceList [(i + 1)];
							origin2 = bounceList [i];
						}
					}
				} else if (lengthX > 0f) {
					//Debug.Log ("er geht rein");
					if (bounceList [(i + 1)] != null) {
						//Debug.Log ("bounceList[i+1] != null");
						if (bounceList [(i + 1)].x >= lengthX && bounceList [i].x <= lengthX) {
							target2 = bounceList [(i + 1)];
							origin2 = bounceList [i];
							//Debug.Log ("korrekter Origin!");
							//Debug.Log ("target2: " + target2 + " origin2: " + origin2);
	                        
						}
					}
				} 
				else {
					Debug.Log ("Which Player?!");
				}
			}
        
        #region alter Code

        /*

                        for (int i = 0; i < bounceArray.Count; i++) {

                        Debug.Log (bounceArray [i]);
                            // targetVector = AimTarget(bounceArray[i], originVector, percentage);

                            if (i < (bounceArray.Count - 1)) {
                                target2 = bounceArray [i + 1];
                                    originVector = bounceArray [i]; //- bounceArray [i];
                                    if (playerTransform == rightPlayer.transform && originVector.x < 0f && target2.x > 0f)
                                        originVector = AimTarget (bounceArray [i + 1], bounceArray [i], percentage, true);
                                    else if (playerTransform == leftPlayer.transform && originVector.x > 0f && target2.x < 0f)
                                        originVector = AimTarget (bounceArray [i + 1], bounceArray [i], percentage, true);


                            }
                            if ((playerTransform == leftPlayer.transform && originVector.x <= 0f) || (playerTransform == rightPlayer.transform && originVector.x >= 0f))
                                lengthX += AimTarget (target2, originVector, percentage, false).x;


                        }

                        for (int i = 0; i <= (bounceArray.Count - 1); i++) {
                            if (i < bounceArray.Count - 1) {
                                if ((bounceArray [i].x <= lengthX && bounceArray [(i + 1)].x >= lengthX) || (bounceArray [i].x >= lengthX && bounceArray [(i + 1)].x <= lengthX)) {
                                    target2 = bounceArray [i + 1];
                                    originVector = bounceArray [i]; //- bounceArray [i];

                                    if (playerTransform == rightPlayer.transform && originVector.x < 0f)
                                        originVector = AimTarget (bounceArray [i + 1], bounceArray [i], percentage, true);
                                    else if (playerTransform == leftPlayer.transform && originVector.x > 0f)
                                        originVector = AimTarget (bounceArray [i + 1], bounceArray [i], percentage, true);

                                }
                            }

                        }
                }

            */
        #endregion

       
            targetVector =  AimTarget(target2, origin2, bounceList, lengthX);
            
            //Debug.Log(playerTransform.gameObject + " " + targetVector + " vor Resett");
            ResetUntilTurn();
            //Debug.Log(playerTransform.gameObject + " " + targetVector + " nach Resett");



        }

    private void ResetUntilTurn()
    {
		if (playerTransform == leftPlayer.transform)
    	{
            if (targetVector.x >= Vector2.zero.x)
            {
				targetVector =  resetPosition;
            }
        }
		else if (playerTransform == rightPlayer.transform)
        {
            if (targetVector.x <= Vector2.zero.x)
            {
                targetVector = resetPosition;
               
            }
        }
    }

    private Vector2 AimTarget(Vector2 targetVector, Vector2 originVector, List<Vector2> bounce, float fixX)
    {
        Debug.Log("Targetvector" + targetVector + " OriginVector " + originVector);
        Vector2 difference;
        //Vector2 sD;
        Vector2 final;
        if ((int)originVector.y == (int)targetVector.y)
        {
            final.y = targetVector.y;
            final.x = fixX;
        }
        
        else
        {
            difference = targetVector - originVector;

            float m = difference.y / difference.x;
            float b = originVector.y - m * originVector.x;

            final.x = fixX;
            final.y = m * final.x + b;
			if (fixX < 0)
				final.x -= 1.15f;
			else if (fixX > 0)
				final.x += 1.15f;
			else
				Debug.Log ("fixX == 0");

            #region alter code
            //sD.x = fixX - originVector.x;
            //sD.y = sD.x * m;
            //final = sD + originVector;
            /* if(bounce.Length <= 2)
             {
                 difference.x = fixX;
                 difference.y = difference.x * m;


             }
             */

            //difference.y = (((difference.x - targetVector.x) / 100) * percentage) * m;
          
           // difference.x = fixX;
            //difference.y = difference.x * m + b;

            /*
             if (playerTransform == rightPlayer.transform)
             {
                 if (originVector.x >= 0)
                     final = difference + originVector;
                 else
                     final = difference;

             }
             else
             {
                 if (originVector.x <= 0)
                     final = difference + originVector;
                 else
                     final = difference;
             }
             */
       
           /* final = difference + originVector;
            if (bounce.Count <= 2)
            {
                final.x *= -1;
            }
            */
            #endregion
            #region alter code
            /*if (zeroOnX) {

                if (originVector.y > targetVector.y )
                    difference.y = m * targetVector.x * -1;
                else
                    difference.y = m * targetVector.x;
                difference.x = 0 ;

            } else {
                difference.x = (difference.x / 100) * percentage;

                if (originVector.y > targetVector.y )
                    difference.y = m * difference.x * -1;
                else 
                    difference.y = m * difference.x;
            }

                Debug.Log ("banana: " + difference);
                if (originVector.y == targetVector.y)
                    difference.y = targetVector.y;
                difference.x = (targetVector.x / 100) * percentage;
                */
            #endregion
        }
      //  Debug.Log("final: " + final);
        return final;
    }

    private void MoveToTargetVector()
	{
        if (ballTransform == null)
        {
            targetVector = resetPosition;
        }
        else {
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
                    if (targetVector.x > rightPlayer.transform.position.x)
                    {
                        moveAxis.x = 1;
                    }
                    else if (targetVector.x < rightPlayer.transform.position.x)
                    {
                        moveAxis.x = -1;
                    }
                    else {
                        moveAxis.x = 0;
                    }
                }
                else if (playerTransform == leftPlayer.transform)
                {
                    if (targetVector.x < leftPlayer.transform.position.x)
                    {
                        moveAxis.x = -1;
                    }
                    else if (targetVector.x > rightPlayer.transform.position.x)
                    {
                        moveAxis.x = 1;
                    }
                    else {
                        moveAxis.x = 0;
                    }
                }
            }
            else moveAxis = Vector2.zero;
        }
    }

    private void MasterMindAi()
    {

		if (playerTransform == rightPlayer.transform && newTargetVectorCountRight == true && ballTransform != null)
        {
			CalculateTargetVector ();
            newTargetVectorCountRight = false;
		}
		else if (playerTransform == leftPlayer.transform && newTargetVectorCountLeft == true && ballTransform != null)
        {
            CalculateTargetVector();
            
            newTargetVectorCountLeft = false;
        }
    }

    private void MMGetBallBehind()
    {
        if (state == State.agressiv && (ballTransform.position.x > character.transform.position.x || ballTransform.position.x < character.transform.position.x))
        {
            state = State.neutral;
            CalculateTargetVector();
        }
        else if (state == State.neutral && (ballTransform.position.x > character.transform.position.x || ballTransform.position.x < character.transform.position.x))
        {
            state = State.defensiv;
            CalculateTargetVector();
        }
        else if (state == State.defensiv && (ballTransform.position.x > character.transform.position.x || ballTransform.position.x < character.transform.position.x))
        {
            state = State.lastSave;
            CalculateTargetVector();
        }
        

        
    }
	//returns the vector2D-position, the AI is moving to
	public Vector2 GetMovementInput()
	{
        
            GetBallTransform();
            SetPlayerBallDistance();
            SetPlayerDistance();
            SetPlayerMinMaxDistance();
            SetAiState();
            
            MasterMindAi();
            //MMGetBallBehind();
            MoveToTargetVector();
        
       /* if (playerBallDistance > 15)
            MoveToTargetVector();
        else
            ReActiveAi();
            */
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

            // if(balltransform.transform.position.x < 0.0f){moveaxis.x = -1;}
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
        if(ballTransform != null)
        {
            if (Vector2.Distance(playerTransform.position, targetVector) < 6f)
               // Debug.Log("Dash !!!");
                return true;
        }
       
        #region alter ReActive code
        /*
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
        */
        #endregion

         return false;

    }

    public bool GetBuff()
    {
        /*if (enemyPlayer)
        {
            
            return true;
        }*/

        return false;
    }

    
    public bool GetPowerShoot()
    {
        GetBallTransform();
        if (ballTransform != null)
        {
            if (Vector3.Distance(ballTransform.position, playerTransform.transform.position) < blockBallDistance)
            {
                return true;
            }
        }

        return false;
    }

    private void SetEnemyPlayer()
    {
        if (playerTransform == rightPlayer.transform)
        {
            enemyPlayer = leftPlayer.GetComponent<Player>();
            character = rightPlayer.GetComponent<Player>();
        }
        else
        {
            enemyPlayer = rightPlayer.GetComponent<Player>();
            character = leftPlayer.GetComponent<Player>();
        }
    }
    
}
