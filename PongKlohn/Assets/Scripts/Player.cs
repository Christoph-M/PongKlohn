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

	private Rigidbody2D myTransform;
	private Animator animator;
	
	private Game gameScript;

	private GameObject catchTrigger;
	private GameObject blockTrigger;
	private GameObject dashTrigger;
	private GameObject missTrigger;
	
	private InputControl controls;
	
	private bool turn;

	private float motionInverter = 1;
	
	void Start() 
	{
		catchTimer = new Timer();
		blockTimer  = new Timer();
		fireTimer = new Timer();
		stunTimer = new Timer();
		waitAfterSoot = new Timer();
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
	
	void Update() 
	{
		SetTrigger(action);
		stunTimer.UpdateTimer();
		fireTimer.UpdateTimer();
		blockTimer.UpdateTimer();
		catchTimer.UpdateTimer();
		waitAfterSoot.UpdateTimer();
		
		if(zuLangsamZumFangenDuMong)///////////////Stun
		{
			isInAction = true;
			isStunned = true;
		}

		direction = controls.UpdateMovement();//zuweisung der Inputachsen
		directionRaw = controls.UpdateMovementRaw();
		
		if (direction != Vector2.zero) {inputAxisDown = true;}
		
		if (controls.IsFireKeyActive(ICanShoot()) && !isInAction)//fire input
		{
			isInAction = true;
			isShooting = true;
		}
		
		if (controls.IsPowerShootActive(ICanShoot()) && !isInAction)//Powershoot input
		{
			isInAction = true;
			isPowerShooting = true;
		}
		

		if (controls.IsBlockKeyActive() && !isInAction)//Block input
		{
			isInAction = true;
			isBlocking = true;
		}
		
		if (controls.IsBuffActive() && !isInAction)//Buff input
		{
			isBlocking = true;
		}
		
		if (controls.IsDashActive() && !isInAction && inputAxisDown && !isDashActive)//Dash input
		{
			isDashActive = true;
			isInAction = true;
			isDashing = true;
		}
		
		if (isShooting)/////////Action Shoot//////////////////////
		{		
			if(shootProgression == 2 && waitAfterSoot.IsFinished())
			{
				shootProgression = 0;
				action = 0;
				isInAction = false;
				isShooting = false;
			}
			else if(shootProgression == 1 && fireTimer.IsFinished())
			{
				waitAfterSoot.SetTimer(0.5f);
				shootProgression = 2;
				Shoot(direction,false);
			}
			else if(shootProgression == 0)
			{
				action = 3;
				fireTimer.SetTimer(1f);
				shootProgression = 1;
			}		
		}
		
		if (isPowerShooting)/////////Action PowerShoot//////////////////////
		{		
			if(shootProgression == 2 && waitAfterSoot.IsFinished())
			{
				shootProgression = 0;
				action = 0;
				isInAction = false;
				isPowerShooting = false;
			}
			else if(shootProgression == 1 && fireTimer.IsFinished())
			{
				waitAfterSoot.SetTimer(0.5f);
				shootProgression = 2;
				Shoot(direction,false);
			}
			else if(shootProgression == 0)
			{
				action = 3;
				fireTimer.SetTimer(1f);
				shootProgression = 1;
			}		
		}
		
		if(isBlocking)///////////////////////Block Action//////////////////////////
		{	
			if(blockProgression == 2 && blockTimer.IsFinished())
			{
				action = 0;
				blockProgression = 0;
				isInAction = false;
				isBlocking = false;
			}
			else if(blockProgression == 1)
			{
				if(!controls.IsBlockKeyActive()){blockProgression = 2;}
				blockTimer.SetTimer(blockTime);
				action = 2;
			}
			else if(blockProgression == 0)
			{
				action = 1;
				blockProgression =1;
			}
		}
	
		if(isDashing && power >= dashEnergyCost)//////////////Dash//////////////////////////////////////////
		{
			if(dashProgression == 1 )
			{
				dashProgression = 0;
				isDashing = false;
				action = 0;
				isInAction = false;
			}
			else if(dashProgression == 0)
			{
				power -= dashEnergyCost;
				action = 10;
				dashProgression =1;
				animator.SetFloat("xAxis", direction.x * motionInverter);
				animator.SetFloat("yAxis", direction.y * motionInverter);
				myTransform.AddForce (directionRaw * dashSpeed, ForceMode2D.Impulse);
			}
		} 
		if(!controls.IsDashActive())
		{
				isDashActive = false;		
		}		
		inputAxisDown =	false;
		
		if(isBuffing)///////////////////////Buff Action//////////////////////////
		{	
			if(blockProgression == 1 && !controls.IsBuffActive())
			{
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
		
		if(isStunned)///////////////////////isStunned Action//////////////////////////
		{	
			if(stunProgression == 1 && stunTimer.IsFinished())
			{
				action = 0;
				isInAction = false;
				isStunned = false;
			}
			else if(stunProgression == 0)
			{
				stunTimer.SetTimer(2f);
				action = 6;
				stunProgression =1;
			}
		}

		if (canMovement)// Bewegt den spieler
		{
			animator.SetFloat("xAxis", direction.x * motionInverter);
			animator.SetFloat("yAxis", direction.y * motionInverter);
			myTransform.AddForce (direction * speed);
		}
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

					canMovement = true;
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
}
