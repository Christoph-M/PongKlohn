using UnityEngine;
using System.Collections;

public class Timer
{	
	float time = 0f;
	bool status = false;
	public Timer(float t)
	{
		time = t;
	}
	public Timer()
	{
		Debug.Log("Timer Set up");
	}
	
	public float UpdateTimer() 
	{
		if(0f<time)
		{
			time-=Time.deltaTime;
			status = false;
		}
		else
		{
			status = true;
		}
		return time;
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
