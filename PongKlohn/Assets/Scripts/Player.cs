using UnityEngine;
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
	private bool isBlocking = false;
	private bool onZuLangsamZumFangenDuMongDown = true;
	private int action = 0;
	private bool isInAction = false;
	
	void Update() 
	{
		if(zuLangsamZumFangenDuMong)
		{
			isStunned = true;
			zuLangsamZumFangenDuMong = false;
			stunTimer.SetTimer(2);
		}
		
		/////////////////INPUT
		if(isStunned)
		{
			Debug.Log("Player is Stunned or Controls havend be initialice");
			action = 4;
			if(stunTimer.IsFinished())
			{
				isStunned = false;
			}
		}
		else//Wenn der Spieler nicht gerade gestunde ist
		{
			direction = controls.UpdateMovement();//zuweisung der Inputachsen
			directionRaw = controls.UpdateMovement();
			
			if(controls.IsActionKeyActive() || isInAction)
			{
				if(controls.IsFireKeyActive(turn)){timeLeft += Time.deltaTime;}////////////SHOOT
				else{timeLeft = 0f;}
				if (controls.IsFireKeyActive(turn) && !fireKeyPressed && !ball)
				{
					isInAction = true;
					action = 3;
					fireTimer.SetTimer(0.7f);
					fireKeyPressed = true;
				}
				if (fireKeyPressed && !ball && fireTimer.IsFinished() && turn) 
				{
					this.Shoot(direction);
					fireKeyPressed = false;
					waitAfterSoot.SetTimer(1);
				}
				
				if (controls.IsBlockKeyActive())/////////////BLOCK
				{
					blockTimer.SetTimer(0.5f);
					if(isBlocking == false)
					{
						isBlocking = true;
						action = 1;
					}
					else
					{
						action = 2;
					}
				}
				
				if(blockTimer.IsFinished()== false && waitAfterSoot.IsFinished())
				{
					isBlocking = false;
					isInAction = false;
				}
			}
			
			else
			{
				canMovement = true;
				action = 0;
				animator.SetBool ("Block", false);
				animator.SetBool ("Fire", false);
			}
			
			//////////////////////////////////////////////////Movements
		}
		
		if(controls.IsBlockKeyActive() && dashHasBeenTriggert)
		{
			animator.SetFloat("xAxis", direction.x * motionInverter);
			animator.SetFloat("yAxis", direction.y * motionInverter);
			myTransform.AddForce (directionRaw * dashSpeed, ForceMode2D.Impulse);
		}
		
		if (blockTimer.IsFinished())// Bewegt den spieler
		{
			animator.SetFloat("xAxis", direction.x * motionInverter);
			animator.SetFloat("yAxis", direction.y * motionInverter);
			myTransform.AddForce (direction * speed);
		}
	
		SetTrigger(action);
		stunTimer.UpdateTimer();
		fireTimer.UpdateTimer();
		blockTimer.UpdateTimer();
		catchTimer.UpdateTimer();
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
					animator.SetBool ("stun", false);
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
					animator.SetBool ("stun", false);
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
					animator.SetBool ("stun", false);
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
					animator.SetBool ("stun", false);
					oldState = newState;
					break;
				
				case 4://stun
					catchTrigger.SetActive(false);
					missTrigger.SetActive(true);/////////
					blockTrigger.SetActive(false);
					dashTrigger.SetActive(false);

					canMovement = false;
					animator.SetBool ("Block", false);
					animator.SetBool ("Fire", true);
					animator.SetBool ("stun", false);
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

	public GameObject Shoot(GameObject ball, Vector3 position, Quaternion rotation)
	{
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
	
	private void Shoot(Vector2 direction)
	{
		if(controls.IsFireKeyActive(turn) && fireTimer.IsFinished() && !ball)//Shoot
		{
			//fireTimer.ResetTimer();

			
			if(timeLeft>=0.9)
			{
				if(transform.position.y>3){ball = Shoot(balls[5], ballSpohorn.position, this.transform.rotation);}//Spezial oban
				else if(transform.position.y<-3){ball = Shoot(balls[4], ballSpohorn.position, this.transform.rotation);}//Special unten
				else{ball = Shoot(balls[3], ballSpohorn.position, this.transform.rotation);}//speschel mitte
				//speschel schuss
			}
			else
			{
				//normal schuss
				if (direction.y > 0f) {//oben schiessen
					ball = Shoot (balls [1], ballSpohorn.position, this.transform.rotation);
				} else if (direction.y < 0f) {//unten schiessen
					ball = Shoot (balls [2], ballSpohorn.position, this.transform.rotation);
				} else if (direction.y == 0f) {//normaler schuss
					ball = Shoot (balls [0], ballSpohorn.position, this.transform.rotation);
				}
			}
		}
	}
}
