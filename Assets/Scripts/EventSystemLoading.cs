using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystemLoading : MonoBehaviour
{
    //This code just makes sure there there is only ever 1 event system no matter where you load the game from
    [HideInInspector]
    public static EventSystemLoading instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
