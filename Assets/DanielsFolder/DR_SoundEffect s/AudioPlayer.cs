using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    public AudioClip[] clips;
    AudioSource audio;

    public float minPitch, maxPitch, minVol, maxVol;

    //playsettings
    public bool playOnEnable = false;
    bool isPlaying;

    //loop settings
    public bool loop;
    public float loopIntervals;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    #region handleActivation
    private void OnEnable()
    {
        if (playOnEnable)
        {
            StopAllCoroutines();
            StartCoroutine(PlayRepeating());
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        isPlaying = false;
    }
    #endregion

    public void Play()
    {
        audio.clip = clips[Random.Range(0, clips.Length - 1)];
        audio.pitch = Random.Range(minPitch, maxPitch);
        audio.volume = Random.Range(minVol, maxVol);
        audio.Play();
    }
    public void Stop()
    {
        isPlaying = false;
        StopAllCoroutines();
    }
    IEnumerator PlayRepeating()
    {
        if (isPlaying) yield return null;
        else
        {
            isPlaying = true;
            while(isPlaying == true)
            {
                Play();
                yield return new WaitForSeconds(loopIntervals);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Play();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(PlayRepeating());
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Stop();
        }
    }
}
