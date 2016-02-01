using UnityEngine;
using System.Collections;

public class Curves : MonoBehaviour {

	public AnimationCurve Curve0 = AnimationCurve.EaseInOut(0f,0f,1f,1f);
	public AnimationCurve Curve1 = AnimationCurve.EaseInOut(0f,0f,1f,1f);
	public AnimationCurve Curve2 = AnimationCurve.EaseInOut(0f,0f,1f,1f);
	public AnimationCurve Curve3 = AnimationCurve.EaseInOut(0f,0f,1f,1f);
	
	public float GetCurve(float value,int curve)
	{
		if(curve == 0){return Curve0.Evaluate(value);}
		if(curve == 1){return Curve1.Evaluate(value);}
		if(curve == 2){return Curve2.Evaluate(value);}
		if(curve == 3){return Curve3.Evaluate(value);}
		return 0f;
	}
}
