using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	//public GameObject shootLeft; 
	//public GameObject shootForward; 
	//public GameObject shootRight;
	//public GameObject shootSuppi;
	
	
	private Rigidbody2D myTransform;
	private Animator animator;
	
	private string xAxis = "x";
	private string yAxis = "y";
	
	public float speed = 10;
	public bool InvertMotion = false;
	private float motionInverter = 1;
	
	void Start() 
	{
		animator = GetComponent<Animator>();
		myTransform = this.GetComponent<Rigidbody2D>();
		if(InvertMotion)
		{
			motionInverter = -1;
		}
		else
		{
			motionInverter = 1;
		}
	}
	
	void Update()
	{
		Vector2 direction = new Vector2(Input.GetAxis(xAxis),Input.GetAxis(yAxis));
		myTransform.AddForce (direction * speed);
		Debug.Log(direction);
		animator.SetFloat("xAxis",direction.x * motionInverter);
		animator.SetFloat("yAxis",direction.y * motionInverter);
	}
	
	
	public void SetInputAxis(string x,string y)
	{
		xAxis = x;
		yAxis = y;
	}
	
	public Vector3 GetProjectilePositin() 
	{
		return myTransform.gameObject.transform.position;
	}
	
	public Quaternion Rotation() 
	{
		return myTransform.gameObject.transform.rotation;
	}
	
	public Vector3 GetRotation()
	{
		return new Vector3(myTransform.gameObject.transform.rotation.x, myTransform.gameObject.transform.rotation.y, myTransform.gameObject.transform.rotation.z);
	}
}
