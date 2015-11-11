using UnityEngine;
using System.Collections;

public class Ball {
	private Rigidbody2D myTransform;

	public Ball(Rigidbody2D trans) {
		myTransform = trans;
	}
	
	public void Set(Vector3 position, Quaternion rotation) {
		myTransform.gameObject.transform.position = position;
		myTransform.gameObject.transform.rotation = rotation;
	}
	
	public Vector3 GetPosition() {
		return myTransform.gameObject.transform.position;
	}
	
	public Quaternion GetRotaion() {
		return myTransform.gameObject.transform.rotation;
	}
	
	public void move(float speed){
		//myTransform.gameObject.transform.Translate ((Vector2.right * Time.deltaTime) * speed);
		//myTransform.AddForce ((myTransform.gameObject.transform.TransformVector(Vector2.right) * Time.deltaTime) * speed, ForceMode2D.Impulse);
		//myTransform.localPosition += (myTransform.TransformVector(Vector2.up) * Time.deltaTime) * speed;
	}
}
