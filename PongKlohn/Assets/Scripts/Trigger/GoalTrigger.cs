using UnityEngine;
using System.Collections;

public class GoalTrigger : MonoBehaviour {
	private Game goal;

	void Start(){
		goal = GameObject.FindObjectOfType (typeof(Game)) as Game;
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.name == "Projectile") {
			Debug.Log ("Collided");
			goal.setTriggeredGoal(this.gameObject.name as string);

			Object.Destroy (other.gameObject);
		}
	}
}
