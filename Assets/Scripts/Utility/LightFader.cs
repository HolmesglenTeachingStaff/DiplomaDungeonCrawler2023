using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFader : MonoBehaviour
{
    Light light;
    float originalIntensity;
    public float fadeInTime, fadeOutTime;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        originalIntensity = light.intensity;
        light.intensity = 0;
        StartCoroutine(FadeLight());
    }

    IEnumerator FadeLight()
    {
        light.intensity = 0;
        float timer = 0;
        WaitForEndOfFrame frame = new WaitForEndOfFrame();

        while(timer < fadeInTime)
        {
            light.intensity = Mathf.Lerp(0, originalIntensity, timer / fadeInTime);
            timer += Time.deltaTime;
            yield return frame;
        }
        timer = 0;
        yield return new WaitForSeconds(1f);
        while (timer < fadeOutTime)
        {
            light.intensity = Mathf.Lerp(originalIntensity, 0, timer / fadeOutTime);
            timer += Time.deltaTime;
            yield return frame;
        }
        //Destroy(gameObject);


    }
}
