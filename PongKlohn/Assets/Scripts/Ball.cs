using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ball : MonoBehaviour {
	public bool move = true;
	public bool sinCosMove = false;
	public bool linearRotation = false;
	public bool sinCosRotation = false;


	private Game gameScript;
	private Move moveScript;
	private SinCosMovement sinCosMovementScript;
	private LinearRotation linearRotationScript;
	private SinCosRotation sinCosRotationScript;

	private Transform myTransform;

	private bool triggered = false;
	
	void Start(){
		gameScript = GameObject.FindObjectOfType (typeof(Game)) as Game;
		moveScript = GameObject.FindObjectOfType (typeof(Move)) as Move;
		sinCosMovementScript = GameObject.FindObjectOfType (typeof(SinCosMovement)) as SinCosMovement;
		linearRotationScript = GameObject.FindObjectOfType (typeof(LinearRotation)) as LinearRotation;
		sinCosRotationScript = GameObject.FindObjectOfType (typeof(SinCosRotation)) as SinCosRotation;

		this.name = "Projectile";

		gameScript.SetProjectileTransform (this.transform);
	}

	void FixedUpdate() {
		if (move) moveScript.Update_ ();
		if (sinCosMove) sinCosMovementScript.Update_ ();
		if (linearRotation) linearRotationScript.Update_ ();
		if (sinCosRotation) sinCosRotationScript.Update_ ();
	}

	float timeElapsed = 0.0f;
	void LateUpdate() {
		timeElapsed += Time.deltaTime;
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

	private void Trigger(GameObject other){
		if (!triggered) {
			if (other.name.Contains ("Goal")) {
				this.Goal (other.gameObject);
			} else if (other.name == "Catch_Trigger") {
				this.Catch (other.gameObject);
			} else if (other.name.Contains ("Wall")) {
				this.Bounce (other.gameObject);
			} else if (other.name == "Miss_Trigger") {
				other.GetComponentInParent<Player>().SetZuLangsamZumFangenDuMong(true);
			} else if (other.name == "Block_Trigger") {
				this.Block (other.gameObject);
			} else if (other.name == "Dash_Trigger") {
				other.GetComponentInParent<Player>().SetDashTrigger(true);
			}
		}
		
		triggered = true;
	}

	private void SetTurn(string name) {
		if (name == "Goal_Red" || name == "Player_01") {
			gameScript.SetTurn (false);
		} else {
			gameScript.SetTurn (true);
		}
	}

	private void Goal(GameObject other) {
		Debug.Log ("Goal. Time: " + timeElapsed);
		timeElapsed = 0.0f;

		if (other.name == "Goal_Red") {
			gameScript.Player2Scored (false);
		} else {
			gameScript.Player1Scored (false);
		}

		this.SetTurn (other.name);
		
		this.DestroyBall ();
		gameScript.ResetBallSpeed();
	}

	private void Catch(GameObject other) {
		Debug.Log ("Catched. Time: " + timeElapsed);
		timeElapsed = 0.0f;

		this.SetTurn (other.transform.parent.name);
		
		this.DestroyBall ();
		gameScript.ResetBallSpeed();
	}

	private void Bounce(GameObject other) {
		Debug.Log ("Bounced. Time: " + timeElapsed);
		timeElapsed = 0.0f;

		if (this.tag == "RedBall") {
			gameScript.Player1Scored (true);
		} else {
			gameScript.Player2Scored (true);
		}

		float distance = this.transform.right.magnitude;
		Vector2 forward = this.transform.right / distance;

		RaycastHit2D hit = Physics2D.Raycast (Vector2.zero, other.transform.position, Mathf.Infinity, -1, 0.09f, 0.11f);
		Vector2 exitDirection =  Vector2.Reflect(forward, hit.normal);

//			Debug.DrawRay (Vector2.zero, other.transform.position, Color.blue, 0.1f);
//			Debug.DrawRay (hit.point, hit.normal, Color.green, 0.1f);
//			Debug.DrawRay (hit.point, exitDirection, Color.red, 0.1f);
		
		float angle = Mathf.Atan2(exitDirection.y, exitDirection.x) * Mathf.Rad2Deg;

		this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);		
	}

	private void Block(GameObject other) {
		Debug.Log ("Blocked. Time: " + timeElapsed);
		timeElapsed = 0.0f;

		Vector2 playerDirection = other.transform.parent.position - this.transform.position;
		
		RaycastHit2D hit = Physics2D.Raycast(this.transform.position, playerDirection);
		Vector2 exitDirection = Vector2.Reflect(playerDirection, hit.normal);
		
//			Debug.DrawRay (this.transform.position, playerDirection, Color.blue, 1000);
//			Debug.DrawRay (hit.point, hit.normal, Color.green, 1000);
//			Debug.DrawRay (hit.point, exitDirection, Color.red, 1000);
		
		float angle = Mathf.Atan2(exitDirection.y, exitDirection.x) * Mathf.Rad2Deg;

		this.gameObject.SetActive (false);
		
		other.GetComponentInParent<Player>().Instance(other.GetComponentInParent<Player>().balls[0], this.transform.position, Quaternion.AngleAxis(angle, Vector3.forward));

		this.DestroyBall ();
		gameScript.BallSpeedUp ();
	}

	private void DestroyBall() {
		Object.Destroy (this.gameObject);
		
		gameScript.SetProjectileTransform (null);
	}
}
