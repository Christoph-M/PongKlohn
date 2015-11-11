using UnityEngine;
using System.Collections;

public class WallTrigger : MonoBehaviour {
	
	void OnCollisionEnter2D(Collision2D other) {
		RaycastHit2D hit;

		if (other.gameObject.name == "Projectile") {
			Debug.Log (other.gameObject);
			hit = Physics2D.Raycast (other.gameObject.transform.position, this.transform.position);
			Vector2 direction = Vector2.Reflect(other.gameObject.transform.forward, hit.normal);
			Debug.DrawRay(other.gameObject.transform.position, direction, Color.green, 20);
			other.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * 5000, ForceMode2D.Impulse);
		}
	}
}
