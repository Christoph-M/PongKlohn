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

	[Header("Particle Objects")]
	public List<GameObject> particleObjs;

	[Header("Path Prediction")]
	public int maxPredictionCount = 15;

	[Header("Freeze Times")]
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
	private const float goalHeight = 10.0f;
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
	private int bounceCount = -1;
	private bool stopMovement = false;
	private bool specialBall = false;

//____________________________________________________________\\\\\\___MonoMethods___//////_______________________________________________________________


//_________________\\\\\\___Awake___//////_________________
// Calculates wall positions and goal posts + sets
// character + resets path
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
	void Awake() {
		masterScript = GameObject.FindObjectOfType (typeof(MasterScript)) as MasterScript;
		gameScript = GameObject.FindObjectOfType (typeof(Game)) as Game;

		moveScript = GameObject.FindObjectOfType (typeof(Move)) as Move;
		sinCosMovementScript = GameObject.FindObjectOfType (typeof(SinCosMovement)) as SinCosMovement;
		linearRotationScript = GameObject.FindObjectOfType (typeof(LinearRotation)) as LinearRotation;
		sinCosRotationScript = GameObject.FindObjectOfType (typeof(SinCosRotation)) as SinCosRotation;


		this.name = "Projectile";

		bounceCount = 0;

		wallTop    =  fieldHeight / 2;
		wallBottom = -fieldHeight / 2;
		wallRight  =  fieldWidth  / 2;
		wallLeft   = -fieldWidth  / 2;
		goalTop    =  goalHeight  / 2;
		goalBottom = -goalHeight  / 2;

		p1char = masterScript.GetCharacter (1) - 1;
		p2char = masterScript.GetCharacter (2) - 1;

		this.ResetPath ();
	}


//_________________\\\\\\___OnEnable___//////_________________
// Activates respective particle object + sets initial
// direction + calculates projectile path
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
	void OnEnable(){
		if (this.tag == "BallP1") {
			particleObjs[p1char].SetActive(true);

			this.SetRotation (1.0f);
		} else {
			particleObjs[p2char].SetActive(true);

			this.SetRotation (-1.0f);
		}

		gameScript.ResetBallSpeed ();

		StartCoroutine (CalcPath (2.0f));
	}


//_________________\\\\\\___FixedUpdate___//////_________________
// Moves projectile + sets projectile back into the field if it
// goes beyond the bounds
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
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


//_________________\\\\\\___LateUpdate___//////_________________
// Keeps time to prevent multiple block activations in a short
// timeframe
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
	private float timeElapsed = 0.0f;
	void LateUpdate() {
		timeElapsed += Time.deltaTime;
	}


//_________________\\\\\\___OnTriggerEnter2D___//////_________________
// Called when projectile collides with a trigger
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
	void OnTriggerEnter2D(Collider2D other){
		this.Trigger (other.gameObject);
	}


//____________________________________________________________\\\\\\___Public___//////_______________________________________________________________
	

//_________________\\\\\\___GetPath___//////_________________
// Returns pre-calculated path of the projectile
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
	public List<Vector2> GetPath() {
		return path;
	}


//____________________________________________________________\\\\\\___Private___//////_______________________________________________________________


//_________________\\\\\\___Trigger___//////_________________
// Determines which trigger was hit and invokes the
// respective action
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
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
		} else if (other.tag == "Goal") {
			this.Goal (other);
		} else if (other.tag.Contains("Wall")) {
			if (specialBall) {
				this.BounceSpecial (other);
			} else {
				this.Bounce (other);
			}
		} 
	}


//_________________\\\\\\___Block___//////_________________
// Adds energy to respective player, bounces projectile
// off the shield, calculates new path, sets new 
// ball particle and speeds up projectile
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
	private void Block(GameObject other) {
		if (timeElapsed >= blockFreezeTime + 0.1f) {
//			Debug.Log ("Blocked. Time: " + timeElapsed);
			timeElapsed = 0.0f;


			this.DisableAllSpecials ();

			string playerTag = other.transform.parent.tag;

			if (other.name != "DashCollider") {
				if (playerTag == "Player1") {
					gameScript.Player1AddEnergy ();
				} else {
					gameScript.Player2AddEnergy ();
				}
			}


			Vector2 playerDirection = other.transform.parent.position - this.transform.position;

			RaycastHit2D hit = Physics2D.Raycast (this.transform.position, playerDirection);
			Vector2 exitDirection = Vector2.Reflect (playerDirection, hit.normal);

			this.transform.rotation = ToolBox.GetRotationFromVector (exitDirection);


			StartCoroutine (CalcPath (blockFreezeTime));
			this.DeactivateParticleObjs ();

			this.SetTag (playerTag);
			if (this.tag == "BallP1") {
				particleObjs[p1char].SetActive(true);
			} else {
				particleObjs[p2char].SetActive(true);
			}

			string name = other.transform.parent.name;

			if (name.Contains("Player")) {
				this.SpeedUpProjectile (other.GetComponentInParent<Player> ().SetOnBlock ());
			}


			gameScript.ShakeScreen (0, (playerTag == "Player1") ? 1 : 2);
		}
	}


//_________________\\\\\\___Bounce___//////_________________
// Takes enemy health if projectile is within the enemy side
// of the field, bounces it off the wall and resets the
// projectile speed to the last speed after a special
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
	private void Bounce(GameObject other) {
//		Debug.Log ("Bounced. Time: " + timeElapsed);
//		timeElapsed = 0.0f;

		this.CheckScored ();


		++bounceCount;

		this.transform.position = new Vector3 (path [bounceCount].x, path [bounceCount].y, this.transform.position.z);

		if (other.tag == "WallLeft") AI.SetNewTargetVectorCount(path[bounceCount]);


		Vector2 exitDirection = path [bounceCount + 1] - path [bounceCount];

		this.transform.rotation = ToolBox.GetRotationFromVector (exitDirection);


		if (other.tag == "WallRight" || other.tag == "WallLeft") {
			this.DeactivateParticleObjs ();
			if (this.tag == "BallP1") {
				particleObjs[p1char].SetActive(true);
			} else {
				particleObjs[p2char].SetActive(true);
			}
		}


		gameScript.ShakeScreen (2);
	}


//_____________________________\\\\\\___Special___//////_____________________________
// Activates special-mode, sets crystal, sets linear rotation direction
// if crystal is 1, sets target point to middleTop, middleBottm respectively,
// calculates new path, sets new ball particle, sets projectile speed to special
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
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
			this.DeactivateParticleObjs ();

			this.SetTag (playerTag);
			particleObjs[(this.tag == "BallP1") ? p1char : p2char + 6].SetActive (true);

			this.SpeedUpProjectile (2.0f, true);


			gameScript.ShakeScreen (3);
		}
	}


//_________________\\\\\\___BounceSpecial___//////_________________
// Sets crystal to -1 if projectile hit the right or left wall,
// takes enemy health if projectile is within enemy side of field,
// performs the respective special, disable special-mode if
// crystal is -1 and invokes a regular bounce if in special-mode
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
	private void BounceSpecial(GameObject other) {
		if (other.tag == "WallRight" || other.tag == "WallLeft" || crystal <= 0) {
			if (crystal == 1) ++bounceCount;

			this.DisableAllSpecials ();
			this.Bounce (other);
		} else {
			this.CheckScored ();

			switch (crystal) {
				case 1:
					this.EnableLinearRotation (true);
					
					
					float distance = this.transform.right.magnitude;
					Vector2 forward = this.transform.right / distance;
					
					RaycastHit2D hit = Physics2D.Raycast (Vector2.zero, other.transform.position, Mathf.Infinity, -1, 0.09f, 0.11f);
					
					this.transform.rotation = ToolBox.GetRotationFromVector (hit.normal); break;
				case 2:
					++bounceCount;
					float posY = path[bounceCount].y;
					
					this.transform.rotation = ToolBox.GetRotationFromVector(path [bounceCount + 1] - path [bounceCount]);
					
					this.DisableAllSpecials (); break;
				case 3:
					++bounceCount;
					this.transform.position = new Vector3 (path [bounceCount].x, path [bounceCount].y, this.transform.position.z);
					
					this.DisableAllSpecials (); break;
				default:
					break;
			}

			gameScript.ShakeScreen (3);
		}
	}


//_________________\\\\\\___Catch___//////_________________
// Decreases projectile speed
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
	private void Catch(GameObject other) {
//		Debug.Log ("Stunned. Time: " + timeElapsed);
//		timeElapsed = 0.0f;

		gameScript.DecreaseBallSpeed();
		moveScript.UpdateBallSpeed ();
	}


//_________________\\\\\\___Goal___//////_________________
// Takes health from respective player, disables
// projectile, resets it and enables it again
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
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


//_________________\\\\\\___CalcPath___//////_________________
// Stops movement + waits a given time + calculates projectile
// path + sets projectile transform in game script + enables
// projectile again
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
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

			if (path.Count == 1 && crystal > 0) {
				switch (crystal) {
					case 1:
						Vector2 dir1 = new Vector2 ((this.tag == "BallP1") ? wallRight : wallLeft, (hitPoint.y >= 0) ? goalTop + 2 : goalBottom - 2) - hitPoint;
						
						startPoint = hitPoint;
						startDirection = dir1;
						
						path.Add (hitPoint); break;
					case 2:
						Vector2 dir2 = new Vector2 ((this.tag == "BallP1") ? wallRight : wallLeft, (hitPoint.y >= 0) ? goalTop : goalBottom) - hitPoint;
						
						startPoint = hitPoint;
						startDirection = dir2;
						
						path.Add (hitPoint); break;
					case 3:
						startPoint = new Vector2 (hitPoint.x, -hitPoint.y);
						
						path.Add (startPoint); break;
					default:
						startPoint = hitPoint;
						startDirection = exitDirection;
						
						path.Add (hitPoint); break;
				}
			} else {
				startPoint = hitPoint;
				startDirection = exitDirection;

				path.Add (hitPoint);
			}
		} while (hit.collider.gameObject.tag.Contains ("Wall") && path.Count <= maxPredictionCount);

		gameScript.SetProjectileTransform (this.transform);

		stopMovement = false;
	}


//____________________________________________________________\\\\\\___HelperMethods___//////_______________________________________________________________


//_________________\\\\\\___SetTag___//////_________________
// Sets projectile tag to change particle effect accordingly
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
	private void SetTag(string name) {
		if (name == "Goal_Red" || name == "Player1") {
			this.tag = "BallP1";
		} else {
			this.tag = "BallP2";
		}
	}


//_________________\\\\\\___SetRotation___//////_________________
// Randomly sets an initial start direction
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
	private void SetRotation(float i) {
		Vector2 direction = new Vector2 (Random.Range (-1.0f, 1.0f), i);
		direction.Normalize ();
//		float fact = 1.0f / Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2));
//		direction = direction * fact;

		this.transform.rotation = Quaternion.LookRotation(direction, Vector3.forward);
	}


//_________________\\\\\\___SetFieldMiddleRotation___//////_________________
// Sets direction to the middle top/bottom of the field
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
	private void SetFieldMiddleRotation(float x, float y) {
		Vector3 dir = new Vector3 (x, y, -6.0f) - this.transform.position;

		this.transform.rotation = ToolBox.GetRotationFromVector (dir);
	}


//_________________\\\\\\___SpeedUpProjectile___//////_________________
// Sets new projectile speed + initiates boost + updates speed
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
	private void SpeedUpProjectile(float fac, bool special = false) {
		gameScript.BallSpeedUp (fac, special);
		StartCoroutine (gameScript.BallSpeedBoost ());
		moveScript.UpdateBallSpeed ();
	}


//_________________\\\\\\___CheckScored___//////_________________
// Checks if projectile is within enemy part of field and
// decreases enemy health
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
	private void CheckScored() {
		if (this.tag == "BallP1" && this.transform.position.x > 0.0f) {
			gameScript.Player1Scored (true);
		} else if (this.tag == "BallP2" && this.transform.position.x < 0.0f) {
			gameScript.Player2Scored (true);
		}
	}


//_________________\\\\\\___DeactivateParticleObjs___//////_________________
// Deactivates all particle objects
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
	private void DeactivateParticleObjs() {
		foreach (GameObject ball in particleObjs) {
			ball.SetActive (false);
		}
	}


//_________________\\\\\\___ResetPath___//////_________________
// Resets path and bounceCount
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
	private void ResetPath() {
		path = new List<Vector2>();
		bounceCount = 0;
	}


//_________________\\\\\\___ResetBall___//////_________________
// Deactivates all particle objects + disables all specials +
// sets projectile to homePoisition + sets projectile tag +
// resets ball speed and updates it + resets path and sets
// projectile transform to null in game script
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
	private void ResetBall(string name) {
		this.DeactivateParticleObjs ();

		this.DisableAllSpecials ();

		this.transform.position = homePosition;

		this.SetTag (name);

		gameScript.ResetBallSpeed();
		moveScript.UpdateBallSpeed ();

		this.ResetPath ();
		gameScript.SetProjectileTransform (null);
	}


//_________________\\\\\\___DisableAllSpecials___//////_________________
// Disables special-mode + disables linear rotation + resets crystal +
// sets respective particle object
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
	private void DisableAllSpecials() {
		specialBall = false;
		this.EnableLinearRotation (false);
		crystal = -1;

		this.SpeedUpProjectile(0.0f);
	}


//_________________\\\\\\___EnableLinearRotation___//////_________________
// Enables/disables linear rotation
//‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾
	private void EnableLinearRotation(bool b) {
		linearRotation = b;
		linearRotationScript.enabled = b;
	}
}
