using UnityEngine;
using System.Collections;
using Image=UnityEngine.UI.Image;

public class HP_Controll : MonoBehaviour {
	Image healthbar_full;
	float tmpHealth=1;
	// Use this for initialization
	void Start () {
		healthbar_full=GameObject.Find ("mainCamera").transform.FindChild ("healthbar_player_1").FindChild ("healthbar").GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		healthbar_full.fillAmount = tmpHealth;
	}
	public void changeHealth(){
		tmpHealth = Random.Range (0f, 1f);
	}	
}