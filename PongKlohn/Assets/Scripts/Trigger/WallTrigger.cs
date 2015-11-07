using UnityEngine;
using System.Collections;

public class WallTrigger : MonoBehaviour {
	
	void OnCollisionStay(Collision other) {
		if (other.gameObject.name == "Projectile") {
			Vector3 direction = Vector3.Reflect(other.gameObject.transform.forward, other.contacts[0].normal);
			other.gameObject.transform.rotation = Quaternion.LookRotation(direction);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
