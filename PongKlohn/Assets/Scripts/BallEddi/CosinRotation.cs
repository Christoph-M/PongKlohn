using UnityEngine;
using System.Collections;

public class CosinRotation : MonoBehaviour
{
    public float speed = 1;
	public float angle = 4;
	public float intervalMuliplier = 1;
	//double startTime;
	float counter=0;
	    
	public bool x_rotation = false;
    public bool y_rotation = false;
    public bool z_rotation = false;
	
	public float offset = 0;
	float deltatime;
	Vector3 movement = Vector3.zero;
	Vector3 ache = Vector3.zero;
	
	void Start()
    {
		deltatime = Time.time;
    }
    
	float tempold = 0f;
	float tempdif = 0f;
	float temp =0f;
    void Update()
    {
        counter=(Time.time-deltatime)* speed;
		temp = offset + Mathf.Cos(counter);
		if(counter>360f)
		{
			deltatime = Time.time;
		}

		temp = angle * temp;
		tempdif = temp - tempold;
		tempold = temp;
		if(x_rotation){transform.rotation *= Quaternion.AngleAxis(tempdif,new Vector3(1,0,0));}
		if(y_rotation){transform.rotation *= Quaternion.AngleAxis(tempdif,new Vector3(0,1,0));}
		if(z_rotation){transform.rotation *= Quaternion.AngleAxis(tempdif,new Vector3(0,0,1));}		
    }    
}
