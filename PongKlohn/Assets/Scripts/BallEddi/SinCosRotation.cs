using UnityEngine;
using System.Collections;

public class SinCosRotation : MonoBehaviour {
	public bool sinus = false;
	public bool cosinus = false;

	public float speed = 1;
	public float angle = 4;
	//double startTime;
	
	public bool x_rotation = false;
	public bool y_rotation = false;
	public bool z_rotation = false;
	
	public float offset = 0;


	private float deltatime;
	private float counter = 0;
	
	void Start()
	{
		deltatime = Time.time;
	}
	
	float tempold = 0f;
	float tempdif = 0f;
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
		
		temp = (tempSin + tempCos) * angle;
		tempdif = temp - tempold;
		tempold = temp;
		if(x_rotation){transform.rotation *= Quaternion.AngleAxis(tempdif,new Vector3(1,0,0));}
		if(y_rotation){transform.rotation *= Quaternion.AngleAxis(tempdif,new Vector3(0,1,0));}
		if(z_rotation){transform.rotation *= Quaternion.AngleAxis(tempdif,new Vector3(0,0,1));}		
	}
}
