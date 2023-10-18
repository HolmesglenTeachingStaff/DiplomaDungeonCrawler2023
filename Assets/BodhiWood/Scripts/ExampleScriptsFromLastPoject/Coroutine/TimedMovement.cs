using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedMovement : MonoBehaviour
{
    [SerializeField] 
    Transform[] positions;

    [SerializeField]
    public float moveSpeed;

    void Start()
    {
        StartCoroutine(Movements());
    }

    IEnumerator Movements()
    {
        for(int i = 0; i < positions.Length; i++)
        {           
            while (Vector3.Distance(transform.position, positions[i].position) > 0.5f)
            {
                transform.position = Vector3.Lerp(transform.position, positions[i].position, moveSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            if(i == positions.Length - 1)
            {
                i = -1;
            }
            yield return new WaitForSeconds(0.2f);
        }

        yield return null;
    }
}
