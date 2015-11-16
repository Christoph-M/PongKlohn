using UnityEngine;
using System.Collections;

public class GameMechanics : MonoBehaviour {
	public Transform figure1;
	public Transform figure2;
	public GameObject sphereRO;
	public GameObject sphereBO;
	public Transform catchTriggerO;
	public Transform blockTriggerO;

	public float ballSpeed = 2.0f;
	public float ballSpawnDistance = 1.2f;
	
	private Game goal;
	private Ball projectile;

	private GameObject sphereC;
	private Transform catchTriggerP1C;
	private Transform catchTriggerP2C;
	private Transform blockTriggerP1C;
	private Transform blockTriggerP2C;
	private RaycastHit hit;

	void Start(){
		goal = GameObject.FindObjectOfType (typeof(Game)) as Game;

		catchTriggerP1C = Instantiate (catchTriggerO, figure1.position, figure1.rotation) as Transform;
		catchTriggerP2C = Instantiate (catchTriggerO, figure2.position, figure2.rotation) as Transform;
		catchTriggerP1C.name = "Catch_Trigger_Player_01";
		catchTriggerP2C.name = "Catch_Trigger_Player_02";

		blockTriggerP1C = Instantiate (blockTriggerO, figure1.position, figure1.rotation) as Transform;
		blockTriggerP2C = Instantiate (blockTriggerO, figure2.position, figure2.rotation) as Transform;
		blockTriggerP1C.name = "Block_Trigger_Player_01";
		blockTriggerP2C.name = "Block_Trigger_Player_02";
		blockTriggerP1C.gameObject.layer = 8;
		blockTriggerP2C.gameObject.layer = 8;
	}

	void Update(){
		catchTriggerP1C.position = figure1.position;
		catchTriggerP2C.position = figure2.position;

		blockTriggerP1C.position = figure1.position;
		blockTriggerP2C.position = figure2.position;
	}

	public void instantiateSphere(GameObject sphereO, Vector3 position, Quaternion rotation){
		sphereC = Instantiate(sphereO, position, rotation) as GameObject;

		sphereC.name = "Projectile";
		projectile = new Ball (sphereC.transform);
	}

	public void p1Shoot(){
		float space = Input.GetAxisRaw ("ShootP1");
		float ver = Input.GetAxisRaw("VerticalP1");

		Quaternion angle = Quaternion.identity;
		angle = figure1.rotation;
		
		if (space > 0 && !sphereC){
			if(ver > 0){
				angle=Quaternion.AngleAxis(45.0f, Vector3.forward);
			}

			if(ver < 0){
				angle=Quaternion.AngleAxis(-45.0f, Vector3.forward);
			}

			instantiateSphere(sphereRO, (Vector2)figure1.position + new Vector2(ballSpawnDistance, 0.0f), angle);
			//sphereC.AddForce (sphereC.gameObject.transform.TransformVector(Vector2.right) * ballSpeed, ForceMode2D.Impulse);
		}
	}

	public void p2Shoot(){
		float rCntrl = Input.GetAxisRaw ("ShootP2");
		float ver = Input.GetAxisRaw("VerticalP2");

		Quaternion angle = Quaternion.identity;
		angle = figure2.rotation;
		
		if (rCntrl > 0 && !sphereC){
			if(ver > 0){
				angle=Quaternion.AngleAxis(135.0f, Vector3.forward);
			}

			if(ver < 0){
				angle=Quaternion.AngleAxis(225.0f, Vector3.forward);
			}

			instantiateSphere(sphereBO, (Vector2)figure2.position + new Vector2(-ballSpawnDistance, 0.0f), angle);
			//sphereC.AddForce (sphereC.gameObject.transform.TransformVector(Vector2.right) * ballSpeed, ForceMode2D.Impulse);
		}
	}

	public void moveBall(){
		if (sphereC) {
			projectile.move (ballSpeed);
		}
	}
}
