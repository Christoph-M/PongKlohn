using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{
    public float speed = 1;
	public float angle = 0;
	
	public bool x_achse = false;
	public bool y_achse = true;
	public bool z_achse = false;
	
	//double startTime;
	
	
	void Start()
    {
		if(x_achse){transform.rotation *= Quaternion.AngleAxis(angle,new Vector3(0,0,1));}
		if(y_achse){transform.rotation *= Quaternion.AngleAxis(angle,new Vector3(1,0,0));}
		if(z_achse){transform.rotation *= Quaternion.AngleAxis(angle,new Vector3(0,1,0));}
    }
    void Update()
    {	
		if(x_achse){transform.position += transform.TransformDirection(new Vector3(1,0,0)) * (Time.deltaTime * speed);}
		if(y_achse){transform.position += transform.TransformDirection(new Vector3(0,1,0)) * (Time.deltaTime * speed);}
		if(z_achse){transform.position += transform.TransformDirection(new Vector3(0,0,1)) * (Time.deltaTime * speed);}
    }    
}
