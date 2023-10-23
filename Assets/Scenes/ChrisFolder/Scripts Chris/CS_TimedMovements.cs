using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_TimedMovements : MonoBehaviour
{
    [SerializeField] Transform [] positions; //[] is to create a list of positions

    [SerializeField] float moveSpeed;

    int positionCounter;

    

    // Start is called before the first frame update
    void Start()
    {
        
        //this begins the below IEnumerator AS a Coroutine
        StartCoroutine(Movements());
    }

    //this below is the long way to write down the coroutine/IEnumerator
    /*void Update() 
    {
        if(positionCounter == 0)
        {
            transform.position = Vector3.Lerp (transform.position, positions[0].position, moveSpeed * Time.deltaTime);
        }
        else if(positionCounter == 1)
        {
            transform.position = Vector3.Lerp (transform.position, positions[1].position, moveSpeed * Time.deltaTime);
        }
        
    }*/

    IEnumerator Movements ()
    {

        for(int i = 0; i < positions.Length; i++) // this makes the object repeat until the length of the positions is completed, by repeating it and adding +1 each time until it's at the max length
        {
        //below beomes an update loop using the yield return new as the waiting period before repeating
        while (Vector3.Distance(transform.position, positions[i].position) > 0.5) //changed 0 to "i" as it's now reffrenced above
        {
            transform.position = Vector3.Lerp(transform.position, positions[i].position, moveSpeed * Time.deltaTime); //changed 0 to "i" as it's now reffrenced above
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(3);
        }

        Debug.Log("I'ma pop off soon");
        yield return new WaitForSeconds(3);
        Debug.Log("POP");
        yield return new WaitForSeconds(1);
        Debug.Log("Pop");

        

        //transform.localScale *= 3; //(this would expand it times 3)

        yield return null;
    }

}
