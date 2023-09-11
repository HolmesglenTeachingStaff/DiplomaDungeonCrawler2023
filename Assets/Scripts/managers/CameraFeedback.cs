using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFeedback : MonoBehaviour
{
    public static CameraFeedback instance;
    public float shakeAmount;

    CinemachineImpulseSource impulseSource;
    public bool canPause = true;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            
        }
        else
        {
            instance = this;
        }
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    public void FramePause(float timeToUnPause)
    {
        if (!canPause) return;
        Time.timeScale = 0.1f;
        Invoke("UnPause", timeToUnPause * Time.timeScale);
        canPause = false;
    }
    public void UnPause()
    {
        Time.timeScale = 1f;
        canPause = true;
    }
    public void ScreenShake(float shakeAmount)
    {
        impulseSource.GenerateImpulse(Vector3.one * shakeAmount);
    }
}
