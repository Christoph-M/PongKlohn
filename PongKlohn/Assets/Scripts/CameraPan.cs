using UnityEngine;
using System.Collections;

public class CameraPan : MonoBehaviour {
	public float camResetTime = 0.2f;


	private Game gameScript;

	private Transform projectile;
	private Vector3 velocity = Vector3.zero;

	void Start() {
		gameScript = GameObject.FindObjectOfType (typeof(Game)) as Game;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameScript.GetProjectileTransform ()) {
			this.transform.position = new Vector3 (gameScript.GetProjectileTransform ().position.x / 10, this.transform.position.y, this.transform.position.z);
		} else if (this.transform.position != Vector3.zero) {
			this.ResetCamera ();
		}
	}

	private void ResetCamera() {
		this.transform.position = Vector3.SmoothDamp (this.transform.position, Vector3.zero, ref velocity, camResetTime);
	}
}
