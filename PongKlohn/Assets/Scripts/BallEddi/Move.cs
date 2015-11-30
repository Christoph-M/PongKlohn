using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{
    public float speed = 10;
	public float angle = 0;
	
	public bool x_achse = true;
	public bool y_achse = false;
	public bool z_achse = false;

	void Start()
    {
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
