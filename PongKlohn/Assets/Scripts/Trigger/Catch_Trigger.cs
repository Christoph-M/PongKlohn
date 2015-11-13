using UnityEngine;
using System.Collections;

public class Catch_Trigger : MonoBehaviour {
	public Rigidbody2D catchTriggerO;

	private Rigidbody2D catchTriggerC;
	
	private int voidLayer;
	// Use this for initialization
	void Start () {
		voidLayer = LayerMask.NameToLayer ("Void");
		catchTriggerC = Instantiate (catchTriggerO, this.transform.position, this.transform.rotation) as Rigidbody2D;
		catchTriggerC.gameObject.layer = voidLayer;
	}
	
	// Update is called once per frame
	void Update () {
		catchTriggerC.transform.position = this.transform.position;
	}
}
