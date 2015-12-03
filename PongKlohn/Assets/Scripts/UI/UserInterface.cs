using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UserInterface : MonoBehaviour {
	public GameObject canvas;


	protected Game gameScript;

	// Use this for initialization
	void Start () {
		gameScript = GameObject.FindObjectOfType (typeof(Game)) as Game;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
