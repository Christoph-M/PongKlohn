/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	
	public Transform ballSpohorn;
	public List<GameObject> balls;

	public GameObject catchCollider;
	public GameObject blockCollider;
	public GameObject dashCollider;
	
	public int health { get; set; }
	public int power { get; set; }
	
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
	
	private GameObject ball;

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
	private bool axisInUse = false;
	private bool fired = false;
	private float timeLeft = 1.0f;
	
	private bool isStunned = false;
	private bool fireKeyPressed = false;
	private bool onZuLangsamZumFangenDuMongDown = true;
	private int action = 5;
	private bool isInAction = false;
	private bool inAction = true;
	private bool hasShoot = false;
	private bool isBlocking = false;
	private int blockProgression = 0;
	private bool isShooting = false;
	private int shootProgression = 0;
	private bool isDashActive = false;
	
	void Update() 
	{
		SetTrigger(action);
		stunTimer.UpdateTimer();
		fireTimer.UpdateTimer();
		blockTimer.UpdateTimer();
		catchTimer.UpdateTimer();
		waitAfterSoot.UpdateTimer();
		
		if(zuLangsamZumFangenDuMong || controls==null)
		{
			Debug.Log("Get Stuned");
			isStunned = true;
			action = 4;
			zuLangsamZumFangenDuMong = false;
			stunTimer.SetTimer(2);
		}

		if(isStunned)
		{	
			Debug.Log("is stuned");
			Debug.Log("Player is Stunned or Controls havend be initialice");
			
			if(stunTimer.IsFinished())
			{
				Debug.Log("was stuned");
				isStunned = false;
				action = 0;
			}
		}
		else//Wenn der Spieler nicht gerade gestunde ist
		{
			direction = controls.UpdateMovement();//zuweisung der Inputachsen
			directionRaw = controls.UpdateMovement();
			if(direction == Vector2.zero){isDashActive = false;}
	
			if(controls.IsFireKeyActive(ICanShoot()) && !isBlocking)//fire input
			{
				isShooting = true;
			}
			if(controls.IsBlockKeyActive() && !isShooting)//Block input
			{
				isBlocking = true;
			}
			
			if(isShooting)/////////Action Shoot//////////////////////
			{		
				if(shootProgression == 2 && waitAfterSoot.IsFinished())
				{
					shootProgression = 0;
					action = 0;
					isShooting = false;
				}
				else if(shootProgression == 1 && fireTimer.IsFinished())
				{
					if(Shoot(direction,false))
					{
						waitAfterSoot.SetTimer(0.3f);
						shootProgression = 2;
					}
				}
				else if(shootProgression == 0)
				{
					action = 3;
					fireTimer.SetTimer(0.5f);
					shootProgression = 1;
				}		
			}
			
			if(isBlocking)///////////////////////Bock Action//////////////////////////
			{	
				if(blockProgression == 2 && blockTimer.IsFinished())
				{
					action = 0;
					blockProgression = 0;
					isBlocking = false;
				}
				else if(blockProgression == 1)
				{
					if(!controls.IsBlockKeyActive()){blockProgression = 2;}
					blockTimer.SetTimer(1);
					action = 2;
				}
				else if(blockProgression == 0)
				{
					action = 1;
					blockProgression =1;
				}
			}
		}
		
		if(isBlocking && dashHasBeenTriggert && !isDashActive)
		{
			isDashActive = true;
			animator.SetFloat("xAxis", direction.x * motionInverter);
			animator.SetFloat("yAxis", direction.y * motionInverter);
			myTransform.AddForce (directionRaw * dashSpeed, ForceMode2D.Impulse);
			dashHasBeenTriggert = false;
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
					animator.SetBool ("Stun", false);
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
					animator.SetBool ("Stun", false);
					oldState = newState;
					break;
				
				case 2://block state 2
					catchTrigger.SetActive(false);
					missTrigger.SetActive(false);
					blockTrigger.SetActive(true);///////////////
					dashTrigger.SetActive(true);//////////////

					canMovement = false;
					animator.SetBool ("Block", true);
					animator.SetBool ("Fire", false);
					animator.SetBool ("Stun", false);
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
					animator.SetBool ("Stun", false);
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
					animator.SetBool ("Stun", true);
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
					animator.SetBool ("Stun", false);
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
		Debug.Log("Insnciere ball");
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
		if(ball == null && turn)
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
				ball = Instance(balls[5], ballSpohorn.position, this.transform.rotation);
				if(ball!=null){return true;}	
			}
			else if(transform.position.y<-3)//Special unten
			{
				ball = Instance(balls[4], ballSpohorn.position, this.transform.rotation);
				if(ball!=null){return true;}
			}
			else//speschel mitte
			{
				ball = Instance(balls[3], ballSpohorn.position, this.transform.rotation);
				if(ball!=null){return true;}
			}	
		}
		else
		{
			//normal schuss
			if (direction.y > 0f) //oben schiessen
			{
				ball = Instance(balls [1], ballSpohorn.position, this.transform.rotation);
				if(ball!=null){return true;}
			} 
			else if (direction.y < 0f) //unten schiessen
			{
				ball = Instance (balls [2], ballSpohorn.position, this.transform.rotation);
				if(ball!=null){return true;}
			} 
			else if (direction.y == 0f) //normaler schuss
			{
				ball = Instance(balls [0], ballSpohorn.position, this.transform.rotation);
				if(ball!=null){return true;}
			}
		}
		
		return false;
	}
}*/
