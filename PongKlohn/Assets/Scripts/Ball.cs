using UnityEngine;
using System.Collections;

public class Ball {
	private Transform myTransform;

	public Ball(Transform trans) {
		myTransform = trans;
	}
	
	public void Set(Vector3 position, Quaternion rotation) {
		myTransform.position = position;
		myTransform.rotation = rotation;
	}
	
	public Vector3 GetPosition() {
		return myTransform.position;
	}
	
	public Quaternion GetRotaion() {
		return myTransform.rotation;
	}
	
	public void move(float speed){
		//myTransform.rotation = rotation;
		myTransform.localPosition += (myTransform.TransformVector(Vector3.forward) * Time.deltaTime) * speed; //(direction * Time.deltaTime) * speed;
	}
}
