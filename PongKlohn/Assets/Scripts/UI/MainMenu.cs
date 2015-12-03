using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : UserInterface {

	// Use this for initialization
	void Start () {
		gameScript = GameObject.FindObjectOfType (typeof(Game)) as Game;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
