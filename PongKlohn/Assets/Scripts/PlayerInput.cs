using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
	public Transform figure1;
	public Transform figure2;

	public float playerSpeed = 5;

	private Player player1;
	private Player player2;

	void Start(){
		player1 = new Player (figure1);
		player2 = new Player (figure2);
	}

	public void moveP1(int collDirection){
		float horP1 = Input.GetAxisRaw ("HorizontalP1");
		float verP1 = Input.GetAxisRaw ("VerticalP1");
		
		switch (collDirection) {
			case 0: verP1 -= 1; break;
			case 90: horP1 -= 1; break;
			case 180: verP1 += 1; break;
			case 270: horP1 += 1; break;
			default: break;
		}

		Vector3 vecP1 = new Vector3 (horP1, 0.0f, verP1);

		player1.Move (vecP1, playerSpeed);
	}

	public void moveP2(int collDirection){
		float horP2 = Input.GetAxisRaw ("HorizontalP2");
		float verP2 = Input.GetAxisRaw ("VerticalP2");
		
		switch (collDirection) {
		case 0: verP2 -= 1; break;
		case 90: horP2 -= 1; break;
		case 180: verP2 += 1; break;
		case 270: horP2 += 1; break;
		default: break;
		}

		Vector3 vecP2 = new Vector3 (horP2, 0.0f, verP2);

		player2.Move (vecP2, playerSpeed);
	}
}
