using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ControlerInput : UIBehaviour {
	private EventSystem eventSystem;

	// Use this for initialization
	void Start () {
		eventSystem = EventSystem.current;
	}
	
	// Update is called once per frame
	void Update () {
		float vert = Input.GetAxisRaw ("ControllerVertical");
		float hori = Input.GetAxisRaw ("ControllerHorizontal");

		if (vert != 0.0f) {

		}
	}
}
