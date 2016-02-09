using UnityEngine;
using System.Collections;

public static class ToolBox {
	public const int p1 = 1;
	public const int p2 = 2;

    public static Quaternion GetRotationFromVector(Vector3 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0, 0, angle);
        //return Quaternion.LookRotation(dir,new Vector3(0,0,1));
        //Transform t = Transform.LookAt()
    }
}
