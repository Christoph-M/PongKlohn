using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
	MeshRenderer shader;

	Vector2 offset = Vector2.zero;
	float up = 0.002f;
	// Use this for initialization
	void Start () {
		shader = this.GetComponent<MeshRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		shader.material.mainTextureOffset = offset;
		
		offset = new Vector2 (offset.x + up, offset.y + up * 10);

		if (offset.x >= 1.0f)
			offset = Vector2.zero;
	}
}
