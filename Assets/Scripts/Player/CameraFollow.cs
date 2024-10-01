using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform followTransform;

    void LateUpdate()
    {
        if (followTransform == null)
        {
            return;
        }
        transform.position = new Vector3(followTransform.position.x, followTransform.position.y, transform.position.z);
    }
}