using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{
	public float angle = 0.0f;
	
	public bool x_achse = true;
	public bool y_achse = false;
	public bool z_achse = false;


	private Game game;

	private float speed;
	
	void Start()
    {
		game = GameObject.FindObjectOfType (typeof(Game)) as Game;

		speed = game.GetBallSpeed();

		if(x_achse){transform.rotation *= Quaternion.AngleAxis(angle,new Vector3(0,0,1));}
		if(y_achse){transform.rotation *= Quaternion.AngleAxis(angle,new Vector3(1,0,0));}
		if(z_achse){transform.rotation *= Quaternion.AngleAxis(angle,new Vector3(0,1,0));}
    }

    public void Update_()
    {
		if(x_achse){transform.position += transform.TransformDirection(new Vector3(1,0,0)) * (Time.deltaTime * speed);}
		if(y_achse){transform.position += transform.TransformDirection(new Vector3(0,1,0)) * (Time.deltaTime * speed);}
		if(z_achse){transform.position += transform.TransformDirection(new Vector3(0,0,1)) * (Time.deltaTime * speed);}
    }
}
