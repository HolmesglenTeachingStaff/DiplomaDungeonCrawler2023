using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovment : MonoBehaviour
{
    #region Component settings
    CharacterController cc;
    Animator anim;
    Camera cam;
    public GameObject model;
    public PlayerIKHandling IKhandling;
    #endregion

    #region preferences
    [Header("General Movement Settings")]
    [SerializeField] public float moveSpeed;
    [SerializeField] public float defaultMoveSpeed;
    [SerializeField] float turnSpeed;
    [Header("Dash Settings")]
    [SerializeField] float dashDistance;
    [SerializeField] float  dashSpeed;
    [SerializeField] float dashCool;
    public  float lastDash = 0f;
    public bool shouldLook = true;
    #endregion

    #region private variables
    [HideInInspector] public Vector3 moveDirection;
    private Vector3 dashTarget;
    private Vector3 preDashPos;
    private Quaternion lookDirection;
    public  Vector3 lookTarget;
    private float gravityValue = -9.81f;
    private Vector3 playerVelocity;
    private MousePosition mousePosition;
    public enum state { Dashing, Walking, Attacking }
    public state currentState = state.Walking;
    #endregion

    public bool canMove;

    // Start is called before the first frame update
    void Awake()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        //mousePosition = new MousePosition();
        cam = Camera.main;
        mousePosition = GetComponent<MousePosition>();
        //InvokeRepeating("RandomIdlePicker", 5, 5);
    }

    // Update is called once per frame
    void Update()
    {
        if (cc.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        if (canMove) ProcessMovement();
        ProcessGravity();
        var speed = currentState == state.Dashing ? dashSpeed : moveSpeed;
        
         cc.Move(moveDirection.normalized * speed * Time.deltaTime);

        if (Input.GetButtonUp("Jump")) StartDash();

        if(shouldLook) DetermineDirection();
        ProcessAnimation();
    }

    void DetermineDirection()
    {
        //get mouse position:
        Vector3 mousePos = mousePosition.GetPosition(cam, transform.position) + Vector3.up;
        //check if mouse is close

        if (Vector3.Distance(transform.position, mousePos) < 1) return;

        //determine base look direction
        lookTarget =  mousePos - transform.position;
        lookTarget.y = model.transform.localPosition.y;

        //LERP
        Quaternion lookDirection = Quaternion.LookRotation(lookTarget, Vector3.up);

        model.transform.localRotation = Quaternion.Lerp(model.transform.localRotation, lookDirection, turnSpeed * Time.deltaTime);

        var IKTarget = mousePos;
        IKTarget.y = model.transform.position.y + 1.6f;

        Vector3 newIkTarget = Vector3.Lerp(IKhandling.lookTarget, IKTarget, turnSpeed);
        IKhandling.lookTarget = newIkTarget;
    }

    void ProcessGravity()
    {
        playerVelocity.y += gravityValue * Time.deltaTime;
        cc.Move(playerVelocity * Time.deltaTime);
    }
    void ProcessMovement()
    {
        if (currentState != state.Dashing)
        {
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");

            var camOffsetx = cam.transform.right;
            var camOffestz = Vector3.Cross(camOffsetx, Vector3.up);

            if (h != 0 || v != 0)
            {
                anim.SetBool("move", true);
                moveDirection = (h * camOffsetx) + (v * camOffestz);
            }
            else
            {
                anim.SetBool("move", false);
                moveDirection = new Vector3(0, moveDirection.y, 0);
            }
        }
        else if (currentState == state.Dashing)
        {
            if(Physics.Raycast(transform.position, moveDirection, 1f))
            {
                currentState = state.Walking;
                return;
            }
            if (Vector3.Distance(transform.position, dashTarget) <= 0.2f || Vector3.Distance(preDashPos, transform.position) > dashDistance)
            {
                currentState = state.Walking;
                return;
            }
        }
        lastDash += Time.deltaTime;
    }
    void ProcessAnimation()
    {
        Vector3 worldDeltaPosition = cc.velocity;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(model.transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(model.transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);
        
        anim.SetFloat("ForwardMomentum", deltaPosition.y);
        anim.SetFloat("SideMomentum", deltaPosition.x);
    }
    void StartDash()
    {
        if (currentState == state.Dashing || currentState == state.Attacking || dashCool > lastDash) return;
        Debug.Log("Dash");
        RaycastHit hit;
        if(Physics.Raycast(transform.position, moveDirection, out hit, dashDistance))
        {
            dashTarget = hit.point;
        }
        else
        {
            dashTarget = transform.position + moveDirection * dashDistance;
        }
        anim.Play("RollForward");
        model.transform.rotation = Quaternion.LookRotation(moveDirection);
        shouldLook = false;
        preDashPos = transform.position;
        lastDash = 0f;
        currentState = state.Dashing;
    }
 
}
