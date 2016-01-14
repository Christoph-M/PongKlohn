using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Player : MonoBehaviour 
{	
	public Transform ballSpohorn;
	public List<GameObject> balls;
	
	public int health { get; set; }
	public int power { get; set; }
	public int dashEnergyCost { get; set; }
	public float blockTime { get; set; }
	
	public float speed { get; set; }
	public float dashSpeed { get; set; }
	public bool InvertMotion = false;
	
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
	public GameObject smoke;
	public GameObject fangShild;
	public GameObject blockShild;
	public AnimationCurve fangCurve = AnimationCurve.EaseInOut(0f,0f,1f,0.666f);
	
	private InputControl controls;
	
	private bool turn;

	private float motionInverter = 1;
	
	void Start() 
	{
		fangShild.transform.localScale = Vector3.zero;
		catchTimer = new Timer();
		blockTimer  = new Timer();
		fireTimer = new Timer();
		stunTimer = new Timer();
		waitAfterSoot = new Timer();
		dashTimer = new Timer();
		gameScript = GameObject.FindObjectOfType (typeof(Game)) as Game;
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
		
		if (InvertMotion) //Spieler Steht Links oder Recht   Steuerung anpassen
		{
			motionInverter = -1;
		} else {
			motionInverter = 1;
		}
	}
	
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
	public float blockMoveMod = 1f;
	
	void Update() 
	{
		//Debug.Log("turn"+turn);
		if(ICanShoot())
		{
			if(fangShildTimer<1f)
			{
				fangShildTimer += Time.deltaTime * 1f;
			}
		}
		else
		{	
			if(fangShildTimer>0f)
			{
				fangShildTimer -= Time.deltaTime * 2f;
			}
			
			
		}
		fangShild.transform.localScale = Vector3.one * fangCurve.Evaluate(fangShildTimer);

		direction = controls.UpdateMovement();//zuweisung der Inputachsen
		directionRaw = controls.UpdateMovementRaw();
		
		StartCoroutine(this.MovePlayer(directionRaw));
		StartCoroutine(this.PerformAction(direction, directionRaw));
	}
	
	IEnumerator MovePlayer(Vector2 direction_) {
		if (canMovement)// Bewegt den spieler
		{
			//animator.SetFloat("xAxis", direction_.x * motionInverter);
			//animator.SetFloat("yAxis", direction_.y * motionInverter);
			move(direction_,speed * blockMoveMod);
			//myTransform.AddForce (direction_ * speed);
		}
		
		yield return 0;
	}
	
	IEnumerator PerformAction(Vector2 direction_, Vector2 directionRaw_) {
		SetTrigger(action);
		stunTimer.UpdateTimer();
		fireTimer.UpdateTimer();
		blockTimer.UpdateTimer();
		catchTimer.UpdateTimer();
		waitAfterSoot.UpdateTimer();
		//dashTimer.UpdateTimer();
		
		if(zuLangsamZumFangenDuMong)///////////////Stun
		{
			isInAction = true;
			isStunned = true;
		} else if (controls.IsFireKeyActive(ICanShoot()) && !isInAction)//fire input
		{
			isInAction = true;
			isShooting = true;
		} else if (controls.IsPowerShootActive(ICanShoot()) && !isInAction)//Powershoot input
		{
			//isInAction = true;
//			isPowerShooting = true;
		} else if (controls.IsBlockKeyActive() && !isInAction)//Block input
		{
			isInAction = true;
			isBlocking = true;
		} else if (controls.IsBuffActive() && !isInAction)//Buff input
		{
			//isBlocking = true;
		} else if (controls.IsDashActive() && !isInAction && directionRaw_ != Vector2.zero)// && power >= dashEnergyCost)//Dash input
		{
			power -= dashEnergyCost;
			isInAction = true;
			isDashing = true;
		}
		
		if(isStunned)///////////////////////isStunned Action//////////////////////////
		{
			//Debug.Log(this.transform.name + " stun enter");
			if(stunProgression == 1 && stunTimer.IsFinished())
			{
				//Debug.Log(this.transform.name + " stun end");
				action = 0;
				isInAction = false;
				isStunned = false;
				zuLangsamZumFangenDuMong = false;
			}
			else if(stunProgression == 0)
			{
				stunTimer.SetTimer(2f);
				action = 6;
				stunProgression =1;
			}
		} else if (isShooting)/////////Action Shoot//////////////////////
		{	
			
			//Debug.Log(this.transform.name + " shoot enter");		
			if(shootProgression == 2 && waitAfterSoot.IsFinished())
			{
				//Debug.Log(this.transform.name + " shoot endet");
				shootProgression = 0;
				action = 0;
				isInAction = false;
				isShooting = false;
			}
			else if(shootProgression == 1 && fireTimer.IsFinished())
			{
				waitAfterSoot.SetTimer(0.1f);
				shootProgression = 2;
				Shoot(direction_,false);
			}
			else if(shootProgression == 0)
			{
				action = 3;
				fireTimer.SetTimer(1f);
				shootProgression = 1;
			}		
		} else if (isPowerShooting)/////////Action PowerShoot//////////////////////
		{
			//Debug.Log(this.transform.name + " powershoot enter");
			if(shootProgression == 2 && waitAfterSoot.IsFinished())
			{
				//Debug.Log(this.transform.name + " powershoot end");
				shootProgression = 0;
				action = 0;
				isInAction = false;
				isPowerShooting = false;
			}
			else if(shootProgression == 1 && fireTimer.IsFinished())
			{
				waitAfterSoot.SetTimer(0.5f);
				shootProgression = 2;
				Shoot(direction_,true);
			}
			else if(shootProgression == 0)
			{
				action = 3;
				fireTimer.SetTimer(1f);
				shootProgression = 1;
			}		
		} else if(isBlocking)///////////////////////Block Action//////////////////////////
		{	
			blockShild.transform.localScale = new Vector3(0.6f,0.6f+(blockLoad*0.5f),0.6f);
			//Debug.Log(this.transform.name + " block Enter");
			if(blockProgression == 2 && blockTimer.IsFinished())
			{
				//Debug.Log(this.transform.name + " block endet");
				action = 0;
				blockMoveMod = 1f;
				blockProgression = 0;
				isInAction = false;
				isBlocking = false;
				blockLoad = 0f;
			}
			else if(blockProgression == 1)
			{
				if(!controls.IsBlockKeyActive()){blockProgression = 2;blockMoveMod = 1f;}
				if(blockLoad<1){blockLoad+=(0.5f * Time.deltaTime);}
				blockMoveMod = 0.5f;
				blockTimer.SetTimer(blockTime);
				action = 2;
			}
			else if(blockProgression == 0)
			{
				action = 1;
				blockProgression =1;
			}
		} else if(isDashing)//////////////Dash//////////////////////////////////////////
		{
			//Debug.Log(this.transform.name + " dash enter");
			if(dashProgression == 1)
			{
				Debug.Log(this.transform.name + " dash endet");
				dashProgression = 0;
				isDashing = false;
				action = 0;
				isInAction = false;
				
			}
			else if(dashProgression == 0)
			{
				Debug.Log(this.transform.name + " dash start");
				action = 10;
				if(MoveTo(directionRaw_))
				{
					dashProgression = 1;
				}
				//animator.SetFloat("xAxis", direction_.x * motionInverter);
				//animator.SetFloat("yAxis", direction_.y * motionInverter);
				//myTransform.AddForce (directionRaw_ * dashSpeed, ForceMode2D.Impulse);
			}
		} else if(isBuffing)///////////////////////Buff Action//////////////////////////
		{	
		//Debug.Log(this.transform.name + " Buff enter");
			if(blockProgression == 1 && !controls.IsBuffActive())
			{
				//Debug.Log(this.transform.name + " Buff Has endet");
				action = 0;
				buffProgression = 0;
				isBuffing = false;
				
			}
			else if(buffProgression == 0)
			{
				action = 6;
				buffProgression =1;
			}
		}
		
		yield return 0;
	}
	
	int oldState =0;
	public void SetTrigger(int newState)
	{
		if(oldState != newState)
		{
			switch(newState)
			{
				case 0://normal
					catchTrigger.SetActive(true);///////////////
					missTrigger.SetActive(false);
					blockTrigger.SetActive(false);
					dashTrigger.SetActive(false);
					this.transform.FindChild("Block").gameObject.SetActive(false);

					canMovement = true;
					animator.SetBool ("Block", false);
					animator.SetBool ("Fire", false);
					animator.SetBool ("PowerShoot", false);
					animator.SetBool ("Buff", false);
					animator.SetBool ("Stun", false);
					animator.SetBool ("Win", false);
					animator.SetBool ("Loose", false);
					animator.SetBool ("Dash", false);
					
					oldState = newState;
					break;
				
				case 1://block state 1
					catchTrigger.SetActive(false);
					missTrigger.SetActive(true);////////////
					blockTrigger.SetActive(false);
					dashTrigger.SetActive(false);

					canMovement = false;
					animator.SetBool ("Block", true);
					animator.SetBool ("Fire", false);
					animator.SetBool ("PowerShoot", false);
					animator.SetBool ("Buff", false);
					animator.SetBool ("Stun", false);
					animator.SetBool ("Win", false);
					animator.SetBool ("Loose", false);
					animator.SetBool ("Dash", false);
					
					oldState = newState;
					break;
				
				case 2://block state 2
					catchTrigger.SetActive(false);
					missTrigger.SetActive(false);
					blockTrigger.SetActive(true);///////////////
					dashTrigger.SetActive(false);//////////////
					this.transform.FindChild("Block").gameObject.SetActive(true);

					canMovement = true;
					animator.SetBool ("Block", true);
					animator.SetBool ("Fire", false);
					animator.SetBool ("PowerShoot", false);
					animator.SetBool ("Buff", false);
					animator.SetBool ("Stun", false);
					animator.SetBool ("Win", false);
					animator.SetBool ("Loose", false);
					animator.SetBool ("Dash", false);
					
					oldState = newState;
					break;
				
				case 3://shoot
					catchTrigger.SetActive(false);
					missTrigger.SetActive(false);
					blockTrigger.SetActive(false);
					dashTrigger.SetActive(false);

					canMovement = true;
					animator.SetBool ("Block", false);
					animator.SetBool ("Fire", true);
					animator.SetBool ("PowerShoot", false);
					animator.SetBool ("Buff", false);
					animator.SetBool ("Stun", false);
					animator.SetBool ("Win", false);
					animator.SetBool ("Loose", false);
					animator.SetBool ("Dash", false);
					
					oldState = newState;
					break;
				
				case 4://stun
					catchTrigger.SetActive(false);
					missTrigger.SetActive(true);/////////
					blockTrigger.SetActive(false);
					dashTrigger.SetActive(false);

					canMovement = false;
					animator.SetBool ("Block", false);
					animator.SetBool ("Fire", false);
					animator.SetBool ("PowerShoot", false);
					animator.SetBool ("Buff", false);
					animator.SetBool ("Stun", true);
					animator.SetBool ("Win", false);
					animator.SetBool ("Loose", false);
					animator.SetBool ("Dash", false);
					
					oldState = newState;
					break;
				
				case 5://start
					catchTrigger.SetActive(true);
					missTrigger.SetActive(false);/////////
					blockTrigger.SetActive(false);
					dashTrigger.SetActive(false);

					canMovement = true;
					animator.SetBool ("Block", false);
					animator.SetBool ("Fire", false);
					animator.SetBool ("PowerShoot", false);
					animator.SetBool ("Buff", false);
					animator.SetBool ("Stun", false);
					animator.SetBool ("Win", false);
					animator.SetBool ("Loose", false);
					animator.SetBool ("Dash", false);
					
					oldState = newState;
					break;
				
				case 6://Buff
					catchTrigger.SetActive(true);
					missTrigger.SetActive(false);/////////
					blockTrigger.SetActive(false);
					dashTrigger.SetActive(false);

					canMovement = true;
					animator.SetBool ("Block", false);
					animator.SetBool ("Fire", false);
					animator.SetBool ("PowerShoot", false);
					animator.SetBool ("Buff", true);
					animator.SetBool ("Stun", false);
					animator.SetBool ("Win", false);
					animator.SetBool ("Loose", false);
					animator.SetBool ("Dash", false);
					
					oldState = newState;
					break;
				
				case 7://Powershoot
					catchTrigger.SetActive(false);
					missTrigger.SetActive(false);/////////
					blockTrigger.SetActive(false);
					dashTrigger.SetActive(false);

					canMovement = true;
					animator.SetBool ("Block", false);
					animator.SetBool ("Fire", false);
					animator.SetBool ("PowerShoot", true);
					animator.SetBool ("Buff", false);
					animator.SetBool ("Stun", false);
					animator.SetBool ("Win", false);
					animator.SetBool ("Loose", false);
					animator.SetBool ("Dash", false);
					
					oldState = newState;
					break;
				
				case 8://win
					catchTrigger.SetActive(false);
					missTrigger.SetActive(false);/////////
					blockTrigger.SetActive(false);
					dashTrigger.SetActive(false);

					canMovement = true;
					animator.SetBool ("Block", false);
					animator.SetBool ("Fire", false);
					animator.SetBool ("PowerShoot", false);
					animator.SetBool ("Buff", false);
					animator.SetBool ("Stun", false);
					animator.SetBool ("Win", true);
					animator.SetBool ("Loose", false);
					animator.SetBool ("Dash", false);
					animator.SetBool ("Dash", false);
					
					oldState = newState;
					break;
				
				case 9://loose
					catchTrigger.SetActive(false);
					missTrigger.SetActive(false);/////////
					blockTrigger.SetActive(false);
					dashTrigger.SetActive(false);

					canMovement = true;
					animator.SetBool ("Block", false);
					animator.SetBool ("Fire", false);
					animator.SetBool ("PowerShoot", false);
					animator.SetBool ("Buff", false);
					animator.SetBool ("Stun", false);
					animator.SetBool ("Win", false);
					animator.SetBool ("Loose", true);
					animator.SetBool ("Dash", false);
	
					oldState = newState;
					break;
				
				case 10://dash
					catchTrigger.SetActive(false);
					missTrigger.SetActive(false);/////////
					blockTrigger.SetActive(true);
					dashTrigger.SetActive(false);

					canMovement = false;
					animator.SetBool ("Block", false);
					animator.SetBool ("Fire", false);
					animator.SetBool ("PowerShoot", false);
					animator.SetBool ("Buff", false);
					animator.SetBool ("Stun", false);
					animator.SetBool ("Win", false);
					animator.SetBool ("Loose", false);
					animator.SetBool ("Dash", true);
				
					oldState = newState;
					break;
			}
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
	
	public bool setTurn(bool turnt)
	{
		return turn = turnt;
	}

	public GameObject Instance(GameObject ball, Vector3 position, Quaternion rotation)
	{
		//Debug.Log("Insnciere ball");
		return Instantiate(ball, position, rotation) as GameObject;
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
	
	private bool ICanShoot()
	{
		//Debug.Log(this.transform.name + ": " + turn);
		
		if(!gameScript.GetProjectileTransform() && turn)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	private bool Shoot(Vector2 direction,bool type)
	{
		if(type)
		{//speschel schuss
			if(transform.position.y>3)//Spezial oban
			{
				Instance(balls[5], ballSpohorn.position, this.transform.rotation);
				if(!gameScript.GetProjectileTransform()){return true;}	
			}
			else if(transform.position.y<-3)//Special unten
			{
				Instance(balls[4], ballSpohorn.position, this.transform.rotation);
				if(!gameScript.GetProjectileTransform()){return true;}
			}
			else//speschel mitte
			{
				Instance(balls[3], ballSpohorn.position, this.transform.rotation);
				if(!gameScript.GetProjectileTransform()){return true;}
			}	
		}
		else
		{
			//normal schuss
			if (direction.y > 0f) //oben schiessen
			{
				Instance(balls [1], ballSpohorn.position, this.transform.rotation);
				if(!gameScript.GetProjectileTransform()){return true;}
			} 
			else if (direction.y < 0f) //unten schiessen
			{
				Instance (balls [2], ballSpohorn.position, this.transform.rotation);
				if(!gameScript.GetProjectileTransform()){return true;}
			} 
			else if (direction.y == 0f) //normaler schuss
			{
				Instance(balls [0], ballSpohorn.position, this.transform.rotation);
				if(!gameScript.GetProjectileTransform()){return true;}
			}
		}
		
		return false;
	}
	
	private float collisionRange = 1f;
	Vector3 dir = Vector3.zero;
	Vector3 oldDir = Vector3.zero;
	public AnimationCurve curve = AnimationCurve.EaseInOut(0,0,0,0);
	public AnimationCurve curveBande = AnimationCurve.EaseInOut(3f,0f,1f,1f);
	private float curvepoint = 0f;
	float easeTime = 0.3f;
	float startValue = 0;
	float deltatime2 = 0f;
	RaycastHit2D playerRayHit;
	private float bandenDist = 0f;
	private Vector3 lerpDir = Vector3.zero;
	private bool collisionMerker = true;
	private Vector3 moveTarget = Vector3.zero;
	private Vector3 moveStart = Vector3.zero;
	
	
	private bool move(Vector3 direction,float moveSpeed)///////////////////////MOVE
	{			
		if(direction != Vector3.zero)
		{
			playerRayHit = Physics2D.Raycast (new Vector2(transform.position.x,transform.position.y), direction, Mathf.Infinity, -1, 0.09f, 0.11f);
			moveTarget =  new Vector3(playerRayHit.point.x,playerRayHit.point.y,0f) - (direction * collisionRange);
			moveStart =  new Vector3(playerRayHit.point.x,playerRayHit.point.y,0f) - (direction * collisionRange *3f);
		}
		
		bandenDist = Vector3.Distance(new Vector3(playerRayHit.point.x,playerRayHit.point.y,transform.position.z),transform.position);	
		
		
		if(direction != dir)
		{
			if (direction != Vector3.zero){
				Instance(smoke,this.transform.position, Quaternion.LookRotation(direction,Vector3.forward));
			}
			curve = AnimationCurve.EaseInOut(0f,0f,easeTime,1f);
			startValue = Time.time;
			//oldDir = dir;
			oldDir = lerpDir;
			dir = direction;
		}
		
		//lerpDir = direction*schbeltasse*Time.dateTime;
		float deltatime = Time.time - startValue;
		lerpDir = Vector3.Lerp(oldDir, direction, curve.Evaluate(deltatime));
		
		//Vector3 dasding = lerpDir * curveBande.Evaluate(bandenDist);
		if(bandenDist >= 3f)
		{
			curvepoint = 1f;
			transform.position += lerpDir *  moveSpeed * Time.deltaTime;
		}
		else
		{
			curvepoint =  curveBande.Evaluate(bandenDist);
			transform.position = Vector3.Lerp(moveStart, moveTarget,curvepoint);
		}
		animator.SetFloat("xAxis", lerpDir.x * motionInverter * curvepoint);
		animator.SetFloat("yAxis", lerpDir.y * motionInverter * curvepoint);
		return false;
	}
		
	
	public Vector3 startVec = Vector3.zero;
	public Vector3 endVec = Vector3.zero;
	public float dashLength = 50f;//Dasch Distance
	public float dashTime = 5f;//Dash dauer
	public AnimationCurve dashCurve;
	bool dashBool = true;
	private bool MoveTo(Vector3 direction)/////////////////////////MoveTo (Dash)
	{
		if(dashBool)
		{
			dashBool = false;
			//effect
			startVec = transform.position;
			endVec = (direction * dashLength);
			dashCurve = AnimationCurve.EaseInOut(0f,0f,dashTime,1f);
			startValue = Time.time;
			//dir = direction;
		}	
		float deltatime = Time.time - startValue;
		//animator.SetFloat("xAxis", dir.x * deltatime/dashTime * motionInverter);
		//animator.SetFloat("yAxis", dir.y * deltatime/dashTime * motionInverter);
		transform.position = startVec + (endVec * dashCurve.Evaluate(deltatime));
		//Debug.Log("deltatime:"+deltatime);
		if(deltatime >= dashTime)
		{
			//Debug.Log("Dash Ende: "+ deltatime);
			dashBool = true;
			return true;
		}
		return false;
	}
}
