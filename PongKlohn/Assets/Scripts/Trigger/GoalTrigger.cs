using UnityEngine;
using System.Collections;

public class GoalTrigger : MonoBehaviour {
	private Game goal;

	void Start(){
		goal = GameObject.FindObjectOfType (typeof(Game)) as Game;
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "Projectile") {
			goal.setTriggeredGoal(this.gameObject.name as string);

			Collider.Destroy (other.gameObject);
		}
	}
}
