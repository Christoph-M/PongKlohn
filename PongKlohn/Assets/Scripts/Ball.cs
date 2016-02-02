using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ball : MonoBehaviour {
	[Header("^^Put Move Script Above Ball Script^^")]
	[Space(10)]
//__________________________Public_____________________________
	public bool move = true;
	public bool sinCosMove = false;
	public bool linearRotation = false;
	public bool sinCosRotation = false;

	[Space(10)]
	public int maxPredictionCount = 15;

//__________________________Protected_____________________________
	protected Game gameScript;
	protected Move moveScript;
	protected SinCosMovement sinCosMovementScript;
	protected LinearRotation linearRotationScript;
	protected SinCosRotation sinCosRotationScript;

	protected const float fieldHeight = 22.0f;
	protected const float fieldWidth = 70.0f;
	protected float wallTop;
	protected float wallBottom;
	protected float wallLeft;
	protected float wallRight;

	protected List<Vector2> path;

//__________________________MonoMethods_____________________________
	void Awake() {
		gameScript = GameObject.FindObjectOfType (typeof(Game)) as Game;

		moveScript = GameObject.FindObjectOfType (typeof(Move)) as Move;
		sinCosMovementScript = GameObject.FindObjectOfType (typeof(SinCosMovement)) as SinCosMovement;
		linearRotationScript = GameObject.FindObjectOfType (typeof(LinearRotation)) as LinearRotation;
		sinCosRotationScript = GameObject.FindObjectOfType (typeof(SinCosRotation)) as SinCosRotation;

		this.name = "Projectile";

		wallTop = fieldHeight / 2;
		wallBottom = -fieldHeight / 2;
		wallRight = fieldWidth / 2;
		wallLeft = -fieldWidth / 2;
	}

	void OnEnable(){
		StartCoroutine (CalcPath ());

		if (this.tag == "BallP1") {
			this.transform.FindChild ("Elektro R R").gameObject.SetActive(true);
		} else {
			this.transform.FindChild ("Elektro B R").gameObject.SetActive(true);
		}
	}

	protected IEnumerator CalcPath() {
		path = new List<Vector2>();

		RaycastHit2D hit;
		Vector2 startPoint = this.transform.position;
		Vector2 startDirection = this.transform.right;

		path.Add (startPoint);

		do {
			hit = Physics2D.Raycast (startPoint, startDirection, Mathf.Infinity, -1, 0.09f, 0.11f);

			string collTag = hit.collider.tag;
			Vector2 hitPoint = Vector2.zero;

			if (collTag.Contains("Wall")) {
				if (collTag == "WallTop") {
					hitPoint = new Vector2(hit.point.x, hit.point.y - 0.5f);
				} else if (collTag == "WallBottom") {
					hitPoint = new Vector2(hit.point.x, hit.point.y + 0.5f);
				} else if (collTag == "WallRight") {
					hitPoint = new Vector2(hit.point.x - 0.5f, hit.point.y);
				} else if (collTag == "WallLeft") {
					hitPoint = new Vector2(hit.point.x + 0.5f, hit.point.y);
				}
			} else {
				hitPoint = hit.point;
			}

			Vector2 exitDirection = Vector2.Reflect (startDirection, hit.normal);

			Debug.DrawLine (new Vector3(startPoint.x, startPoint.y, -6.0f), new Vector3(hitPoint.x, hitPoint.y, -6.0f), Color.red, 3.0f);

			startPoint = hitPoint;
			startDirection = exitDirection;

			path.Add (hitPoint);
		} while (hit.collider.gameObject.tag.Contains ("Wall") && path.Count <= maxPredictionCount);

		gameScript.SetProjectileTransform (this.transform);

		yield return 0;
	}

	protected void FixedUpdate() {
		if (move) moveScript.Update_ ();
		if (sinCosMove) sinCosMovementScript.Update_ ();
		if (linearRotation) linearRotationScript.Update_ ();
		if (sinCosRotation) sinCosRotationScript.Update_ ();

		if (this.transform.position.x > wallRight) {
			this.transform.position = new Vector3(wallRight, this.transform.position.y, this.transform.position.z);
		} else if (this.transform.position.x < wallLeft) {
			this.transform.position = new Vector3(wallLeft, this.transform.position.y, this.transform.position.z);
		}

		if (this.transform.position.y > wallTop) {
			this.transform.position = new Vector3(this.transform.position.x, wallTop, this.transform.position.z);
		} else if (this.transform.position.y < wallBottom) {
			this.transform.position = new Vector3(this.transform.position.x, wallBottom, this.transform.position.z);
		}
	}

	protected float timeElapsed = 0.0f;
	protected void LateUpdate() {
		timeElapsed += Time.deltaTime;
	}

	protected void OnTriggerEnter2D(Collider2D other){
		this.Trigger (other.gameObject);
	}

//__________________________Public_____________________________
	public List<Vector2> GetPath() {
		return path;
	}

//__________________________Private_____________________________
	private void Trigger(GameObject other){
		if (other.tag.Contains("Wall")) {
			this.Bounce (other.gameObject);
		} else if (other.tag == "BlockTrigger") {
			this.Block (other.gameObject);
		} else if (other.tag == "MissTrigger") {
			other.GetComponentInParent<Player>().SetZuLangsamZumFangenDuMong(true);
		} else if (other.tag == "CatchTrigger") {
			this.Catch (other.gameObject);
		} else if (other.tag == "DashTrigger") {
			other.GetComponentInParent<Player>().SetDashTrigger(true);
		} else if (other.tag == "Goal") {
			this.Goal (other.gameObject);
		}
	}

	private void Bounce(GameObject other) {
		Debug.Log ("Bounced. Time: " + timeElapsed);
		timeElapsed = 0.0f;

		if (this.tag == "BallP1" && this.transform.position.x > 0.0f) {
			gameScript.Player1Scored (true);
		} else if (this.tag == "BallP2" && this.transform.position.x < 0.0f) {
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

		gameScript.ShakeScreen (2);
	}

	private void Block(GameObject other) {
		if (timeElapsed >= 0.1f) {
			Debug.Log ("Blocked. Time: " + timeElapsed);

			timeElapsed = 0.0f;

			string playerTag = other.transform.parent.tag;

			if (playerTag == "Player1") {
				gameScript.Player1AddEnergy ();
			} else {
				gameScript.Player2AddEnergy ();
			}

			Vector2 playerDirection = other.transform.parent.position - this.transform.position;
		
			RaycastHit2D hit = Physics2D.Raycast (this.transform.position, playerDirection);
			Vector2 exitDirection = Vector2.Reflect (playerDirection, hit.normal);
		
//				Debug.DrawRay (this.transform.position, playerDirection, Color.blue, 1000);
//				Debug.DrawRay (hit.point, hit.normal, Color.green, 1000);
//				Debug.DrawRay (hit.point, exitDirection, Color.red, 1000);
		
			float angle = Mathf.Atan2 (exitDirection.y, exitDirection.x) * Mathf.Rad2Deg;

			this.enabled = false;

			Player playerScript = other.GetComponentInParent<Player> ();
			//playerScript.Instance (playerScript.balls [0], this.transform.position, Quaternion.AngleAxis (angle, Vector3.forward));

			this.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);

			foreach (Transform child in this.transform) {
				child.gameObject.SetActive (false);
			}

			this.SetTurn (other.transform.parent.tag);

			gameScript.BallSpeedUp ();

			gameScript.ShakeScreen (0, (playerTag == "Player1") ? 1 : 2);

			this.enabled = true;
		}
	}

	private void Catch(GameObject other) {
		Debug.Log ("Catched. Time: " + timeElapsed);
		timeElapsed = 0.0f;

		this.enabled = false;

		this.SetTurn (other.transform.parent.tag);

		this.ResetBall ();
		gameScript.DecreaseBallSpeed();

		this.enabled = true;
	}

	private void Goal(GameObject other) {
		Debug.Log ("Goal. Time: " + timeElapsed);
		timeElapsed = 0.0f;

		if (other.name == "Goal_Red") {
			gameScript.Player2Scored (false);
		} else {
			gameScript.Player1Scored (false);
		}

		this.enabled = false;

		this.SetTurn (other.name);

		this.ResetBall ();
		gameScript.ResetBallSpeed();

		gameScript.ShakeScreen (1);

		this.enabled = true;
	}

//__________________________HelperMethods_____________________________
	private void SetTurn(string name) {
		if (name == "Goal_Red" || name == "Player1") {
			this.tag = "BallP1";
		} else {
			this.tag = "BallP2";
		}
	}

	protected  void DestroyBall() {
		Object.Destroy (this.gameObject);

		gameScript.SetProjectileTransform (null);
	}

	private void ResetBall() {
		foreach (Transform child in this.transform) {
			child.gameObject.SetActive (false);
		}

		this.transform.position = Vector3.zero;

		gameScript.SetProjectileTransform (null);
	}
}
