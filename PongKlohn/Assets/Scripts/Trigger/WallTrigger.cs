using UnityEngine;
using System.Collections;

public class WallTrigger : MonoBehaviour {
	
	void OnCollisionEnter2D(Collision2D other) {
		RaycastHit2D hit;

		if (other.gameObject.name == "Projectile") {
			Debug.Log (other.gameObject);

			float distance = other.transform.right.magnitude;
			Vector2 dir = other.transform.right / distance;

			hit = Physics2D.Raycast (other.transform.position, other.transform.right);
			Debug.Log (hit.normal);
			Vector2 direction = Vector2.Reflect(dir, other.contacts[0].normal);
			Debug.Log (direction);
			Debug.DrawRay(other.contacts[0].point, direction, Color.green, 20);

			Quaternion rotation = Quaternion.LookRotation (direction, Vector3.forward);
			//rotation.Set(rotation.x, rotation.y, 0.0f, 0.0f);
			
			other.transform.rotation = rotation;
			//other.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * 5000, ForceMode2D.Impulse);
		}
	}
}
