using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	public Transform ballSpohorn;
	public List<GameObject> balls;

	public GameObject catchCollider;
	public GameObject blockCollider;
	public GameObject dashCollider;
	
	public float speed { get; set; }
	public float dashSpeed { get; set; }
	public bool InvertMotion = false;
	
	private Timer catchTimer;
	private Timer blockTimer;
	private Timer fireTimer;

	private Rigidbody2D myTransform;
	private Animator animator;
	
	private GameObject ball;

	private GameObject catchTrigger;
	private GameObject blockTrigger;
	private GameObject dashTrigger;
	private GameObject missTrigger;
		
	private string xAxis = "x";
	private string yAxis = "y";
	private string shoot = "s";
	private string block = "b";
	private string dash  = "d";

	private bool turn;

	private float motionInverter = 1;
	
	void Start() 
	{
		catchTimer = new Timer();
		blockTimer  = new Timer();
		fireTimer = new Timer();
		
		animator = GetComponent<Animator>();
		myTransform = this.GetComponent<Rigidbody2D>();
		
		var children = taube.GetComponentsInChildren<GameObject>();// finde Trigger  
		foreach (var child in children)
		{
			if (child.name == "Catch_Trigger")
			{
				catchTrigger = child;
			}
			
			if (child.name == "Block_Trigger")
			{
				blockTrigger = child;
			}
			
			if (child.name == "Dash_Trigger")
			{
				dashTrigger = child;
			}
			
			if (child.name == "Miss_Trigger")
			{
				missTrigger = child;
			}
		}

		if (InvertMotion) //Spieler Steht Links oder Recht   Steuerung anpassen
		{
			motionInverter = -1;
		} else {
			motionInverter = 1;
		}
	}
	
	private bool dashHasBeenTriggert = false;
	private bool zuLangsamZumFangenDuMong = false;
	private bool canMovement = true;
	private bool axisInUse = false;
	private bool fired = false;
	private float timeLeft = 1.0f;
	
	private bool isStunned = false;
	private bool onFireKeyDown = true;
	private bool onBlockKeyDown = true;
	
	void Update() 
	{
		/////////////////INPUT
		
		if(isStunned)
		{
			animator.SetBool("Stunn");
		}
		
		else
		{
			Vector2 direction = new Vector2(Input.GetAxis(xAxis), Input.GetAxis(yAxis));//zuweisung der Inputachsen
			Vector2 directionRaw = new Vector2(Input.GetAxisRaw(xAxis), Input.GetAxisRaw(yAxis));

			if (Input.GetAxis(shoot) != 0f)//Intput   Feuerachse
			{
				timeLeft -= Time.deltaTime;
				animator.SetBool("Fire", true);
				if(onFireKeyDown && fireTimer.IsFinished())
				{
					fireTimer.SetTimer(1);
				}
				canMovement = false;
			}
			
			else
			{
				animator.SetBool("Fire", false);
				canMovement = true;
			}
			
			if (Input.GetAxis(block) != 0f)// input Block
			{
				canMovement = false;
				//if(onBlockKeyDown)
				//{
					blockTimer(0.5f);
					
				//}	
				animator.SetBool ("Block", true);
			}
			else
			{
				canMovement = true;
				animator.SetBool ("Block", false);
			}
			
			//////////////Output
			
			if(fireTimer.IsFinished() && turn && !ball)//Shoot
			{
				fireTimer.RestTimer();
				canMovement = false;
				catchTrigger.SetActive(false);
				blockTrigger.SetActive(false);
				dashTrigger.SetActive(false);
				
				if(timeLeft<=0.1)
				{
					if(transform.position.y>3){ball = Shoot(balls[5], ballSpohorn.position, this.transform.rotation);}//Spezial oban
					else if(transform.position.y<-3){ball = Shoot(balls[4], ballSpohorn.position, this.transform.rotation);}//Special unten
					else{ball = Shoot(balls[3], ballSpohorn.position, this.transform.rotation);}//speschel mitte
					//speschel schuss
				}
				else
				{
					//normal schuss
					if (Input.GetAxisRaw (yAxis) == 1f) {//oben schiessen
						ball = Shoot (balls [1], ballSpohorn.position, this.transform.rotation);
					} else if (Input.GetAxisRaw (yAxis) == -1f) {//unten schiessen
						ball = Shoot (balls [2], ballSpohorn.position, this.transform.rotation);
					} else if (Input.GetAxisRaw (yAxis) == 0f) {//normaler schuss
						ball = Shoot (balls [0], ballSpohorn.position, this.transform.rotation);
					}
				}
			}
			else
			{
				catchTrigger.SetActive(true);
			}
			
			
			//Block
			if (blockTimer.IsFinished() == false) //Input Blocktaste
			{
				canMovement = false;
				missTrigger.SetActive(true);
				if(!zuLangsamZumFangenDuMong)
				{
					blockTrigger.SetActive (true);
					missTrigger.SetActive(false);
				}
				catchTrigger.SetActive (true);
				

				axisInUse = false;
			} else if (Input.GetAxisRaw (block) != 0f) {
				canMovement = false;

				if (!axisInUse) {
					
					animator.SetFloat("xAxis", direction.x * motionInverter);
					animator.SetFloat("yAxis", direction.y * motionInverter);
					myTransform.AddForce (directionRaw * dashSpeed, ForceMode2D.Impulse);

					axisInUse = true;
				} else {
					animator.SetBool ("Block", true);
					blockTrigger.SetActive (true);
				}
			} else if (Input.GetAxisRaw (block) == 0f) {
				animator.SetBool("Block", false);
				blockTrigger.SetActive(false);
				canMovement = true;
				
				axisInUse = false;
			}



			if (Input.GetAxis (shoot) != 0f && turn && !ball) {//mitte schiessen
				
				fired = true;
			} else if (fired && turn && !ball) {
				if (timeLeft > 0) {
					Debug.Log("Regular");
					if (Input.GetAxisRaw (yAxis) == 1f) {//oben schiessen
						ball = Shoot (balls [1], ballSpohorn.position, this.transform.rotation);
					} else if (Input.GetAxisRaw (yAxis) == -1f) {//unten schiessen
						ball = Shoot (balls [2], ballSpohorn.position, this.transform.rotation);
					} else if (Input.GetAxisRaw (yAxis) == 0f) {
						ball = Shoot (balls [0], ballSpohorn.position, this.transform.rotation);
					}

					timeLeft = 1.0f;
					fired = false;
				} else if (turn && !ball) {
					Debug.Log("Special");
					ball = Shoot(balls[3], ballSpohorn.position, this.transform.rotation);

					timeLeft = 1.0f;
					fired = false;
				}
			}
			
			if (canMovement)// Bewegt sen spieler
			{
				animator.SetFloat("xAxis", direction.x * motionInverter);
				animator.SetFloat("yAxis", direction.y * motionInverter);
				myTransform.AddForce (direction * speed);
			}
			
			fireTimer.Update();
			blockTimer.Update();
			catchTimer.Update();
			Debug.Log (timeLeft);
		}
	}

	void FixedUpdate()
	{
		catchTrigger.transform.position = this.transform.position;
		blockTrigger.transform.position = this.transform.position;
	}
	
	public void SetInputAxis(string x,string y, string s, string b) {
		xAxis = x;
		yAxis = y;
		shoot = s;
		block = b;
	}
	
	public void SetZuLangsamZumFangenDuMong(bool zLZFDM)
	{
		zuLangsamZumFangenDuMong = zLZFDM;
	}
	
	public void SetDashTrigger(bool dT)
	{
		dashHasBeenTriggert = dt;
	}
	
	public bool setTurn(bool turnt){
		return turn = turnt;
	}

	public GameObject Shoot(GameObject ball, Vector3 position, Quaternion rotation){
		return Instantiate(ball, position, rotation) as GameObject;
	}
	
	public Vector3 GetProjectilePositin() {
		return myTransform.gameObject.transform.position;
	}
	
	public Quaternion Rotation() {
		return myTransform.gameObject.transform.rotation;
	}
	
	public Vector3 GetRotation(){
		return new Vector3(myTransform.gameObject.transform.rotation.x, myTransform.gameObject.transform.rotation.y, myTransform.gameObject.transform.rotation.z);
	}
}
