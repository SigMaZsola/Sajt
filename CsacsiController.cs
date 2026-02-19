
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using System;
using System.IO;
using Asino;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System.Collections;


public class CsacsiController : MonoBehaviour
{
private Vector3 targetPosition;

    private Vector3 hitPoint;

    public Animator animator;
    public Rigidbody hips;
    
    public GameObject  target;

    [SerializeField] GameObject pathTarget;
    private GameObject crowdTarget;


    public float speed = 50f;

    public float rayDistance = 0.3f;
    [SerializeField] LayerMask ground;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] csacsiBubi csBubi;
    private int pushCounter;
    public bool isGrounded;
    private bool isBumped;
    public LayerMask playerLayer;
    public LayerMask hordeLayer;

    public bool toTarget = true;
    public int csacsiNumber = 1;
    
    enum State 
    {
    calm,
    idle,
    scared
    }

    State csacsiState = State.calm;

    void Awake()    
    {
        
        Physics.IgnoreLayerCollision(
        LayerMask.NameToLayer("blue"),
        LayerMask.NameToLayer("blue"),
        true);

        Physics.IgnoreLayerCollision(
        LayerMask.NameToLayer("red"),
        LayerMask.NameToLayer("red"),
        true);


    }
    void Start()
    {
        csacsiState = State.calm;
        animator.SetBool("isWlak", true);
        hips = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        CheckGround();

    }

    void Update()
    {
        if(hips.transform.position.y < -20){hips.transform.position = new Vector3(0,0,0);}
        var crowds = FindObjectsByType<CrowdController>(FindObjectsSortMode.None);
        foreach (var c in crowds)
        {
            var input = c.GetComponent<PlayerInput>();
            if (input != null && input.playerIndex == csacsiNumber - 1)
            {
                crowdTarget = c.gameObject;
                break;
            }
        }
        if(!isGrounded)return;
        if(!toTarget)return;
        if (target != null)
        {
            targetPosition = target.transform.position;
            MoveToTarget();
        }
        DetermineState();
    }
    private void CheckGround()
    {
        isGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            rayDistance,
            ground
        );
    }


    private void DetermineState()
{
    if (crowdTarget == null) return;

    Vector3 pos = hips.position;
    Vector3 flatPos = new Vector3(pos.x, 0, pos.z);
    Vector3 crowdPos = crowdTarget.transform.position;
    Vector3 flatCrowd = new Vector3(crowdPos.x, 0, crowdPos.z);
    float fleeRadius = 15f;
    float crowdDistance = Vector3.Distance(flatPos, flatCrowd);
    
    if (csacsiState == State.calm)
    {
        
        animator.SetBool("isWlak", true);
        if (crowdDistance < fleeRadius)
        {
            csacsiState = State.scared;
            csBubi.AppearBubi(csacsiState.ToString());
            csBubi.canShow = true;
        }
    }
    else if (csacsiState == State.scared)
    {
        
        animator.SetBool("isWlak", false);
        if (crowdDistance > fleeRadius)
        {
            csacsiState = State.calm;
            csBubi.AppearBubi(csacsiState.ToString());
            csBubi.canShow = true;
        }
    }
    else if (csacsiState == State.idle)
    {
        
        if (crowdDistance > fleeRadius)
        {
            StartCoroutine(ChangeToCalm());
        }
    }
}

IEnumerator ChangeToCalm()
{
    yield return new WaitForSeconds(1f);
    csacsiState = State.calm;
    csBubi.AppearBubi(csacsiState.ToString());
    csBubi.canShow = true;
}

private void MoveToTarget()
{
    if (crowdTarget == null) return;

    Vector3 pos = hips.position;
    Vector3 crowdPos = crowdTarget.transform.position;
    Vector3 flatPos = new Vector3(pos.x, 0, pos.z);
    Vector3 flatCrowd = new Vector3(crowdPos.x, 0, crowdPos.z);

    float crowdDistance = Vector3.Distance(flatPos, flatCrowd);
    Vector3 direction = Vector3.zero;

    if (csacsiState == State.scared)
    {
        direction = (flatPos - flatCrowd).normalized;
        
        RaycastHit hit;
        bool hitSomething = Physics.Raycast(
            hips.position,
            direction,
            out hit,
            5f,
            obstacleLayer
        );

        if (hitSomething && crowdDistance < 15)
        {

            csacsiState = State.idle;
            hips.linearVelocity = Vector3.zero;
            return;
        }
    }
    else if (csacsiState == State.calm)
    {
        direction = hips.transform.forward;
        
        RaycastHit hit;
        bool hitSomething = Physics.Raycast(
            hips.position,
            direction,
            out hit,
            5f,
            obstacleLayer
        );

        if (hitSomething)
        {

            direction = Vector3.Reflect(direction, hit.normal).normalized;
        }
    }
    else if (csacsiState == State.idle)
    {

        hips.linearVelocity = Vector3.zero;
        return;
    }

    float damping = 8f;
    Vector3 desiredVelocity = direction * speed;
    Vector3 steering = desiredVelocity - hips.linearVelocity;

    hips.AddForce(steering * damping, ForceMode.Acceleration);

    if (direction.sqrMagnitude > 0.001f)
    {
        hips.MoveRotation(Quaternion.LookRotation(direction));
    }
}

}