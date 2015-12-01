using UnityEngine;
using System.Collections;

public class LinearRotation : MonoBehaviour
{
    public float speed = 1;
	//double startTime;
	float counter=0;
	public bool x_rotation = false;
    public bool y_rotation = false;
    public bool z_rotation = false;
	public bool invertDirection;
	int direction;

	void Start()
    {
		if(invertDirection){direction = -1;}
		else{direction = 1;}
    }
    
	public void Update_()
    {
		if(x_rotation){transform.rotation *= Quaternion.AngleAxis(Time.deltaTime * speed * direction,new Vector3(1,0,0));}
		if(y_rotation){transform.rotation *= Quaternion.AngleAxis(Time.deltaTime * speed * direction,new Vector3(0,1,0));}
		if(z_rotation){transform.rotation *= Quaternion.AngleAxis(Time.deltaTime * speed * direction,new Vector3(0,0,1));}		
    }    
}
