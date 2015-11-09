using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
	public Transform figure1;
	public Transform figure2;

	public float playerSpeed = 5;

	private Player player1;
	private Player player2;
	private RaycastHit hit;

	void Start(){
		player1 = new Player (figure1);
		player2 = new Player (figure2);
	}

	public void moveP1(){
		float hor = Input.GetAxis ("HorizontalP1");
		float ver = Input.GetAxis ("VerticalP1");

		if (Physics.Raycast (figure1.position, Vector3.forward, out hit, 1.0f))
			ver -= 1;
		
		if (Physics.Raycast (figure1.position, Vector3.right, out hit, 1.0f))
			hor -= 1;
		
		if (Physics.Raycast (figure1.position, Vector3.back, out hit, 1.0f))
			ver += 1;
		
		if (Physics.Raycast (figure1.position, Vector3.left, out hit, 1.0f))
			hor += 1;

		Vector3 vec = new Vector3 (hor, 0.0f, ver);

		player1.Move (vec, playerSpeed);
	}

	public void moveP2(){
		float hor = Input.GetAxis ("HorizontalP2");
		float ver = Input.GetAxis ("VerticalP2");
		
		if (Physics.Raycast (figure2.position, Vector3.forward, out hit, 1.0f))
			ver -= 1;
		
		if (Physics.Raycast (figure2.position, Vector3.right, out hit, 1.0f))
			hor -= 1;
		
		if (Physics.Raycast (figure2.position, Vector3.back, out hit, 1.0f))
			ver += 1;
		
		if (Physics.Raycast (figure2.position, Vector3.left, out hit, 1.0f))
			hor += 1;

		Vector3 vec = new Vector3 (hor, 0.0f, ver);

		player2.Move (vec, playerSpeed);
	}
}
