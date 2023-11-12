using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// tracks the position of an orbitTarget, and stores a vector that circles the orbit target in the OrbitPosition variable
/// </summary>
public class OrbitTarget : MonoBehaviour
{
    public GameObject orbitTarget;
    public float orbitSpeed, orbitDistance, orbitValue1, orbitValue2;
    public Vector3 orbitPosition;
    
    // Update is called once per frame
    void Update()
    {
        //determine OrbitValue;
        orbitValue1 = orbitDistance  * Mathf.Sin(Time.time * orbitSpeed);
        orbitValue2 = orbitDistance  * Mathf.Cos(Time.time * orbitSpeed);

        //projectOrbitOnto a Vector.
        Vector3 orbitVector = new Vector3(orbitValue1, 0, orbitValue2);

        //follow with orbit
        orbitPosition = orbitTarget.transform.position + orbitVector;
    }
    
}
