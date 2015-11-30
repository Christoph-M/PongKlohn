using UnityEngine;
using System.Collections;

public class physicsToggle : MonoBehaviour {
	private bool triggered = false;

	void OnTriggerStay2D(Collider2D other){
		triggered = true;

		this.GetComponentInParent<Ball> ().GetComponent<Rigidbody2D> ().isKinematic = false;
	}

	void OnTriggerExit2D(Collider2D other){
		triggered = false;
	}

	void Update(){
		if (!triggered) {
			this.GetComponentInParent<Ball> ().GetComponent<Rigidbody2D> ().isKinematic = true;
		}
	}
}
