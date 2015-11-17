using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	//public GameObject shootLeft; 
	//public GameObject shootForward; 
	//public GameObject shootRight;
	//public GameObject shootSuppi;
	public GameObject sphereO;
	public Transform catchTriggerO;
	public Transform blockTriggerO;
	
	public float speed = 10;
	public bool InvertMotion = false;
	
	private Rigidbody2D myTransform;
	private Animator animator;
	private Ball projectile;

	private GameObject sphereC;
	private Transform catchTriggerC;
	private Transform blockTriggerC;
	
	private string xAxis = "x";
	private string yAxis = "y";
	private string shoot = "s";
	private string block = "b";
	private string dash  = "d";

	private bool turn;

	private float motionInverter = 1;
	
	void Start() {
		animator = GetComponent<Animator>();
		myTransform = this.GetComponent<Rigidbody2D>();

		catchTriggerC = Instantiate (catchTriggerO, this.transform.position, this.transform.rotation) as Transform;
		catchTriggerC.parent = this.transform;
		catchTriggerC.name = "Catch_Trigger";
		
		blockTriggerC = Instantiate (blockTriggerO, this.transform.position, this.transform.rotation) as Transform;
		blockTriggerC.gameObject.SetActive (false);
		blockTriggerC.parent = this.transform;
		blockTriggerC.name = "Block_Trigger";

		if(InvertMotion) {
			motionInverter = -1;
		} else {
			motionInverter = 1;
		}
	}
	
	void FixedUpdate() {
		Vector2 direction = new Vector2(Input.GetAxis(xAxis),Input.GetAxis(yAxis));
		float shooting = Input.GetAxisRaw (shoot);
		float blocking = Input.GetAxisRaw (block);

		if (blocking == 1.0f) {
			blockTriggerC.gameObject.SetActive (true);
			
			animator.SetFloat("xAxis", 0.0f);
			animator.SetFloat("yAxis", 0.0f);
		} else {
			blockTriggerC.gameObject.SetActive (false);

			myTransform.AddForce (direction * Game.playerSpeed);
			
			animator.SetFloat("xAxis", direction.x * motionInverter);
			animator.SetFloat("yAxis", direction.y * motionInverter);
		}

		if (shooting == 1.0f && turn) {
			if (this.name == "Player_01"){
				p1Shoot();
			} else {
				p2Shoot();
			}
		}
	}

	void Update(){
		catchTriggerC.position = this.transform.position;
		blockTriggerC.position = this.transform.position;
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
		sphereC = Instantiate(sphereO, position, rotation) as GameObject;
		
		sphereC.name = "Projectile";
		projectile = new Ball(sphereC.transform);
	}
	
	public void p1Shoot(){
		float ver = Input.GetAxisRaw("VerticalP1");
		
		Quaternion angle = Quaternion.identity;
		angle = this.transform.rotation;
		
		if (!sphereC){
			if(ver > 0){
				angle=Quaternion.AngleAxis(45.0f, Vector3.forward);
			}
			
			if(ver < 0){
				angle=Quaternion.AngleAxis(-45.0f, Vector3.forward);
			}
			
			instantiateSphere(sphereO, (Vector2)this.transform.position + new Vector2(Game.ballSpawnDistance, 0.0f), angle);
			//sphereC.AddForce (sphereC.gameObject.transform.TransformVector(Vector2.right) * ballSpeed, ForceMode2D.Impulse);
		}
	}
	
	public void p2Shoot(){
		float ver = Input.GetAxisRaw ("VerticalP2");
		
		Quaternion angle = Quaternion.identity;
		angle = this.transform.rotation;
		
		if (!sphereC) {
			if (ver > 0) {
				angle = Quaternion.AngleAxis (135.0f, Vector3.forward);
			}
			
			if (ver < 0) {
				angle = Quaternion.AngleAxis (225.0f, Vector3.forward);
			}
			
			instantiateSphere (sphereO, (Vector2)this.transform.position + new Vector2 (-Game.ballSpawnDistance, 0.0f), angle);
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
		if (sphereC) {
			projectile.move (Game.ballSpeed);
		}
	}
}
