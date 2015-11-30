using UnityEngine;
using System.Collections;

public class Timer
{	
	float timer =0f;
	float time = 0f;
	bool status = false;
	
	public void Update () 
	{
		if(timer<time)
		{
			timer+=Time.deltaTime;
			status = false;
		}
		else
		{
			status = true;
		}
	}
	
	public void RestTimer()
	{
		timer = 0f;
	}
	
	public void SetTimer(float t)
	{
		time = t;
	}
	
	public bool IsFinished()
	{
		return status;
	}
}
