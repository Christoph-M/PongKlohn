using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
	public Transform figure1;
	public Transform figure2;

	public float playerSpeed = 5;
	public float collisionDistance = 0.6f;

	private Player player1;
	private Player player2;
	private RaycastHit hit;

	void Start(){
		player1 = new Player (figure1);
		player2 = new Player (figure2);
	}

	public void moveP1(){
		float horP = Input.GetAxis ("HorizontalP1p");
		float verP = Input.GetAxis ("VerticalP1p");
		float horN = Input.GetAxis ("HorizontalP1n");
		float verN = Input.GetAxis ("VerticalP1n");

		if (Physics.Raycast (figure1.position, Vector3.forward, out hit, collisionDistance) &&
		    hit.transform.gameObject.name != "Projectile")
			verP = 0;
		
		if (Physics.Raycast (figure1.position, Vector3.right, out hit, collisionDistance) &&
		    hit.transform.gameObject.name != "Projectile")
			horP = 0;
		
		if (Physics.Raycast (figure1.position, Vector3.back, out hit, collisionDistance) &&
		    hit.transform.gameObject.name != "Projectile")
			verN = 0;
		
		if (Physics.Raycast (figure1.position, Vector3.left, out hit, collisionDistance) &&
		    hit.transform.gameObject.name != "Projectile")
			horN = 0;

		float hor = horP + horN;
		float ver = verP + verN;

		Vector3 vec = new Vector3 (hor, 0.0f, ver);

		player1.Move (vec, playerSpeed);
	}

	public void moveP2(){
		float horP = Input.GetAxis ("HorizontalP2p");
		float verP = Input.GetAxis ("VerticalP2p");
		float horN = Input.GetAxis ("HorizontalP2n");
		float verN = Input.GetAxis ("VerticalP2n");
		
		if (Physics.Raycast (figure2.position, Vector3.forward, out hit, collisionDistance) &&
		    hit.transform.gameObject.name != "Projectile")
			verP = 0;
		
		if (Physics.Raycast (figure2.position, Vector3.right, out hit, collisionDistance) &&
		    hit.transform.gameObject.name != "Projectile")
			horP = 0;
		
		if (Physics.Raycast (figure2.position, Vector3.back, out hit, collisionDistance) &&
		    hit.transform.gameObject.name != "Projectile")
			verN = 0;
		
		if (Physics.Raycast (figure2.position, Vector3.left, out hit, collisionDistance) &&
		    hit.transform.gameObject.name != "Projectile")
			horN = 0;
		
		float hor = horP + horN;
		float ver = verP + verN;

		Vector3 vec = new Vector3 (hor, 0.0f, ver);

		player2.Move (vec, playerSpeed);
	}
}
