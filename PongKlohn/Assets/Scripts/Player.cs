using UnityEngine;
using System.Collections;

public class Player {
	private Rigidbody2D myTransform;

	public Player(Rigidbody2D trans) {
		myTransform = trans;
	}

	public void Move(Vector2 direction, float speed) {
		myTransform.AddForce (direction * speed);
		//myTransform.position += (direction * Time.deltaTime) * speed;
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
