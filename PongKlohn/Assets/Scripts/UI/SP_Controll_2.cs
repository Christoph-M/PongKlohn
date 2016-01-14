using UnityEngine;
using System.Collections;
using Image=UnityEngine.UI.Image;

public class SP_Controll_2 : MonoBehaviour {
	Image special_bar;
	float tmpHealth=1;
	// Use this for initialization
	void Start () {
		special_bar=GameObject.Find ("mainCamera").transform.FindChild ("specialbar_player_2").FindChild ("specialbar").GetComponent<Image> ();
	}

	// Update is called once per frame
	void Update () {
		special_bar.fillAmount = tmpHealth;
	}
	public void changeHealth(){
		tmpHealth = Random.Range (0f, 1f);
	}	
}