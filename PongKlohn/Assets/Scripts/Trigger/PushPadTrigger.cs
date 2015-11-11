using UnityEngine;
using System.Collections;

public class PushPadTrigger : MonoBehaviour {
	public Rigidbody2D pushPadO;

	private Rigidbody2D pushPadC;

	private int voidLayer;

	void Start(){
		voidLayer = LayerMask.NameToLayer ("Void");
		pushPadO.gameObject.layer = voidLayer;
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.name == "Projectile") {
			pushPadC = Instantiate (pushPadO, this.transform.position, this.transform.rotation) as Rigidbody2D;
			pushPadC.AddForce (pushPadC.gameObject.transform.right * 1.5f, ForceMode2D.Impulse);

			Object.Destroy (pushPadC.gameObject, 0.1f);
		}
	}
}
