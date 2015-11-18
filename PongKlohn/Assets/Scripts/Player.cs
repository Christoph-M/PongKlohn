using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	//public GameObject shootLeft; 
	//public GameObject shootForward; 
	//public GameObject shootRight;
	//public GameObject shootSuppi;
	public GameObject catchCollider;
	public GameObject blockCollider;
	private GameObject catchTrigger;
	private GameObject blockTrigger;
	
	public float speed = 10;
	public bool InvertMotion = false;
	
	private Rigidbody2D myTransform;
	private Animator animator;
	private Ball projectile;

	public List<GameObject> balls;
	private GameObject ball;
	
	private string xAxis = "x";
	private string yAxis = "y";
	private string shoot = "s";
	private string block = "b";
	private string dash  = "d";

	private bool turn;

	private float motionInverter = 1;
	
	void Start() 
	{
		catchTrigger = Instantiate(catchCollider, transform.position, transform.rotation) as GameObject;
		blockTrigger = Instantiate(blockCollider, transform.position, transform.rotation) as GameObject;
		animator = GetComponent<Animator>();
		myTransform = this.GetComponent<Rigidbody2D>();
		
		catchTrigger.SetActive(true);
		blockTrigger.SetActive(false);
		
		if(InvertMotion){motionInverter = -1;}
		else{motionInverter = 1;}

		if(InvertMotion) {
			motionInverter = -1;
		} else {
			motionInverter = 1;
		}

	}
	
	private bool canMovement = true;
	void FixedUpdate() 
	{
		
		Vector2 direction = new Vector2(Input.GetAxis(xAxis),Input.GetAxis(yAxis));
		if(Input.GetAxis(block)!=0f)
		{
			canMovement = false;
			animator.SetBool("Block",true);
			blockTrigger.SetActive(true);
		}
		else
		{
			canMovement = true;
			animator.SetBool("Block",false);
			blockTrigger.SetActive(false);
		}
		
		/*if(Input.GetAxis(dash)!=0f)
		{
			animator.SetBool("Block",true);
		}*/	
		
		//Debug.Log(direction);
		if(canMovement)
		{
			animator.SetFloat("xAxis",direction.x * motionInverter);
			animator.SetFloat("yAxis",direction.y * motionInverter);
			myTransform.AddForce (direction * speed);
		}

		float shooting = Input.GetAxisRaw (shoot);
		float blocking = Input.GetAxisRaw (block);

		/*if (blocking == 1.0f) {
			blockTrigger.SetActive (true);
			
			animator.SetFloat("xAxis", 0.0f);
			animator.SetFloat("yAxis", 0.0f);
		} else {
			blockTrigger.SetActive (false);

			myTransform.AddForce (direction * Game.playerSpeed);
			
			animator.SetFloat("xAxis", direction.x * motionInverter);
			animator.SetFloat("yAxis", direction.y * motionInverter);
		}

		/*if (shooting == 1.0f && turn) {
			if (this.name == "Player_01"){
				p1Shoot();
			} else {
				p2Shoot();
			}
		}*/
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

	public void instantiateSphere(GameObject sphereO, Vector3 position, Quaternion rotation){
		ball = Instantiate(sphereO, position, rotation) as GameObject;
		
		ball.name = "Projectile";
		projectile = new Ball(ball.transform);
	}
	
	public void Shoot(GameObject ball,Vector3 position,Quaternion rotation)
	{
		float ver = Input.GetAxisRaw ("VerticalP2");
		
		Quaternion angle = Quaternion.identity;
		angle = this.transform.rotation;
		
		if (!ball) {
			if (ver > 0) {
				angle = Quaternion.AngleAxis (135.0f, Vector3.forward);
			}
			
			if (ver < 0) {
				angle = Quaternion.AngleAxis (225.0f, Vector3.forward);
			}
			
			instantiateSphere (balls[0], (Vector2)this.transform.position + new Vector2 (-Game.ballSpawnDistance, 0.0f), angle);
			//sphereC.AddForce (sphereC.gameObject.transform.TransformVector(Vector2.right) * ballSpeed, ForceMode2D.Impulse);
		}
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

	public void moveBall(){
		if (ball) {
			projectile.move (Game.ballSpeed);
		}
	}
}
