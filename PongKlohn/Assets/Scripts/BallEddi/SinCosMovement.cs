using UnityEngine;
using System.Collections;

public class SinCosMovement : MonoBehaviour {
	public bool sinus = false;
	public bool cosinus = false;

	public float speed = 1;
	public float range = 1;
	//double startTime;
	
	public bool x_movement = false;
	public bool y_movement = false;
	public bool z_movement = false;
	
	public float offset = 0;
	float deltatime;
	float counter=0;
	
	void Start()
	{
		deltatime = Time.time;
	}

	float temp = 0f;
	float tempSin = 0f;
	float tempCos = 0f;
	public void Update_()
	{
		counter=(Time.time-deltatime)* speed;

		if (sinus) {
			tempSin = offset + Mathf.Sin (counter);
		} 

		if (cosinus) {
			tempCos = offset + Mathf.Cos (counter);
		}

		if(counter>360f)
		{
			deltatime = Time.time;
		}

		temp = tempSin + tempCos;

		if(x_movement){transform.position += transform.TransformDirection(new Vector3(1,0,0)) * range * temp;}
		if(y_movement){transform.position += transform.TransformDirection(new Vector3(0,1,0)) * range * temp;}
		if(z_movement){transform.position += transform.TransformDirection(new Vector3(0,0,1)) * range * temp;}		
	}
}
