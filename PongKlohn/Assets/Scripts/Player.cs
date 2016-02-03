using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Player : MonoBehaviour 
{	
	public Transform ballSpohorn;
	public List<GameObject> balls;
	public int health { get; set; }
	public int power { get; set; }
	public float blockTime { get; set; }
	public float speed { get; set; }
	public float dashSpeed { get; set; }
	public bool InvertMotion = false;
	protected const float fieldHeight = 22.0f;
	protected const float fieldWidth = 70.0f;
	protected float wallTop;
	protected float wallBottom;
	protected float wallLeft;
	protected float wallRight;
    private Timer buffCoolDown;
    private Timer buffTimer;
	private Timer catchTimer;
	private Timer blockTimer;
	private Timer fireTimer;
	private Timer stunTimer;
	private Timer waitAfterSoot;
	private Timer dashTimer;
	private Rigidbody2D myTransform;
	private Animator animator;
	private Game gameScript;
	private GameObject catchTrigger;
	private GameObject blockTrigger;
	private GameObject dashTrigger;
	private GameObject missTrigger;
	private InputControl controls;
	private float motionInverter = 1;
	private AudioLoop audioDing;
	private Vector2 direction = Vector2.zero;//zuweisung der Inputachsen
	private	Vector2 directionRaw = Vector2.zero;
	private bool dashHasBeenTriggert = false;
	private bool zuLangsamZumFangenDuMong = false;
	private bool canMovement = true;
	private int action = 5;
	private int blockProgression = 0;
	private int shootProgression = 0;
	private int dashProgression = 0;
	private int buffProgression = 0;
	private int stunProgression = 0;
	private bool isInAction = false;
	private bool isDashing = false;
	private bool isBuffing = false;
	private bool isDashActive = false;
	private bool isPowerShooting = false;
	private bool inputAxisDown = false;
	private bool isShooting = false;
	private bool isBlocking = false;
	private bool isStunned = false;
	private float fangShildTimer = 0f;
	private float blockLoad = 0F;
	private float blockMoveMod = 1f;
	private float collisionRange = 1f;
	private Vector3 dir = Vector3.zero;
	private Vector3 oldDir = Vector3.zero;
	private float curvepoint = 0f;
	private float deltatime2 = 0f;
	private RaycastHit2D playerRayHit;
	private float bandenDist = 0f;
	private Vector3 lerpDir = Vector3.zero;
	private float maxX = 0f;
	private float maxY = 0f;
	private Vector3 lerpCurve = Vector3.zero;
	private Vector3 startVec = Vector3.zero;
	private Vector3 endVec = Vector3.zero;
	private float dashLength = 7f;//Dasch Distance
	private float dashTime = 0.2f;//Dash dauer
	private float hitLength = 0f;
	private Vector3 oldValue = Vector3.zero;
	private Vector3 newValue = Vector3.zero;
	private Vector3 wallDistance = Vector3.zero;
	private Vector3 lerpedDash = Vector3.zero;
	private int oldState =0;
	private bool dashBool = true;
	public GameObject smoke;
	public GameObject fangShild;
	public GameObject blockShild;
    public GameObject BlockCollider;
    public GameObject DashCollider;
    private Curves curves;
	public int dashEnergyCost { get; set; }
    private int dashCost;
	public int specialCost = 0;
	public int buffCost = 0;
    private bool blockWasHit = false;
    private int crystal = 0;
    private MasterScript masterScript;
    private float buffMoveMod = 1;


    void Start() 
	{
		curves = GameObject.FindObjectOfType (typeof(Curves)) as Curves;
		audioDing = GameObject.FindObjectOfType (typeof(AudioLoop)) as AudioLoop;
		fangShild.transform.localScale = Vector3.zero;
        buffCoolDown = new Timer();
        catchTimer = new Timer();
		blockTimer  = new Timer();
		fireTimer = new Timer();
		stunTimer = new Timer();
		waitAfterSoot = new Timer();
		dashTimer = new Timer();
		buffTimer = new Timer();
		gameScript = GameObject.FindObjectOfType (typeof(Game)) as Game;
        masterScript = GameObject.FindObjectOfType(typeof(MasterScript)) as MasterScript;
        int c;
        if (this.tag == "Player1")
        {
            c = 1;
        }
        else
        {
            c = 2;
        }
        crystal = masterScript.GetCrystal(c);
        animator = GetComponent<Animator>();
		myTransform = this.GetComponent<Rigidbody2D>();
		
		//var children = gameObject.GetComponentsInChildren<Transform>() as GameObject;// finde Trigger  
		//foreach (var child in children)
		foreach (Transform child in transform)
		{
			if (child.name == "Catch_Trigger")
			{
				catchTrigger = child.gameObject;
			}
			
			if (child.name == "Block_Trigger")
			{
				blockTrigger = child.gameObject;
			}
			
			if (child.name == "Dash_Trigger")
			{
				dashTrigger = child.gameObject;
			}
			
			if (child.name == "Miss_Trigger")
			{
				missTrigger = child.gameObject;
			}
		}
		
		catchTrigger.SetActive(true);///////////////
		missTrigger.SetActive(false);
		blockTrigger.SetActive(false);
		dashTrigger.SetActive(false);

		wallTop = fieldHeight / 2;
		wallBottom = -fieldHeight / 2;
		wallRight = fieldWidth / 2;
		wallLeft = -fieldWidth / 2;

        dashCost = dashEnergyCost;

		if (InvertMotion) //Spieler Steht Links oder Recht   Steuerung anpassen
		{
			motionInverter = -1;
		} else {
			motionInverter = 1;
		}
	}
	
	void Update() 
	{
		direction = controls.UpdateMovement();//zuweisung der Inputachsen
		directionRaw = controls.UpdateMovementRaw();
		
		StartCoroutine(this.MovePlayer(directionRaw));
		StartCoroutine(this.PerformAction(direction, directionRaw));
		StartCoroutine (this.KeepPlayerInField ());
	}

	IEnumerator KeepPlayerInField() {
		if (this.transform.position.x > wallRight) {
			this.transform.position = new Vector3(wallRight, this.transform.position.y, this.transform.position.z);
		} else if (this.transform.position.x < wallLeft) {
			this.transform.position = new Vector3(wallLeft, this.transform.position.y, this.transform.position.z);
		}

		if (this.transform.position.y > wallTop) {
			this.transform.position = new Vector3(this.transform.position.x, wallTop, this.transform.position.z);
		} else if (this.transform.position.y < wallBottom) {
			this.transform.position = new Vector3(this.transform.position.x, wallBottom, this.transform.position.z);
		}

		yield return 0;
	}
	
	IEnumerator MovePlayer(Vector2 direction_) {
		if (canMovement)// Bewegt den spieler
		{
			//animator.SetFloat("xAxis", direction_.x * motionInverter);
			//animator.SetFloat("yAxis", direction_.y * motionInverter);
			move(direction_,speed * blockMoveMod * buffMoveMod);
			//myTransform.AddForce (direction_ * speed);
		}
		
		yield return 0;
	}

    Vector2 directionRaw_;
	int actionIndex = 0;
	IEnumerator PerformAction(Vector2 direction_, Vector2 dirRaw_) {
		directionRaw_ = dirRaw_;
		
		SetTrigger(action);

        buffCoolDown.UpdateTimer();
        stunTimer.UpdateTimer();
		fireTimer.UpdateTimer();
		blockTimer.UpdateTimer();
		catchTimer.UpdateTimer();
		waitAfterSoot.UpdateTimer();
		buffTimer.UpdateTimer();
        //dashTimer.UpdateTimer();

        if (buffCoolDown.IsFinished())
        {
            buffMoveMod = 1;
            SetBlockColliderCale(1f);
            dashEnergyCost = dashCost;
        }


        if (zuLangsamZumFangenDuMong)///////////////Stun
		{
            Debug.Log("stun call");
			actionIndex = 1;//Do stunAction
			isInAction = true;
			
		}

        if (controls.IsBlockKeyActive() && !isInAction)//Block input
		{
			//audioDing.SetSrei();
			actionIndex = 2;//Do Block
			isInAction = true;
		}

        if (controls.IsBuffActive() && !isInAction && buffCoolDown.IsFinished() && power >= buffCost)//Buff input
		{
            //Debug.Log("buff input works");
            power -= buffCost;
			actionIndex = 4;//Do buff
			isInAction = true;
		}

        if (controls.IsDashActive() && !isInAction && directionRaw_ != Vector2.zero&& power >= dashEnergyCost)//Dash input
		{
			audioDing.SetSrei();
			power -= dashEnergyCost;
			actionIndex = 3;//Do Dash
			isInAction = true;
		}
		
		if (controls.IsPowerShootActive(true) && (!isInAction || isBlocking) && power >= specialCost)//Powershoot input
		{
			actionIndex = 2;//Do DoBlock
			isInAction = true;
			isPowerShooting = true;//DO special on bounce
		}
		else{isPowerShooting = false;}
		
        

		if(Actions(actionIndex))
		{
			isInAction = false;
            action = 0;
            actionIndex = 0;
		}
		
		yield return 0;
	}

    public GameObject Instance(GameObject ball, Vector3 position, Quaternion rotation)
    {
        //Debug.Log("Insnciere ball");
        return Instantiate(ball, position, rotation) as GameObject;
    }

    private float startValue = 0F;
	private bool move(Vector3 direction,float moveSpeed)///////////////////////MOVE
	{
		if(direction != dir)
		{
			if (direction != Vector3.zero){
				Instance(smoke,this.transform.position, Quaternion.LookRotation(direction,Vector3.forward));
			}
			startValue = Time.time;
			//oldDir = dir;
			oldDir = lerpDir;
			dir = direction;
		}

		lerpDir = Vector3.Lerp (oldDir, direction,curves.GetCurve((Time.time - startValue)*3,1));
		lerpDir = Vector3.ClampMagnitude (lerpDir, 1.0f);

		if (lerpDir.x > 0.0f) {
			lerpCurve.x = curves.GetCurve(this.AbstandRight()/3,2);
		}
		if (lerpDir.x < 0.0f) {
			lerpCurve.x = curves.GetCurve(this.AbstandLeft()/3,2);
		}
		if (lerpDir.y > 0.0f) {
			lerpCurve.y = curves.GetCurve(this.AbstandTop()/3,2);
		}
		if (lerpDir.y < 0.0f) {
			lerpCurve.y = curves.GetCurve(this.AbstandBottom()/3,2);
		}
		//Debug.Log(this.AbstandTop()+"   "+lerpCurve.y);
		transform.position += new Vector3 ((Time.deltaTime * moveSpeed * lerpDir.x * lerpCurve.x), (Time.deltaTime * moveSpeed * lerpDir.y * lerpCurve.y), 0.0f);
		animator.SetFloat("xAxis", lerpDir.x * motionInverter * lerpCurve.x);
		animator.SetFloat("yAxis", lerpDir.y * motionInverter * lerpCurve.y);
		return false;
	}
		
	private bool MoveTo(Vector3 direction)/////////////////////////MoveTo (Dash)
	{
		if(dashBool)
		{
			Debug.Log("Dash start: ");
			
			dashBool = false;
			startVec = transform.position;
			lerpedDash = Vector3.ClampMagnitude (direction, 1.0f);
			endVec =  startVec + (lerpedDash * dashLength);
			startValue = Time.time;
			oldValue = transform.position;
			
			if (lerpedDash.x > 0.0f) {
				wallDistance.x = this.AbstandRight();
				if(wallDistance.x < lerpedDash.x * dashLength){endVec.x = startVec.x + wallDistance.x;}
			}
			if (lerpedDash.x < 0.0f) {
				wallDistance.x = this.AbstandLeft();
				if(wallDistance.x < lerpedDash.x * dashLength){endVec.x = startVec.x - wallDistance.x;}
			}
			if (lerpedDash.y > 0.0f) {
				wallDistance.y = this.AbstandTop();
				if(wallDistance.y < lerpedDash.y * dashLength){endVec.y = startVec.y + wallDistance.y;}
			}
			if (lerpedDash.y < 0.0f) {
				wallDistance.y = this.AbstandBottom();
				if(wallDistance.y < lerpedDash.y * dashLength){endVec.y = startVec.y - wallDistance.y;}
			}
            Vector3 diff = (startVec - endVec);
            GameObject g = Instance(DashCollider, transform.position -(diff/2), ToolBox.GetRorationFromVector(new Vector3(directionRaw_.x,directionRaw_.y,0)));
            g.tag = this.tag;
        }

        animator.SetFloat("xAxis", direction.x);
		animator.SetFloat("yAxis", direction.y);
		
		newValue = Vector3.Lerp (startVec, endVec, curves.GetCurve((Time.time - startValue)*4f,2));
		transform.position += (newValue - oldValue);
		oldValue = newValue;
		float deltatime = Time.time - startValue;
		if(deltatime >= dashTime)
		{
			//Debug.Log("Dash Ende: "+ deltatime);
			dashBool = true;
			return true;
		}
		return false;
	}

    private void SetBlockColliderCale(float sice)
    {
        blockTrigger.transform.localScale = new Vector3(1, sice, 1);
    }

    private float AbstandTop() { return wallTop - transform.position.y; }

	private float AbstandBottom() { return transform.position.y - wallBottom; }

	private float AbstandRight() {
		if (InvertMotion) {
			return wallRight - transform.position.x;
		} else {
			return  -5 - transform.position.x;
		}
	}

	private float AbstandLeft() { 
		if (InvertMotion) {
			return transform.position.x - 5;
		} else {
			return transform.position.x - wallLeft;
		}
	}
	
	public void SetPlayer(string player) 
	{
		controls = new InputControl(player,this.transform);
	}
	
	public void SetZuLangsamZumFangenDuMong(bool zLZFDM)
	{
		zuLangsamZumFangenDuMong = zLZFDM;
	}
	
	public void SetDashTrigger(bool dt)
	{
		dashHasBeenTriggert = dt;
	}
	
	public Vector3 GetProjectilePositin() 
	{
		return myTransform.gameObject.transform.position;
	}
	
	public Quaternion Rotation() 
	{
		return myTransform.gameObject.transform.rotation;
	}
	
	public Vector3 GetRotation()
	{
		return new Vector3(myTransform.gameObject.transform.rotation.x, myTransform.gameObject.transform.rotation.y, myTransform.gameObject.transform.rotation.z);
	}

    private float blockEfectScale = 0f;
    private float blockEffectRotSpeed = 0f;
    private float ballSpeedup = 0f;

    public float GetBlockMode()
    {
        return ballSpeedup;
    }

    public bool SetBlock(string mode)
    {
        if (mode == "Block" || mode == "Load" || mode == "Special")
        {
            blockTimer.SetTimer(blockTime);
            blockMoveMod = 0.25f;
            if (blockEfectScale <= 1)
            {
                blockEfectScale += Time.deltaTime;
                blockShild.transform.localScale = Vector3.one * blockEfectScale;
            }

            if(mode == "Block")//block while move
            {
                blockTrigger.gameObject.tag = "BlockTrigger";
                if (ballSpeedup >0)
                {
                    ballSpeedup -= Time.deltaTime*2;
                }
                else
                {
                    ballSpeedup = 0;
                }
            }else if (mode == "Load")
            {
                blockTrigger.gameObject.tag = "BlockTrigger";
                if (ballSpeedup < 1)
                {
                    ballSpeedup += (Time.deltaTime/5);
                    //Debug.Log("block loads");
                }
                else
                {
                    ballSpeedup = 1f;
                }
                if(OnBlock())
                {
                    ballSpeedup = 0;
                }
            }else if (mode == "Special")
            {
                blockTrigger.gameObject.tag = "SpecialTrigger";

                //Debug.Log("spessel");
                ballSpeedup = 2f;
                if(OnBlock())
                {
                    power -= specialCost;
                }
            }

        }
        else if (mode == "Non")
        {
            if (blockTimer.IsFinished())
            {
                blockMoveMod = 1f;
                if (blockEfectScale >= 0)
                {
                    blockEfectScale -= Time.deltaTime;
                    blockShild.transform.localScale = Vector3.one * blockEfectScale;
                }
                else
                {
                    blockShild.transform.localScale = Vector3.zero;
                    return true;
                }
            }
        }
        blockEffectRotSpeed += (ballSpeedup * 800) * Time.deltaTime;
        //Debug.Log("blockEffectRotSpeed"+ blockEffectRotSpeed + "   :"+ballSpeedup);
        blockShild.transform.rotation = Quaternion.AngleAxis(blockEffectRotSpeed, new Vector3(1,0,0));
        return false;
    }

    public bool Actions(int i)
	{
		switch(i)
		{
			case 0://normal
				action = 0;
				return true;
			case 1:////isStunned Action////
				if(stunProgression == 1 && stunTimer.IsFinished())
				{
                    Debug.Log("stun endet");
                    zuLangsamZumFangenDuMong = false;
                    return true;
				}
				else if(stunProgression == 0)
				{
                    Debug.Log("stun ist gesetzt");
					stunTimer.SetTimer(2f);
					action = 4;
					stunProgression =1;
				}
				return false;

            case 2://///Block Action//////////////////////////	

				if(blockProgression == 2 && blockTimer.IsFinished())
				{
                    if(SetBlock("Non"))
                    {
		                blockProgression = 0;
                        return true;
                    }
                    return false;
				}
				else if(blockProgression == 1)
				{
                    action = 2;
                    if (controls.IsPowerShootActive(true))
                    {
                        SetBlock("Special");
                    }
                    else
                    {
                        if (directionRaw_ == Vector2.zero || OnBlock())
                        {
                            SetBlock("Load");
                        }
                        else
                        {
                            SetBlock("Block");
                        }
                    }
					if(!controls.IsBlockKeyActive() && !controls.IsPowerShootActive(true) )
                    {
                        blockProgression = 2;
                    }
				}
				else if(blockProgression == 0)
				{
					action = 2;
					blockProgression =1;
				}
				return false;

            case 3://///Dash//////////////////////////////////////////
				if(dashProgression == 2)
				{
					dashProgression = 0;
                    return true;
				}
				else if(dashProgression == 1)
				{
                    if (MoveTo(directionRaw_))
					{
						dashProgression = 2;
					}
				}
                else if (dashProgression == 0)
                {
                    action = 10;
                    dashProgression = 1;
                }
                return false;

            case 4://///////Buff Action//////////////////////////	;
				if(buffProgression == 1 && buffTimer.IsFinished())
				{
                    //Debug.Log("buff ende");
					buffProgression = 0;
                    return true;
				}
				else if(buffProgression == 0)
				{
                    //Debug.Log("Buff");
                    PerformBuff();
                    buffCoolDown.SetTimer(10f);
                    buffTimer.SetTimer(2f);
					action = 6;
					buffProgression =1;
				}
				return false;
			default: 
			   return false;
		}
	}
    private float buffColliderScale = 1;
    private void PerformBuff()
    {
        if(crystal == 0f)
        {
            buffMoveMod = 3f;
        }
        if (crystal == 1f)
        {
            SetBlockColliderCale(2f);
        }
        if (crystal == 2f)
        {
            dashEnergyCost = 0;
        }
    }

	private bool OnBlock()
    {
        if(blockWasHit)
        {
            blockWasHit = false;
            return true;
        }
        return false;
    }

    public float SetOnBlock()
    {    
        blockWasHit = true;
        return blockLoad;
    }

	public void SetTrigger(int newState)
	{
		if(oldState != newState)
		{
			switch(newState)
			{
				case 0://normaler
					//Debug.Log("xxxxx");
					RestTrigger();
					missTrigger.SetActive(true);
					//this.transform.FindChild("Block").gameObject.SetActive(false);
					canMovement = true;
					ResetAnimator();
					oldState = newState;
					break;
				
				case 1://block state 1
					RestTrigger();
					missTrigger.SetActive(true);////////////
					canMovement = false;
					ResetAnimator();
					animator.SetBool ("Block", true);				
					oldState = newState;
					break;
				
				case 2://block state 2
					RestTrigger();
					blockTrigger.SetActive(true);///////////////
					//this.transform.FindChild("Block").gameObject.SetActive(true);
					canMovement = true;
					ResetAnimator();
					animator.SetBool ("Block", true);
					oldState = newState;
					break;
				
				case 3://shoot
					RestTrigger();
					canMovement = true;
					ResetAnimator();
					animator.SetBool ("Fire", true);
					oldState = newState;
					break;
				
				case 4://stun
					RestTrigger();
					missTrigger.SetActive(true);/////////
					canMovement = false;
					ResetAnimator();
					animator.SetBool ("Stun", true);					
					oldState = newState;
					break;
				
				case 5://start
					//Debug.Log("start");
					RestTrigger();
					catchTrigger.SetActive(true);
					canMovement = true;
					ResetAnimator();
					oldState = newState;
					break;
				
				case 6://Buff
					RestTrigger();
					canMovement = false;
					ResetAnimator();
					animator.SetBool ("Buff", true);					
					oldState = newState;
					break;
				
				case 7://Powershoot
					RestTrigger();
					canMovement = true;
					ResetAnimator();
					animator.SetBool ("PowerShoot", true);					
					oldState = newState;
					break;
				
				case 8://win
					RestTrigger();
					canMovement = false;
					ResetAnimator();
					animator.SetBool ("Win", true);
					oldState = newState;
					break;
				
				case 9://loose
					RestTrigger();
					canMovement = false;
					ResetAnimator();
					animator.SetBool ("Loose", true);
					oldState = newState;
					break;
				
				case 10://dash
					RestTrigger();
					blockTrigger.SetActive(true);
                    dashTrigger.SetActive(true);
					canMovement = false;
					ResetAnimator();
					animator.SetBool ("Block", true);
					animator.SetBool ("Dash", true);
					oldState = newState;
					break;
			}
		}
	}
	
	public void ResetAnimator()
	{
		animator.SetBool ("Block", false);
		animator.SetBool ("Fire", false);
		animator.SetBool ("PowerShoot", false);
		animator.SetBool ("Buff", false);
		animator.SetBool ("Stun", false);
		animator.SetBool ("Win", false);
		animator.SetBool ("Loose", false);
		animator.SetBool ("Dash", false);
	}
	
	public void RestTrigger()
	{
		catchTrigger.SetActive(false);
		missTrigger.SetActive(false);/////////
		blockTrigger.SetActive(false);
		dashTrigger.SetActive(false);
	}
}
