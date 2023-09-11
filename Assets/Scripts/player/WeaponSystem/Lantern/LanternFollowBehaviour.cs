using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternFollowBehaviour : MonoBehaviour
{
    //follow settings
    public float followSpeed;
    public Transform followTarget;

    //attack settings
    public Vector3 targetDestination;
    public float attackSpeed;
    public float recoverySpeed;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desiredPosition = Vector3.Lerp(transform.position, followTarget.position, followSpeed * Time.deltaTime);
        transform.position = desiredPosition;
    }
}
