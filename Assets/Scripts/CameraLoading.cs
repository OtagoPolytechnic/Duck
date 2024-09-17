using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLoading : MonoBehaviour
{
    //This code just makes sure there there is only ever 1 event system no matter where you load the game from
    [HideInInspector]
    public static CameraLoading instance;
    public bool isMainCamera = false; //This is to make sure that the main camera is kept instead of a menu one
    void Awake()
    {
        if (isMainCamera && instance != null) //If this is the main camera and there is already another camera, destroy it
        {
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance == null)
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
