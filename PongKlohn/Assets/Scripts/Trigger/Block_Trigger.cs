using UnityEngine;
using System.Collections;

public class Block_Trigger : MonoBehaviour {
//	private Game goal;
//	
//	// Use this for initialization
//	void Start () {
//		goal = GameObject.FindObjectOfType (typeof(Game)) as Game;
//	}
//	
//	void OnTriggerStay2D(Collider2D other) {
//		if (other.name == "Projectile") {
//			if (this.name == "Block_Trigger_Player_01") {
//				float lShift = Input.GetAxisRaw ("BlockP1");
//
//				Vector2 playerDirection = gameMechanics.figure1.transform.position - other.transform.position;
//				
//				RaycastHit2D hit = Physics2D.Raycast(other.transform.position, playerDirection);
//				Debug.DrawRay(other.transform.position, playerDirection, Color.green, 20);
//				Debug.Log("Hit object: " + hit.collider.gameObject.name);
//
//				if (lShift > 0) {
//					Vector2 position = other.transform.position;
//					Vector2 direction = Vector2.Reflect (gameMechanics.figure1.transform.right, hit.normal);
//					float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
//					
//					Object.Destroy (other.gameObject);
//					
//					Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
//					
//					gameMechanics.instantiateSphere(gameMechanics.sphereRO, position, rotation);
//				}
//			} else {
//				float rShift = Input.GetAxisRaw ("BlockP2");
//
//				Vector2 playerDirection = gameMechanics.figure2.transform.position - other.transform.position;
//				
//				RaycastHit2D hit = Physics2D.Raycast(other.transform.position, other.transform.TransformDirection(playerDirection));
//				Debug.DrawRay(other.transform.position, playerDirection, Color.green, 20);
//				Debug.Log("Hit object: " + hit.collider.gameObject.name);
//
//				if (rShift > 0){					
//					Vector2 position = other.transform.position;
//					Vector2 direction = Vector2.Reflect (gameMechanics.figure2.transform.right, hit.normal);
//					float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
//
//					Object.Destroy (other.gameObject);
//					
//					Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
//					
//					gameMechanics.instantiateSphere(gameMechanics.sphereBO, position, rotation);
//				}
//			}
//		}
//	}
}
