using UnityEngine;
using System.Collections;

public class SinMovement : MonoBehaviour
{
    public float speed = 1;
	public float intervalMuliplier = 1;
	//double startTime;
	float counter=0;

	
	public bool x_movement = false;
    public bool y_movement = false;
    public bool z_movement = false;
	
	public float offset = 0;
	float deltatime;
	
	void Start()
    {
		deltatime = Time.time;
    }
    
	float temp = 0;
    void Update()
    {
        counter=(Time.time-deltatime)* speed;
		temp = offset + Mathf.Sin(counter);
		if(counter>360f)
		{
			deltatime = Time.time;
		}
		
		if(x_movement){transform.position += transform.TransformDirection(new Vector3(1,0,0)) * intervalMuliplier * temp;}
		if(y_movement){transform.position += transform.TransformDirection(new Vector3(0,1,0)) * intervalMuliplier * temp;}
		if(z_movement){transform.position += transform.TransformDirection(new Vector3(0,0,1)) * intervalMuliplier * temp;}		
    }    
}
