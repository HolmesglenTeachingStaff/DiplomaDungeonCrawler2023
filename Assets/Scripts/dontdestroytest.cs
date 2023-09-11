using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontdestroytest : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Debug.Log("awake");
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start");
    }

    // Update is called once per frame
    void OnEnable()
    {
        Debug.Log("enable");
    }
    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("level was loaded");
    }
}
