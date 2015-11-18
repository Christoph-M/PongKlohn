using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
	private Game goal;

	private Transform myTransform;
	
	void Start(){
		goal = GameObject.FindObjectOfType (typeof(Game)) as Game;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.name.Contains("Goal") || other.name == "Catch_Trigger") {
			this.Catch(other.gameObject);
		} else if (other.name.Contains ("Wall")) {
			this.bounce();
		} else if (other.name == "Block_Trigger") {
			this.block(other.gameObject);
		}
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

	private void bounce() {
		float distance = this.transform.right.magnitude;
		Vector2 forwardL = this.transform.right / distance;
		Vector2 forwardG = this.transform.TransformDirection(forwardL);
		
		RaycastHit2D hit = Physics2D.Raycast (this.transform.position, forwardG);
		Vector2 exitDirection = Vector2.Reflect(forwardL, hit.normal);
		
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
		
		RaycastHit2D hit = Physics2D.Raycast(other.transform.position, this.transform.TransformDirection(playerDirection));
		Vector2 exitDirection = Vector2.Reflect(playerDirection, hit.normal);
		
		float angle = Mathf.Atan2(exitDirection.y, exitDirection.x) * Mathf.Rad2Deg;

		this.gameObject.SetActive (false);
		
		other.GetComponentInParent<Player>().Shoot(other.GetComponentInParent<Player>().balls[0], this.transform.position, Quaternion.AngleAxis(angle, Vector3.forward));

		Object.Destroy(this.gameObject);
	}
}
