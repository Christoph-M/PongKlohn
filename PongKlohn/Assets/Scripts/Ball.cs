using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
	private Game goal;

	private Transform myTransform;
	
	void Start(){
		goal = GameObject.FindObjectOfType (typeof(Game)) as Game;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.name == "Goal_Red" || other.name == "Goal_Blue" ||
			other.name == "Catch_Trigger_Player_01" || other.name == "Catch_Trigger_Player_02") {
			if (other.name == "Goal_Red" || other.name == "Catch_Trigger_Player_01") {
				goal.setTurn (true);
			} else {
				goal.setTurn (false);
			}

			Object.Destroy (this.gameObject);
		} else if (other.name == "Wall_01" || other.name == "Wall_02") {
			float distance = this.transform.right.magnitude;
			Vector2 forwardL = this.transform.right / distance;
			Vector2 forwardG = this.transform.TransformDirection(forwardL);

			RaycastHit2D hit = Physics2D.Raycast (this.transform.position, forwardG);
			Vector2 exitDirection = Vector2.Reflect(forwardL, hit.normal);
			float angle = Mathf.Atan2(exitDirection.y, exitDirection.x) * Mathf.Rad2Deg;

			this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
	}

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
//		myTransform.position = Vector2.Lerp (myTransform.position, Vector2.right  * Time.deltaTime, speed);
		myTransform.Translate ((Vector2.right * Time.deltaTime) * speed);
		//myTransform.AddForce ((myTransform.gameObject.transform.TransformVector(Vector2.right) * Time.deltaTime) * speed, ForceMode2D.Impulse);
		//myTransform.localPosition += (myTransform.TransformVector(Vector2.up) * Time.deltaTime) * speed;
	}
}
