using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnAnimationEnd : MonoBehaviour
{
    public void DestroySelf() // can be placed on any animation you want deleted after animation plays
    {
        Destroy(gameObject);
    }
}
