using UnityEngine;
using System.Collections;

public class GameMechanics : MonoBehaviour {
	public Transform figure1;
	public Transform figure2;
	public Transform sphereO;
	private Transform sphereC;

	public float ballSpeed = 30;

	private Game goal;
	private Ball projectile;
	private RaycastHit hit;

	void Start(){
		goal = GameObject.FindObjectOfType (typeof(Game)) as Game;
	}
	
	public int detectCollisionP1(){
		if (Physics.Raycast (figure1.position, Vector3.forward, out hit, 1.0f)) {
			Debug.DrawRay(figure1.position, Vector3.forward, Color.green);
			Debug.Log("Forward 0");
			return 0;
		}

		if (Physics.Raycast (figure1.position, Vector3.right, out hit, 1.0f)) {
			Debug.DrawRay(figure1.position, Vector3.right, Color.green);
			Debug.Log("Right 90");
			return 90;
		}

		if (Physics.Raycast (figure1.position, Vector3.back, out hit, 1.0f)) {
			Debug.DrawRay(figure1.position, Vector3.back, Color.green);
			Debug.Log("Back 180");
			return 180;
		}

		if (Physics.Raycast (figure1.position, Vector3.left, out hit, 1.0f)) {
			Debug.DrawRay(figure1.position, Vector3.left, Color.green);
			Debug.Log("Left 270");
			return 270;
		}

		return -1;
	}
	
	public int detectCollisionP2(){
		if (Physics.Raycast (figure2.position, Vector3.forward, out hit, 1.0f)) {
			Debug.DrawRay(figure2.position, Vector3.forward, Color.green);
			Debug.Log("Forward 0");
			return 0;
		}
		
		if (Physics.Raycast (figure2.position, Vector3.right, out hit, 1.0f)) {
			Debug.DrawRay(figure2.position, Vector3.right, Color.green);
			Debug.Log("Right 90");
			return 90;
		}
		
		if (Physics.Raycast (figure2.position, Vector3.back, out hit, 1.0f)) {
			Debug.DrawRay(figure2.position, Vector3.back, Color.green);
			Debug.Log("Back 180");
			return 180;
		}
		
		if (Physics.Raycast (figure2.position, Vector3.left, out hit, 1.0f)) {
			Debug.DrawRay(figure2.position, Vector3.left, Color.green);
			Debug.Log("Left 270");
			return 270;
		}
		
		return -1;
	}

	public void p1Shoot(){
		Quaternion angle = Quaternion.identity;
		angle = figure1.rotation;
		
		if (Input.GetKeyDown("space") && !sphereC){
			if(Input.GetKey("up"))
			{
				angle=Quaternion.AngleAxis(-45.0f, Vector3.up);
			}
			if(Input.GetKey("down"))
			{
				angle=Quaternion.AngleAxis(-135.0f, Vector3.up);
			}
			
			//angle += player1.GetProjectileRotation();
			sphereC = Instantiate(sphereO, figure1.position, angle) as Transform;
			
			sphereC.name = "Projectile";
			projectile = new Ball (sphereC);
		}
	}

	public void p2Shoot(){
		Quaternion angle = Quaternion.identity;
		angle = figure2.rotation;
		
		if (Input.GetKeyDown("right ctrl") && !sphereC){
			if(Input.GetKey("up"))
			{
				angle=Quaternion.AngleAxis(-45.0f, Vector3.up);
			}
			if(Input.GetKey("down"))
			{
				angle=Quaternion.AngleAxis(-135.0f, Vector3.up);
			}
			
			//angle += player2.GetProjectileRotation();
			sphereC = Instantiate(sphereO, figure2.position, angle) as Transform;
			
			sphereC.name = "Projectile";
			projectile = new Ball (sphereC);
		}
	}

	public void detectBallBlockP1(){
		if (Input.GetKeyDown("left shift") && Physics.Raycast(figure1.position, Vector3.right, out hit, 2)){
			goal.setTriggeredGoal("Goal_Red");
			Vector3 direction = Vector3.Reflect(sphereC.forward, figure1.InverseTransformVector(Vector3.forward));
			sphereC.rotation = Quaternion.LookRotation(direction);
		}
	}
	
	public void detectBallBlockP2(){
		if (Input.GetKeyDown("right shift") && Physics.Raycast(figure2.position, Vector3.left, out hit, 2)){
			goal.setTriggeredGoal("Goal_Blue");
			Vector3 direction = Vector3.Reflect(sphereC.forward, figure2.InverseTransformVector(Vector3.forward));
			sphereC.rotation = Quaternion.LookRotation(direction);
		}
	}

	public void moveBall(){
		if (sphereC) {
			projectile.move (ballSpeed);
		}
	}
}
