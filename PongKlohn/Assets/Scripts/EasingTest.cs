using System;
using UnityEngine;
public class EasingTest{

	protected delegate double Ease(double t, double b, double c, double d);
	protected Ease ease;

	public EasingTest (){

		float start = 0;
		float change = 50;
		float duration = 10;
		
		//ease = Easing.Linear;
		Debug.Log(ease.Method.Name);
		for(float t = 0; t < duration; t++){
			Debug.Log(ease(t, start, change, duration));
		}

		//ease = Easing.QuartEaseInOut;
		Debug.Log(ease.Method.Name);
		for(float t = 0; t < duration; t++){
			Debug.Log(ease(t, start, change, duration));
		}

		// cast to float like so for use in Unity:
		// (float)ease(t, start, change, duration);

		// or convert the Easing class to floats with Mathf and
		// try not to fuck up the lack of accuracy ;)

	}
}