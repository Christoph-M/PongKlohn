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


	private Rigidbody2D myTransform;
	private Animator animator;
	private Ball projectile;
	
	private GameObject ball;

	private GameObject catchTrigger;
	private GameObject blockTrigger;
	private GameObject dashTrigger;
		
	private string xAxis = "x";
	private string yAxis = "y";
	private string shoot = "s";
	private string block = "b";
	private string dash  = "d";

	private bool turn;

	private float motionInverter = 1;
	
	void Start() 
	{
		animator = GetComponent<Animator>();
		myTransform = this.GetComponent<Rigidbody2D>();

		catchTrigger = Instantiate(catchCollider, transform.position, transform.rotation) as GameObject;
		blockTrigger = Instantiate(blockCollider, transform.position, transform.rotation) as GameObject;
		dashTrigger = Instantiate(dashCollider, transform.position, transform.rotation) as GameObject;
		catchTrigger.transform.parent = this.transform;
		blockTrigger.transform.parent = this.transform;
		dashTrigger.transform.parent = this.transform;
		catchTrigger.SetActive(true);
		blockTrigger.SetActive(false);
		dashTrigger.SetActive(false);
		catchTrigger.name = "Catch_Trigger";
		blockTrigger.name = "Block_Trigger";
		dashTrigger.name = "Dash_Trigger";
		
		if (InvertMotion) {
			motionInverter = -1;
		} else {
			motionInverter = 1;
		}
	}
	
	private bool canMovement = true;
	private bool axisInUse = false;
	private bool fired = false;
	private float timeLeft = 1.0f;
	void FixedUpdate() 
	{
		Vector2 direction = new Vector2(Input.GetAxis(xAxis), Input.GetAxis(yAxis));
		Vector2 directionRaw = new Vector2(Input.GetAxisRaw(xAxis), Input.GetAxisRaw(yAxis));

		if (Input.GetAxis(shoot) != 0f)
		{
			animator.SetBool("Fire", true);
			canMovement = false;
		}
		else
		{
			canMovement = true;
			animator.SetBool("Fire", false);
		}

		if (Input.GetAxis (block) != 0f && directionRaw.x == 0f && directionRaw.y == 0f) {
			canMovement = false;
			animator.SetBool ("Block", true);
			blockTrigger.SetActive (true);

			axisInUse = false;
		} else if (Input.GetAxis (block) != 0f) {
			canMovement = false;

			if (!axisInUse) {
				animator.SetBool ("Block", false);
				animator.SetFloat("xAxis", direction.x * motionInverter);
				animator.SetFloat("yAxis", direction.y * motionInverter);
				myTransform.AddForce (directionRaw * dashSpeed, ForceMode2D.Impulse);

				axisInUse = true;
			}
		} else {
			animator.SetBool("Block", false);
			blockTrigger.SetActive(false);
			canMovement = true;
			
			axisInUse = false;
		}

		if (canMovement)
		{
			animator.SetFloat("xAxis", direction.x * motionInverter);
			animator.SetFloat("yAxis", direction.y * motionInverter);
			myTransform.AddForce (direction * speed);
		}


		if (Input.GetAxis (shoot) != 0f && Input.GetAxisRaw (yAxis) == 0f && turn && !ball) {//mitte schiessen
			timeLeft -= Time.deltaTime;
			fired = true;
		} else if (fired && turn && !ball) {
			if (timeLeft > 0) {
				Debug.Log("Regular");
				if (Input.GetAxisRaw (yAxis) == 1f) {//oben schiessen
					ball = Shoot (balls [1], ballSpohorn.position, this.transform.rotation);
				} else if (Input.GetAxisRaw (yAxis) == -1f) {//unten schiessen
					ball = Shoot (balls [2], ballSpohorn.position, this.transform.rotation);
				} else {
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

		Debug.Log (timeLeft);
	}

	void Update()
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
