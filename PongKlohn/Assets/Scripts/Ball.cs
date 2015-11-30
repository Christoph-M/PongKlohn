using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
	public LayerMask mask = -1;

	private Game goal;

	private Transform myTransform;

	Vector2 positionLastFrame;

	private bool triggered = false;
	
	void Start(){
		goal = GameObject.FindObjectOfType (typeof(Game)) as Game;
	}

	void OnTriggerEnter2D(Collider2D other){
		this.Trigger (other.gameObject);
	}
	
	void OnTriggerStay2D(Collider2D other) {
		this.Trigger (other.gameObject);
	}

	void OnTriggerExit2D(Collider2D other){
		triggered = false;
	}

//	void OnTriggerStay2D(Collider2D other) {
//		if (other.name.Contains ("Wall")) {
//			RaycastHit2D hit = Physics2D.Raycast (Vector2.zero, other.transform.position, Mathf.Infinity, -1, 0.09f, 0.11f);
//			
//			float spherePosition = other.transform.localScale.y + this.transform.localScale.y;
//
//			this.transform.Translate(hit.normal * spherePosition);
//		}
//	}

	private void Trigger(GameObject other){
		if (!triggered) {
			if (other.name.Contains ("Goal") || other.name == "Catch_Trigger") {
				this.Catch (other.gameObject);
			} else if (other.name.Contains ("Wall")) {
				this.bounce (other.gameObject);
			} else if (other.name == "Block_Trigger") {
				this.block (other.gameObject);
			}
		}
		
		triggered = true;
	}

	private void SetTurn(string name) {
		if (name == "Goal_Red" || name == "Player_01") {
			goal.setTurn (true);
		} else {
			goal.setTurn (false);
		}
	}

	private void Catch(GameObject other) {
		this.SetTurn (other.name);
		
		Object.Destroy (this.gameObject);
	}

	private void bounce(GameObject other) {
		float distance = this.transform.right.magnitude;
		Vector2 forwardL = this.transform.right / distance;
		Vector2 forwardG = this.transform.TransformDirection(forwardL);

		RaycastHit2D hit = Physics2D.Raycast (Vector2.zero, other.transform.position, Mathf.Infinity, -1, 0.09f, 0.11f);
		Vector2 exitDirection =  Vector2.Reflect(forwardL, hit.normal);

			Debug.DrawRay (Vector2.zero, other.transform.position, Color.blue, 0.1f);
			Debug.DrawRay (hit.point, hit.normal, Color.green, 0.1f);
			Debug.DrawRay (hit.point, exitDirection, Color.red, 0.1f);
		
		float angle = Mathf.Atan2(exitDirection.y, exitDirection.x) * Mathf.Rad2Deg;

		this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);		
	}

	private void block(GameObject other) {
		if (other.name.Contains ("Goal") || other.name == "Catch_Trigger") {
			this.SetTurn (other.name);
		} else {
			this.SetTurn (other.transform.parent.name);
		}
		
		Vector2 playerDirection = other.transform.parent.position - this.transform.position;
		
		RaycastHit2D hit = Physics2D.Raycast(this.transform.position, playerDirection);
		Vector2 exitDirection = Vector2.Reflect(playerDirection, hit.normal);
		
//			Debug.DrawRay (this.transform.position, playerDirection, Color.blue, 1000);
//			Debug.DrawRay (hit.point, hit.normal, Color.green, 1000);
//			Debug.DrawRay (hit.point, exitDirection, Color.red, 1000);
		
		float angle = Mathf.Atan2(exitDirection.y, exitDirection.x) * Mathf.Rad2Deg;

		this.gameObject.SetActive (false);
		
		other.GetComponentInParent<Player>().Shoot(other.GetComponentInParent<Player>().balls[0], this.transform.position, Quaternion.AngleAxis(angle, Vector3.forward));

		Object.Destroy(this.gameObject);
	}
}
