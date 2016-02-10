using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class AI 
{
	private Game gameScript;
	private Transform ballTransform;
	private Vector2 moveAxis = Vector2.zero;
    private Vector2 ballVec2;
	private Transform playerTransform;
	private Vector3 resetPosition ;
    
    private float playerDistance;
    public float minimumPlayerDistanceReset = 30.0f;
    public float maximumPlayerDistanceReset = 35.0f;
    public float minimumPlayerDistance;
    public float maximumPlayerDistance;
    public float playerDistanceJump = 5.0f;
    private float playerBallDistance;
    public static float blockBallDistance = 10f;
    private Vector3 nullPosition = Vector3.zero;
    //will be used to stop the characters from trembling by giving worldspace for tolerance
    private float stopTrembling = 1.75f;
    private float ballSpeed =0f;
    private float persFieldLength;
    int maxMissChance = 90;
    int missChnc;


	private Player character;
    private GameObject leftPlayer;
    private GameObject rightPlayer;
    private Player enemyPlayer;

	public enum State {defensiv, agressiv, neutral, lastSave};
	public State state;

    
	private Vector2 targetVector = new Vector2(0, 0);
	private Vector2 originVector = new Vector2(0,0);
    private float percentageX;
    private int aiStrength;
    private List<int> usedTargetPoints = new List<int>();

    private bool buffAsk = true;
    private bool startingState = true;
    private bool ballMissed = false;
    
    List<Vector2> ballBounceList = new List<Vector2>();
    List<Vector2> ballBounceListSave = new List<Vector2>();
   
    private static Vector2 currentBounce;
    private  enum BounceEvent {AIShot,EnemyShot, neutral};
    private  BounceEvent bounceEvent = BounceEvent.neutral;

    private Transform redWallTrans;
    public bool inBlockRange = false;
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
		aiStrength = gameScript.aiStrength;
        missChnc = GetMissingChance(aiStrength);
		percentageX = StatePercentage();
        persFieldLength = MeasureField();
        
        //blockTrigger = new AIBlockTrigger();
       

    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~  SetballSpeed  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
    //~~~~~~~~~~~~~~~~ Gets the ball speed from Game-class and ~~~~~~~~~~~~~\\
    //~~~~~~~~~~~~~~~~ stores it for later use                 ~~~~~~~~~~~~~\\
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
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

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\

    //~~~~~~~~~~~~~~~~~~~~~~~~~~  MeasureField  ~~~~~~~~~~~~~~~~~~~~~~~~~\\
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
    //~~~~~~~~~~~~~~ Measures the right players field length ~~~~~~~~~~~~\\
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
    private float MeasureField()
    {
        float length;
        RaycastHit2D hit = Physics2D.Raycast(Vector2.zero, Vector2.right, Mathf.Infinity, -1, 0.09f, 0.11f);
        length = hit.distance;

        return length;
    }

    //~~~~~~~~~~~~~~~~~~~~~~  resetMinMaxPlayerDistance  ~~~~~~~~~~~~~~~~~~~\\
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
    //~~~~~~~~~~~~ Resets the maximum- and minimumplayerdistance ~~~~~~~~~~~\\
    //~~~~~~~~~~~~ back to its origin value                      ~~~~~~~~~~~\\
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
    private void resetMinMaxPlayerDistance()
    {
            minimumPlayerDistance = minimumPlayerDistanceReset;
            maximumPlayerDistance = maximumPlayerDistanceReset;
    }

    //~~~~~~~~~~~~~~~~~~~~~~~  SetBallTransform  ~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
    //~~~~~~~~~~~~~ Gets the transform of the ball an stores ~~~~~~~~~~~~~~~\\
    //~~~~~~~~~~~~~ it for later use                         ~~~~~~~~~~~~~~~\\
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
    private void SetBallTransform()
	{
		ballTransform = gameScript.GetProjectileTransform ();
	}

    
    private int GetMissingChance(int strength)
    {
        int missChance =(int) (maxMissChance * (strength / 100));
        return missChance;
    }

    private Vector2 GetMissValue(int miss)
    {
        Vector2 vecVal;
        System.Random rnd = new System.Random();
        int rndValue = rnd.Next(0, 100);

        if (rndValue <= 100 - miss)
            return Vector2.zero;
        else
        {
            vecVal = new Vector2(UnityEngine.Random.Range(6f, 9f), UnityEngine.Random.Range(-5f, -8f));
            float coin = UnityEngine.Random.Range(-1f, 1f);
            if (coin <= 0) vecVal *= -1;
            return vecVal;
        }
    }

    private float GetPercentageOnX(float strength)
    {
       // Debug.Log("strength" + strength);
        float percent;
        if (strength > 100)
            strength = 100;

        percent = StatePercentage(); //* (strength/100f);
        //Debug.Log("percent: " + percent);
        return percent;
    }

    private float StatePercentage()
    {

        if (state == State.agressiv)
            return UnityEngine.Random.Range(35f, 55f);
        else if (state == State.defensiv)
            return UnityEngine.Random.Range(75, 100f);
        else if (state == State.neutral)
            return UnityEngine.Random.Range(55f, 75f);
        else if (state == State.lastSave)
            return 100f;
        else
        {
            Debug.Log("Cannot read State");
            return 0f;
        }

    }

    private float StatePercentage(State testState)
    {
        if (testState == State.agressiv)
            percentageX = 35f;
        else if (testState == State.defensiv)
            percentageX = 75;
        else if (testState == State.neutral)
            percentageX = 55f;
        else if (testState == State.lastSave)
            percentageX = 100f;
        else
        {
            Debug.Log("Cannot read State");
            return 0f;
        }
       
        float lengthX = 0f;

        lengthX = persFieldLength * (percentageX / 100f);
        return lengthX;
    }

   private void SetbounceEvent()
    {
        if ((playerTransform == rightPlayer.transform && currentBounce.x > 0f) || (playerTransform == leftPlayer.transform && currentBounce.x < 0f))
        {
            bounceEvent = BounceEvent.AIShot;
        }
        else if ((playerTransform == rightPlayer.transform && currentBounce.x < 0f) || (playerTransform == leftPlayer.transform && currentBounce.x > 0f))
        {
            bounceEvent = BounceEvent.EnemyShot;
        }
        else if (currentBounce.x == 0f)
        {
           
            if((playerTransform == rightPlayer.transform && ballTransform.gameObject.GetComponent<Ball>().GetPath()[1].x < 0f) || (playerTransform == leftPlayer.transform && ballTransform.gameObject.GetComponent<Ball>().GetPath()[1].x > 0f))
            {
                bounceEvent = BounceEvent.AIShot;
            }
            else if ((playerTransform == rightPlayer.transform && ballTransform.gameObject.GetComponent<Ball>().GetPath()[1].x > 0f) || (playerTransform == leftPlayer.transform && ballTransform.gameObject.GetComponent<Ball>().GetPath()[1].x < 0f))
            {
                bounceEvent = BounceEvent.EnemyShot;
            }
        }
    }
    public static void SetNewTargetVectorCount(Vector2 e)
    {
        
        currentBounce = e;
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
      
        //new calculating if ai missed the ball
        if (ballMissed)
        {
            if (playerTransform = rightPlayer.transform)
            {
                if (currentBounce.x > StatePercentage(State.agressiv)) state = State.neutral;
                else if (currentBounce.x > StatePercentage(State.neutral)) state = State.defensiv;
                else if (currentBounce.x > StatePercentage(State.defensiv)) state = State.lastSave;
            }
            else
            {
                if (currentBounce.x < -StatePercentage(State.agressiv)) state = State.neutral;
                else if (currentBounce.x < -StatePercentage(State.neutral)) state = State.defensiv;
                else if (currentBounce.x < -StatePercentage(State.defensiv)) state = State.lastSave;
            }

           /* if (state == State.agressiv)
            {
                state = State.neutral;
            }
            else if (state == State.neutral)
            {
                state = State.defensiv;
            }
            else if (state == State.defensiv)
            {
                state = State.lastSave;
            }
            
            ballMissed = false;*/
        }
        
    }

    private void AskStartingState(Vector2 o)
    {
        if(o.x == 0f)
        {
            state = State.defensiv;
        }
    }

   
    int a = 0;
    private void CalculateTargetVector ()
	{
        a++;
        Debug.Log("Now Calculating" + a);
        SetbounceEvent();
        if (bounceEvent == BounceEvent.AIShot)
        {
            targetVector = resetPosition;
        }
        else if (bounceEvent == BounceEvent.EnemyShot)
        {
            ballBounceList = ballTransform.gameObject.GetComponent<Ball>().GetPath();
            if (ballBounceList != ballBounceListSave)
            {
                usedTargetPoints.Clear();
            }


            Vector2 origin2 = new Vector2(0, 0);
            Vector2 target2 = new Vector2(0, 0);
            percentageX = StatePercentage();
            float lengthX = 0f;

            lengthX = persFieldLength * (percentageX / 100f);



            for (int i = 0; i < (ballBounceList.Count - 1); i++)
            {
                bool allRdyUsed = false;

                if (lengthX < 0f)
                {
                    if (ballBounceList[i + 1] != null)
                    {
                        if (ballBounceList[i + 1].x <= lengthX && ballBounceList[i].x >= lengthX)
                        {
                            if (ballBounceListSave == ballBounceList)
                            {
                                foreach (int o in usedTargetPoints)
                                {
                                    if (o == i)
                                    {
                                        allRdyUsed = true;
                                    }
                                }
                            }
                            if (!allRdyUsed)
                            {
                                usedTargetPoints.Add(i);
                                target2 = ballBounceList[(i + 1)];
                                origin2 = ballBounceList[i];
                                break;
                            }
                        }
                    }
                }
                else if (lengthX > 0f)
                {
                    if (ballBounceList[(i + 1)] != null)
                    {
                        if (ballBounceList[(i + 1)].x >= lengthX && ballBounceList[i].x <= lengthX)
                        {
                            if (ballBounceListSave == ballBounceList)
                            {
                                foreach (int o in usedTargetPoints)
                                {
                                    if (o == i)
                                    {
                                        allRdyUsed = true;
                                    }

                                    if (ballBounceList[0].x > 0f) allRdyUsed = false;
                                }
                            }
                            if (!allRdyUsed)
                            {
                                usedTargetPoints.Add(i);
                                target2 = ballBounceList[(i + 1)];
                                origin2 = ballBounceList[i];
                                break;
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("Which Player?!");
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
            if (target2 == Vector2.zero && usedTargetPoints.Count != 1)
            {
                /*int c = 0;
                foreach(int i in usedTargetPoints)
                {
                    c = i;
                }
                usedTargetPoints[c] = 0;
                CalculateTargetVector();*/
                targetVector = resetPosition;

            }
            else if (target2 == Vector2.zero && usedTargetPoints.Count == 1)
            {
                //targetvector stays on old value
            }
            else
            {
                AskStartingState(origin2);
                /*  if ((target2.x - origin2.x < 0 && playerTransform == rightPlayer.transform) || (target2.x - origin2.x > 0 && playerTransform == leftPlayer.transform))
                    targetVector = resetPosition;
                    else{*/
                targetVector = AimTarget(target2, origin2, ballBounceList, lengthX);
                targetVector += GetMissValue(missChnc);
                //if (playerTransform == rightPlayer.transform) { targetVector += ;}


            }

            ResetUntilTurn();
            ballBounceListSave = ballBounceList;
        }
        else if (bounceEvent == BounceEvent.neutral) { Debug.Log("bounceEvent == neutral"); }
        bounceEvent = BounceEvent.neutral;
    }

    private void BackTargetCalculation()
    {
        ballBounceList = ballTransform.gameObject.GetComponent<Ball>().GetPath();
        if (ballBounceList != ballBounceListSave)
        {
            usedTargetPoints.Clear();
        }
        percentageX = StatePercentage();

        Vector2 origin2 = new Vector2(0, 0);
        Vector2 target2 = new Vector2(0, 0);

        float lengthX = 0f;
        /*
            foreach (Vector2 target in bounceList)
            {
                target2 = target;
            }
       */
        lengthX = persFieldLength * (percentageX / 100f);



        for (int i = 0; i < (ballBounceList.Count - 1); i++)
        {
            bool allRdyUsed = false;

            if (lengthX < 0f)
            {
                if (ballBounceList[i + 1] != null)
                {
                    if (ballBounceList[i + 1].x >= lengthX && ballBounceList[i].x <= lengthX)
                    {
                        if (ballBounceListSave == ballBounceList)
                        {
                            foreach (int o in usedTargetPoints)
                            {
                                if (o == i)
                                {
                                    allRdyUsed = true;
                                }
                            }
                        }
                        if (!allRdyUsed)
                        {
                            usedTargetPoints.Add(i);
                            target2 = ballBounceList[(i + 1)];
                            origin2 = ballBounceList[i];
                            break;
                        }
                    }
                }
            }
            else if (lengthX > 0f)
            {
                if (ballBounceList[(i + 1)] != null)
                {
                    if (ballBounceList[(i + 1)].x <= lengthX && ballBounceList[i].x >= lengthX)
                    {
                        if (ballBounceListSave == ballBounceList)
                        {
                            foreach (int o in usedTargetPoints)
                            {
                                if (o == i)
                                {
                                    allRdyUsed = true;
                                }
                            }
                        }
                        if (!allRdyUsed)
                        {
                            usedTargetPoints.Add(i);
                            target2 = ballBounceList[(i + 1)];
                            origin2 = ballBounceList[i];
                            break;
                        }
                    }
                }
            }
            else {
                Debug.Log("Which Player?!");
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
        if (target2 == Vector2.zero && usedTargetPoints.Count == 1)
        {
            //targetvector stays targetvector
        }
        else if(target2 == Vector2.zero && usedTargetPoints.Count != 1)
        {
            targetVector = resetPosition;
        }
        else
        {
            AskStartingState(origin2);

            targetVector = AimTarget(target2, origin2, ballBounceList, lengthX);
            targetVector += GetMissValue(missChnc);
        }

        ResetUntilTurn();
        ballBounceListSave = ballBounceList;
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
        Vector2 difference;
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
            float b = targetVector.y - m * targetVector.x;

            final.x = fixX;
            final.y = m * final.x + b;
			/*if (fixX < 0)
				final.x -= 3f;
			else if (fixX > 0)
				final.x += 1.15f;
			else
				Debug.Log ("fixX == 0");*/

         
        }
      //  Debug.Log("final: " + final);
        return final;
    }

    private void MoveToTargetVector()
	{
        if (ballTransform == null || ballTransform.position.x == 0f)
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

    private void MissedBall()
    {
        if (ballTransform != null)
        {
            if ((playerTransform == rightPlayer.transform && currentBounce.x > playerTransform.position.x) || (playerTransform == leftPlayer.transform && currentBounce.x < playerTransform.position.x))
            {
                ballMissed = true;
                SetAiState();
                CalculateTargetVector();
            }
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

      // MissedBall();
    }
  
	//returns the vector2D-position, the AI is moving to
	public Vector2 GetMovementInput()
	{
        
            SetBallTransform();
            SetPlayerBallDistance();
            SetPlayerDistance();
            SetPlayerMinMaxDistance();
            SetAiState();
            
            MasterMindAi();
            //MMGetBallBehind();
            MoveToTargetVector();
        //ReActiveAi();

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
//manipulates the moving axis on the x-axis to reach critical balls
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
        
		SetBallTransform();
		if(ballTransform!=null )
		{
            
			//if (Vector3.Distance (ballTransform.position, targetVector) < blockBallDistance)
            if (Vector3.Distance (ballTransform.position, playerTransform.transform.position) < blockBallDistance)
            {
				return true;
			}
		}

		return false;
	}

    public bool GetDash()
    {
        if(ballTransform != null && ballBounceList[0] != null)
        {
            if (Vector2.Distance(playerTransform.position, targetVector) < 5f && Vector2.Distance(playerTransform.position, targetVector) > 4f && playerBallDistance < 15f)
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
//        GetBallTransform();
//        if (ballTransform != null)
//        {
//            if (Vector3.Distance(ballTransform.position, playerTransform.transform.position) < blockBallDistance)
//            {
//                return true;
//            }
//        }

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
