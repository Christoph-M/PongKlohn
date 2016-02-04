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
	public List<GameObject> projectiles;
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

	private int p1char = -1;
	private int p2char = -1;
	private int crystal = -1;
	private bool stopMovement = false;
	private bool specialBall = false;

//___________________________________________\\\\\\___MonoMethods___//////______________________________________________
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

		p1char = masterScript.GetCharacter (1) - 1;
		p2char = masterScript.GetCharacter (2);

		this.ResetPath ();
	}

	void OnEnable(){
		if (this.tag == "BallP1") {
			projectiles[p1char].SetActive(true);

			this.SetRotation (1.0f);
		} else {
			projectiles[p2char].SetActive(true);

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


//___________________________________________\\\\\\___Public___//////______________________________________________
	public List<Vector2> GetPath() {
		return path;
	}


//___________________________________________\\\\\\___Private___//////______________________________________________
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


//_________________\\\\\\___Block___//////_________________
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

			this.transform.rotation = ToolBox.GetRotationFromVector (exitDirection);
			//			float angle = Mathf.Atan2 (exitDirection.y, exitDirection.x) * Mathf.Rad2Deg;
			//
			//			this.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);


			StartCoroutine (CalcPath (blockFreezeTime));
			this.DeactivateProjectiles ();

			this.SetTurn (playerTag);
			if (this.tag == "BallP1") {
				projectiles[p1char].SetActive(true);
			} else {
				projectiles[p2char].SetActive(true);
			}

			string name = other.transform.parent.name;

			if (name.Contains("Player")) {
				this.SpeedUpProjectile (other.GetComponentInParent<Player> ().SetOnBlock ());
			}


			gameScript.ShakeScreen (0, (playerTag == "Player1") ? 1 : 2);
		}
	}


//_________________\\\\\\___Bounce___//////_________________
	private void Bounce(GameObject other, bool resetSpeed = false) {
//		Debug.Log ("Bounced. Time: " + timeElapsed);
//		timeElapsed = 0.0f;

		this.CheckScored ();


		float distance = this.transform.right.magnitude;
		Vector2 forward = this.transform.right / distance;

		RaycastHit2D hit = Physics2D.Raycast (Vector2.zero, other.transform.position, Mathf.Infinity, -1, 0.09f, 0.11f);
		Vector2 exitDirection =  Vector2.Reflect(forward, hit.normal);

		this.transform.rotation = ToolBox.GetRotationFromVector (exitDirection);


//		StartCoroutine (CalcPath (0.5f));
		if (!resetSpeed) this.SpeedUpProjectile(0.0f);

		gameScript.ShakeScreen (2);
	}


//_____________________________\\\\\\___Special___//////_____________________________
	private void Special(GameObject other) {
		if (timeElapsed >= specialFreezeTime + 0.1f) {
//			Debug.Log ("Special. Time: " + timeElapsed);
			timeElapsed = 0.0f;


			specialBall = true;

			string playerTag = other.transform.parent.tag;
			float posY = other.transform.position.y;

			crystal = masterScript.GetCrystal ((playerTag == "Player1") ? 1 : 2);


			if (crystal == 1) {
				if ((playerTag == "Player1" && posY < 0.0f) || (playerTag == "Player2" && posY > 0.0f)) {
					linearRotationScript.SetDirection (-1);
				} else if ((playerTag == "Player1" && posY > 0.0f) || (playerTag == "Player2" && posY < 0.0f)) {
					linearRotationScript.SetDirection (1);
				}
			}
				

			this.SetFieldMiddleRotation (0.0f, (posY >= 0) ? wallTop : wallBottom);


			StartCoroutine (CalcPath (specialFreezeTime));
			this.DeactivateProjectiles ();

			this.SetTurn (playerTag);
			projectiles[crystal + 5].SetActive (true);

			this.SpeedUpProjectile (2.0f, true);


			gameScript.ShakeScreen (3);
		}
	}

//_________________\\\\\\___BounceSpecial___//////_________________
	private void BounceSpecial(GameObject other) {
		if (other.tag == "WallRight" || other.tag == "WallLeft") {
			crystal = -1;
		} else {
			gameScript.ShakeScreen (3);
		}


		this.CheckScored ();


		switch (crystal) {
			case 1:
				this.EnableLinearRotation (true);


				float distance = this.transform.right.magnitude;
				Vector2 forward = this.transform.right / distance;

				RaycastHit2D hit = Physics2D.Raycast (Vector2.zero, other.transform.position, Mathf.Infinity, -1, 0.09f, 0.11f);

				this.transform.rotation = ToolBox.GetRotationFromVector (hit.normal);
				break;
			case 2:
				float posY = other.transform.position.y;

				this.SetFieldMiddleRotation ((this.tag == "BallP1") ? wallRight : wallLeft, (posY >= 0) ? goalTop : goalBottom);
				break;
			case 3:
				this.transform.position = new Vector3 (this.transform.position.x, -this.transform.position.y, this.transform.position.z);
				crystal = -1;
				break;
			default:
				this.DisableAllSpecials ();
				this.Bounce (other, true);
				break;
		}
	}


//_________________\\\\\\___Catch___//////_________________
	private void Catch(GameObject other) {
//		Debug.Log ("Stunned. Time: " + timeElapsed);
//		timeElapsed = 0.0f;

		gameScript.DecreaseBallSpeed();
		moveScript.UpdateBallSpeed ();
	}


//_________________\\\\\\___Goal___//////_________________
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

		gameScript.ShakeScreen (1);

		this.enabled = true;
	}


//___________________________________________\\\\\\___HelperMethods___//////______________________________________________
	private void SetTurn(string name) {
		if (name == "Goal_Red" || name == "Player1") {
			this.tag = "BallP1";
		} else {
			this.tag = "BallP2";
		}
	}

	private void SetRotation(float i) {
		Vector2 direction = new Vector2 (Random.Range (-1.0f, 1.0f), i);
		direction.Normalize ();
//		float fact = 1.0f / Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2));
//		direction = direction * fact;

		this.transform.rotation = Quaternion.LookRotation(direction, Vector3.forward);
	}

	private void SetFieldMiddleRotation(float x, float y) {
		Vector3 dir = new Vector3 (x, y, -6.0f) - this.transform.position;

		this.transform.rotation = ToolBox.GetRotationFromVector (dir);
	}

	private void SpeedUpProjectile(float fac, bool special = false) {
		gameScript.BallSpeedUp (fac, special);
		moveScript.UpdateBallSpeed ();
	}

	private void CheckScored() {
		if (this.tag == "BallP1" && this.transform.position.x > 0.0f) {
			gameScript.Player1Scored (true);
		} else if (this.tag == "BallP2" && this.transform.position.x < 0.0f) {
			gameScript.Player2Scored (true);
		}
	}

	private void DeactivateProjectiles() {
		foreach (GameObject ball in projectiles) {
			ball.SetActive (false);
		}
	}

	private void ResetPath() {
		path = new List<Vector2>();
	}

	private void ResetBall(string name) {
		this.DeactivateProjectiles ();

		this.DisableAllSpecials ();

		this.transform.position = homePosition;

		this.SetTurn (name);

		gameScript.ResetBallSpeed();
		moveScript.UpdateBallSpeed ();

		this.ResetPath ();
		gameScript.SetProjectileTransform (null);
	}

	private void DisableAllSpecials() {
		specialBall = false;
		this.EnableLinearRotation (false);
	}

	private void EnableLinearRotation(bool b) {
		linearRotation = b;
		linearRotationScript.enabled = b;
	}
}
