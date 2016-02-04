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
	public float blockFreezeTime = 0.1f;
	public float specialFreezeTime = 0.2f;

//__________________________Private_____________________________
	private MasterScript masterScript;
	private Game gameScript;
	private Move moveScript;
	private SinCosMovement sinCosMovementScript;
	private LinearRotation linearRotationScript;
	private SinCosRotation sinCosRotationScript;

	private Vector3 homePosition = new Vector3 (0.0f, 0.0f, -6.0f);

	private const float fieldHeight = 22.0f;
	private const float fieldWidth = 70.0f;
	private const float goalHeight = 11.0f;
	private float wallTop;
	private float wallBottom;
	private float wallLeft;
	private float wallRight;
	private float goalTop;
	private float goalBottom;

	private List<Vector2> path;

	private int crystal = -1;
	private bool stopMovement = false;
	private bool specialBall = false;

//__________________________MonoMethods_____________________________
	void Awake() {
		masterScript = GameObject.FindObjectOfType (typeof(MasterScript)) as MasterScript;
		gameScript = GameObject.FindObjectOfType (typeof(Game)) as Game;

		moveScript = GameObject.FindObjectOfType (typeof(Move)) as Move;
		sinCosMovementScript = GameObject.FindObjectOfType (typeof(SinCosMovement)) as SinCosMovement;
		linearRotationScript = GameObject.FindObjectOfType (typeof(LinearRotation)) as LinearRotation;
		sinCosRotationScript = GameObject.FindObjectOfType (typeof(SinCosRotation)) as SinCosRotation;


		this.name = "Projectile";

		wallTop    =  fieldHeight / 2;
		wallBottom = -fieldHeight / 2;
		wallRight  =  fieldWidth  / 2;
		wallLeft   = -fieldWidth  / 2;
		goalTop    =  goalHeight  / 2;
		goalBottom = -goalHeight  / 2;

		this.ResetPath ();
	}

	void OnEnable(){
		if (this.tag == "BallP1") {
			this.transform.FindChild ("Elektro R R").gameObject.SetActive(true);

			this.SetRotation (1.0f);
		} else {
			this.transform.FindChild ("Elektro B R").gameObject.SetActive(true);

			this.SetRotation (-1.0f);
		}

		StartCoroutine (CalcPath (2.0f));
	}

	private IEnumerator CalcPath(float t) {
		stopMovement = true;

		this.ResetPath ();

		RaycastHit2D hit;
		Vector2 startPoint = this.transform.position;
		Vector2 startDirection = this.transform.right;

		yield return new WaitForSeconds (t);

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

		stopMovement = false;
	}

	void FixedUpdate() {
		if (!stopMovement) {
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
	}

	private float timeElapsed = 0.0f;
	void LateUpdate() {
		timeElapsed += Time.deltaTime;
	}

	void OnTriggerEnter2D(Collider2D other){
		this.Trigger (other.gameObject);
	}

//__________________________Public_____________________________
	public List<Vector2> GetPath() {
		return path;
	}

//__________________________Private_____________________________
	private void Trigger(GameObject other){
		if (other.tag == "BlockTrigger") {
			this.Block (other);
		} else if (other.tag == "SpecialTrigger") {
			this.Special (other);
		} else if (other.tag == "MissTrigger") {
			other.GetComponentInParent<Player>().SetZuLangsamZumFangenDuMong(true);
		} else if (other.tag == "CatchTrigger") {
			this.Catch (other);
		} else if (other.tag == "DashTrigger") {
			other.GetComponentInParent<Player>().SetDashTrigger(true);
		} else if (other.tag.Contains("Wall")) {
			if (specialBall) {
				this.BounceSpecial (other);
			} else {
				this.Bounce (other);
			}
		} else if (other.tag == "Goal") {
			this.Goal (other);
		}
	}

	private void Bounce(GameObject other) {
//		Debug.Log ("Bounced. Time: " + timeElapsed);
//		timeElapsed = 0.0f;


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

//		Vector3 diff = (Vector2)this.transform.position - hit.point;
//		this.transform.position = (Mathf.Sqrt(diff.sqrMagnitude) * exitDirection) + hit.point;

		this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

//		if (other.tag == "WallTop") {
//			this.transform.position = new Vector3 (this.transform.position.x, wallTop - 0.5f, this.transform.position.z);
//		} else if (other.tag == "WallBottom") {
//			this.transform.position = new Vector3 (this.transform.position.x, wallBottom + 0.5f, this.transform.position.z);
//		} else if (other.tag == "WallRight") {
//			this.transform.position = new Vector3 (wallRight - 0.5f, this.transform.position.y, this.transform.position.z);
//		} else if (other.tag == "WallLeft") {
//			this.transform.position = new Vector3 (wallLeft + 0.5f, this.transform.position.y, this.transform.position.z);
//		}


//		StartCoroutine (CalcPath (0.5f));

		gameScript.ShakeScreen (2);
	}

	private void BounceSpecial(GameObject other) {
//		Debug.Log ("Bounce Special. Time: " + timeElapsed);
//		timeElapsed = 0.0f;


		if (other.tag == "WallRight" || other.tag == "WallLeft") {
			crystal = -1;
		}

		switch (crystal) {
		case 1:
			linearRotation = true;
			linearRotationScript.enabled = true;

			float distance = this.transform.right.magnitude;
			Vector2 forward = this.transform.right / distance;

			RaycastHit2D hit = Physics2D.Raycast (Vector2.zero, other.transform.position, Mathf.Infinity, -1, 0.09f, 0.11f);
			Vector2 exitDirection = Vector2.Reflect (forward, hit.normal);

//				Debug.DrawRay (Vector2.zero, other.transform.position, Color.blue, 0.1f);
//				Debug.DrawRay (hit.point, hit.normal, Color.green, 0.1f);
//				Debug.DrawRay (hit.point, exitDirection, Color.red, 0.1f);

			float angle1 = Mathf.Atan2 (hit.normal.y, hit.normal.x) * Mathf.Rad2Deg;

			this.transform.rotation = Quaternion.AngleAxis (angle1, Vector3.forward);

			gameScript.BallSpeedUp (4.0f);
			moveScript.UpdateBallSpeed ();


			gameScript.ShakeScreen (3);
			break;
		case 2:
			Vector3 dir;

			float x;

			if (this.tag == "BallP1") {
				x = wallRight;
			} else {
				x = wallLeft;
			}

			if (other.transform.position.y >= 0) {
				dir = new Vector3 (x, goalTop, -6.0f) - this.transform.position;
			} else {
				dir = new Vector3 (x, goalBottom, -6.0f) - this.transform.position;
			}

			float angle2 = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;

			this.transform.rotation = Quaternion.AngleAxis (angle2, Vector3.forward);

			gameScript.BallSpeedUp (4.0f);
			moveScript.UpdateBallSpeed ();


			gameScript.ShakeScreen (3);
			break;
		case 3:
			this.transform.position = new Vector3 (this.transform.position.x, -this.transform.position.y, this.transform.position.z);
			crystal = -1;

			gameScript.BallSpeedUp (4.0f);
			moveScript.UpdateBallSpeed ();


			gameScript.ShakeScreen (3);
			break;
		default:
			this.DisableAllSpecials ();
			this.Bounce (other);
			break;
		}
	}

	private void Block(GameObject other) {
		if (timeElapsed >= blockFreezeTime + 0.1f) {
//			Debug.Log ("Blocked. Time: " + timeElapsed);
			timeElapsed = 0.0f;


			this.DisableAllSpecials ();

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

			this.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);


			StartCoroutine (CalcPath (blockFreezeTime));
			this.DeactivateProjectiles ();

			this.SetTurn (playerTag);
			if (this.tag == "BallP1") {
				this.transform.FindChild ("Elektro R R").gameObject.SetActive(true);
			} else {
				this.transform.FindChild ("Elektro B R").gameObject.SetActive(true);
			}

			if (other.transform.parent.name.Contains("Player")) {
				gameScript.BallSpeedUp (other.GetComponentInParent<Player>().SetOnBlock());
				moveScript.UpdateBallSpeed ();
			}


			gameScript.ShakeScreen (0, (playerTag == "Player1") ? 1 : 2);
		}
	}

	private void Special(GameObject other) {
		if (timeElapsed >= specialFreezeTime + 0.1f) {
//			Debug.Log ("Special. Time: " + timeElapsed);
			timeElapsed = 0.0f;


			specialBall = true;


			Vector3 dir;

			if (other.transform.position.y >= 0) {
				dir = new Vector3 (0.0f, wallTop, -6.0f) - this.transform.position;
			} else {
				dir = new Vector3 (0.0f, wallBottom, -6.0f) - this.transform.position;
			}

			float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;

			this.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);


			string playerTag = other.transform.parent.tag;
			crystal = masterScript.GetCrystal ((playerTag == "Player1") ? 1 : 2);

			if (crystal == 1) {
				string tag = other.transform.parent.tag;
				float pos = this.transform.position.y;

				if ((tag == "Player1" && pos < 0.0f) || (tag == "Player2" && pos > 0.0f)) {
					linearRotationScript.SetDirection(-1);
				} else if ((tag == "Player1" && pos > 0.0f) || (tag == "Player2" && pos < 0.0f)) {
					linearRotationScript.SetDirection(1);
				}
			}


			StartCoroutine (CalcPath (specialFreezeTime));
			this.DeactivateProjectiles ();

			this.SetTurn (playerTag);
			this.transform.FindChild ("Special_" + crystal).gameObject.SetActive (true);

			gameScript.BallSpeedUp (2.0f);
			moveScript.UpdateBallSpeed ();


			gameScript.ShakeScreen (3);
		}
	}

	private void Catch(GameObject other) {
//		Debug.Log ("Stunned. Time: " + timeElapsed);
//		timeElapsed = 0.0f;

		gameScript.DecreaseBallSpeed();
		moveScript.UpdateBallSpeed ();
	}

	private void Goal(GameObject other) {
//		Debug.Log ("Goal. Time: " + timeElapsed);
//		timeElapsed = 0.0f;

		if (other.name == "Goal_Red") {
			gameScript.Player2Scored (false);
		} else {
			gameScript.Player1Scored (false);
		}


		this.enabled = false;

		this.ResetBall (other.name);
		gameScript.ResetBallSpeed();
		moveScript.UpdateBallSpeed ();

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

		Debug.Log ("SetTurn(): " + this.tag);
	}

	private void SetRotation(float i) {
		Vector2 direction = new Vector2 (Random.Range (-1.0f, 1.0f), i);
		direction.Normalize ();
//		float fact = 1.0f / Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2));
//		direction = direction * fact;

		this.transform.rotation = Quaternion.LookRotation(direction, Vector3.forward);
	}

	private void DeactivateProjectiles() {
		foreach (Transform child in this.transform) {
			child.gameObject.SetActive (false);
		}

		Debug.Log ("DeactivateProjectiles()");
	}

	private void ResetPath() {
		path = new List<Vector2>();
	}

	private  void DestroyBall() {
		Object.Destroy (this.gameObject);

		gameScript.SetProjectileTransform (null);
	}

	private void ResetBall(string name) {
		this.DeactivateProjectiles ();

		this.DisableAllSpecials ();

		this.transform.position = homePosition;

		this.SetTurn (name);

		this.ResetPath ();
		gameScript.SetProjectileTransform (null);
	}

	private void DisableAllSpecials() {
		specialBall = false;
		linearRotation = false;
		linearRotationScript.enabled = false;
	}
}
