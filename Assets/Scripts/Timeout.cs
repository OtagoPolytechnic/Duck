using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeout : MonoBehaviour
{
    public void DeathTimeout(float time, GameObject obj)
    {
        obj.AddComponent<Animator>();
    }
}
