using UnityEngine;
using System.Collections;

public class DashCoolDown : MonoBehaviour {

    float lifeTime = 2f;
    void Start()
    {
        Object.Destroy(gameObject, lifeTime);
    }
}
