using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    [SerializeField]
    Transform target;
    bool following;

    void LateUpdate()
    {
        if (following && target.position.x >= -1.5f && target.position.x <= 45)
        {
            Follow();
        }
    }

    void Follow()
    {
        Vector3 aux = transform.position;
        aux.x = target.position.x + 1.5f;
        transform.position = aux;
    }

    public void SetFollow(bool bol)
    {
        following = bol;
    }
}
