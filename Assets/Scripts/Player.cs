using UnityEngine;
using System.Collections;

public class Player
{
	Transform myTransform;
	//private Transform transform;
	public Player(Transform trans)
	{
		myTransform = trans;
	}

	public void Move(Vector3 direction, float speed)
	{
		myTransform.position += (direction * Time.deltaTime) * speed;
	}

	public Vector3 GetProjectilePositin()
	{
		return myTransform.position;
	}
	
	public Quaternion Rotation()
	{
		return myTransform.rotation;
	}
	
	public Vector3 GetRotation(){
		return new Vector3(myTransform.rotation.x, myTransform.rotation.y, myTransform.rotation.z);
	}
}
