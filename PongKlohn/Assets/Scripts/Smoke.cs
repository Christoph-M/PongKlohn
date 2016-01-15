using UnityEngine;
using System.Collections;

public class Smoke : MonoBehaviour
{
	public float lifetime = 1f;
	private float time = 0f;
	
	void Update()
	{
		time +=Time.deltaTime;
		if(time>=lifetime)
		{
			Destroy(this.gameObject);		
		}
	}
}