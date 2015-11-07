using UnityEngine;
using System.Collections;

public class GoalTrigger : MonoBehaviour {
	Game goal;

	void Start(){
		goal = GameObject.FindObjectOfType (typeof(Game)) as Game;
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "Projectile") {
			goal.setTriggeredGoal(gameObject.name as string);

			Collider.Destroy (other.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
