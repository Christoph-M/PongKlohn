using UnityEngine;
using System.Collections;

public static class ToolBox {
	public const int p1 = 1;
	public const int p2 = 2;

    public static Quaternion GetRorationFromVector(Vector3 dir)
    {
        float angel = Mathf.Atan2(dir.y, dir.x) *Mathf.Rad2Deg;
        Debug.Log("dir:" + dir + "   angle:" + angel);
        return Quaternion.Euler(0,0,angel);
        //return Quaternion.LookRotation(dir,new Vector3(0,0,1));
        //Transform t = Transform.LookAt()
    }
}
