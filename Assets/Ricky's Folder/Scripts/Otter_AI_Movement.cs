using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class Otter_AI_Movement : MonoBehaviour
{
    public float movementSpeed = 20f;
    public float rotationSpeed = 100f;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;

    Rigidbody rb;
    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isWandering == false)
        {
            StartCoroutine(Wander());
        }
        if(isRotatingRight == true)
        {
            transform.Rotate(transform.up *Time.deltaTime * rotationSpeed);
        }
        if(isRotatingLeft == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * -rotationSpeed);
                       
        }
        if (isWalking == true)
        {
            rb.AddForce(transform.forward * movementSpeed);
            animator.SetBool("isWalking", true);
        }
        if (isWalking == false)
        {
            animator.SetBool("isWalking", false);
        }
    }

    IEnumerator Wander()
    {
        int rotationTime = Random.Range(1, 3);
        int rotateWait = Random.Range(1, 3);
        int rotateDirection = Random.Range(1, 2);
        int walkWait = Random.Range(1, 3);
        int WalkTime = Random.Range(1, 3);

        isWandering = true;
        yield return new WaitForSeconds(walkWait);

        isWalking = true;
        yield return new WaitForSeconds(walkWait);

        isWalking = false;
        yield return new WaitForSeconds(rotateWait);

        if(rotateDirection == 1)
        {
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotationTime);
            isRotatingLeft = false;

        }
        
        if(rotateDirection == 2)
        {
            isRotatingRight = true;
            yield return new WaitForSeconds(rotationTime);
            isRotatingRight = false;

        }

        isWandering = false;
    }
    



}
