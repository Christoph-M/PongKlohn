using UnityEngine;
using System.Collections;

public class GameMechanics : MonoBehaviour {
	public Transform figure1;
	public Transform figure2;
	public Transform sphereO;

	public float ballSpeed = 30;
	
	private Game goal;
	private Ball projectile;

	private Transform sphereC;
	private RaycastHit hit;

	void Start(){
		goal = GameObject.FindObjectOfType (typeof(Game)) as Game;
	}

	public void p1Shoot(){
		float space = Input.GetAxisRaw ("ShootP1");
		float ver = Input.GetAxisRaw("VerticalP1p") + Input.GetAxisRaw ("VerticalP1n");

		Quaternion angle = Quaternion.identity;
		angle = figure1.rotation;
		
		if (space > 0 && !sphereC){
			if(ver > 0){
				angle=Quaternion.AngleAxis(45.0f, Vector3.up);
			}

			if(ver < 0){
				angle=Quaternion.AngleAxis(135.0f, Vector3.up);
			}
			
			sphereC = Instantiate(sphereO, figure1.position, angle) as Transform;
			
			sphereC.name = "Projectile";
			projectile = new Ball (sphereC);
		}
	}

	public void p2Shoot(){
		float rCntrl = Input.GetAxisRaw ("ShootP2");
		float ver = Input.GetAxisRaw("VerticalP2p") + Input.GetAxisRaw ("VerticalP2n");

		Quaternion angle = Quaternion.identity;
		angle = figure2.rotation;
		
		if (rCntrl > 0 && !sphereC){
			if(ver > 0){
				angle=Quaternion.AngleAxis(-45.0f, Vector3.up);
			}

			if(ver < 0){
				angle=Quaternion.AngleAxis(-135.0f, Vector3.up);
			}
			
			sphereC = Instantiate(sphereO, figure2.position, angle) as Transform;
			
			sphereC.name = "Projectile";
			projectile = new Ball (sphereC);
		}
	}

	public void detectBallBlockP1(){
		float lShift = Input.GetAxisRaw ("BlockP1");

		if (lShift > 0 && Physics.Raycast (figure1.position, Vector3.right, out hit, 2)) {
			goal.setTriggeredGoal ("Goal_Red");

			Vector3 normal = hit.normal;
			if (hit.normal.y != 0) normal.y = 0;

			Vector3 direction = Vector3.Reflect (figure1.forward, sphereC.InverseTransformVector (normal));
			sphereC.rotation = Quaternion.LookRotation (direction);
		}
	}
	
	public void detectBallBlockP2(){
		float rShift = Input.GetAxisRaw ("BlockP2");

		if (rShift > 0 && Physics.Raycast(figure2.position, Vector3.left, out hit, 2)){
			goal.setTriggeredGoal("Goal_Blue");

			Vector3 normal = hit.normal;
			if (hit.normal.y != 0) normal.y = 0;

			Vector3 direction = Vector3.Reflect(figure2.forward, sphereC.InverseTransformVector (normal));
			sphereC.rotation = Quaternion.LookRotation(direction);
		}
	}

	public void moveBall(){
		if (sphereC) {
			projectile.move (ballSpeed);
		}
	}
}
