using System.Numerics;
using Asino;
using UnityEngine;
using System.Collections;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

using Quaternion = UnityEngine.Quaternion;
using System.Linq.Expressions;
using System.Collections.Generic;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    private Vector3 targetPosition;
   
   public Vector3 parentmovement;



    public ActionTimer actionTimer;

    public LayerMask hitLayers; 

    private Vector3 screenPos;

    public Rigidbody hips;
    
    public float speed = 50f;

    public Animator animator;
    
    public CrowdController crowd;

    public float attackEnergy = 100.0f;

    public double healthPoints = 100.0;

    public Material blueTeam;
    public Material redTeam;

    public float jumpForce = 300f;

    public float rayDistance = 0.3f;
    public LayerMask ground;

    public bool isHitting;


    public bool isGrounded;

    public LayerMask playerLayer;
    public LayerMask hordeLayer;



    public Transform test;
                Vector3 movement = Vector3.zero;

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
        int blueLayer = LayerMask.NameToLayer("blue");
        int objectLayer = LayerMask.NameToLayer("object");

       

        hips = GetComponent<Rigidbody>();
        animator.SetBool("isWalk", false);

    }



    void Update()
    {
        if(healthPoints  <= 0){
            Die();
            
        }   
    }

    void FixedUpdate()
    {
        if(healthPoints  <= 0)return;
        CheckGround();
        if (!isGrounded){
            return;
        }

        
        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
        }
        MoveToTarget();
    }

    public void Die()
    {
        
        foreach (Transform t in hips.transform.parent.GetComponentsInChildren<Transform>())
        {
            t.gameObject.layer = 7;
            

        }
        foreach (ConfigurableJoint t in hips.transform.parent.GetComponentsInChildren<ConfigurableJoint>())
        {
            JointDrive x = t.angularXDrive;
            JointDrive yz = t.angularYZDrive;

            x.positionSpring = 10f;
            x.positionDamper = 0f;
            x.maximumForce = float.MaxValue;

            yz.positionSpring = 10f;
            yz.positionDamper = 0f;
            yz.maximumForce = float.MaxValue;

            t.angularXDrive = x;
            t.angularYZDrive = yz;
            
        }

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



    public void setTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }



    public void loadTexture(int playerN)
    {
        Color teamColor = (playerN == 2)
            ? new Color32(120, 200, 255, 255)
            : new Color32(230, 60, 60, 255);

        int teamLayer = LayerMask.NameToLayer(playerN == 1 ? "blue" : "red");
        gameObject.tag = playerN == 1 ? "blue" : "red";

        foreach (Transform t in hips.transform.parent.GetComponentsInChildren<Transform>())
        {
            t.gameObject.layer = teamLayer;

             
        }


        Renderer r = test.GetComponent<Renderer>();
        
        Material[] mats = r.materials;

            for (int i = 0; i < mats.Length; i++)
            {
            if (mats[i].name.Contains("Material.003"))
            {
                mats[i] .color = teamColor;
            }
        }

        r.materials = mats;
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(
            transform.position,
            transform.position + Vector3.down * rayDistance
        );
    }


private Quaternion lastTargetRotation = Quaternion.identity;

private void MoveToTarget()
{
    if(isHitting)
    {
        animator.SetBool("isWalk", false);
        return;
    }
    ConfigurableJoint hipsJoint = hips.GetComponent<ConfigurableJoint>();
    Vector3 toTarget = targetPosition - hips.position;
    float distance = toTarget.magnitude;
    float stopDistance = 0.3f;
    float slowDownRadius = 6f;
    Vector3 direction = toTarget.normalized;
    
    if (distance <= stopDistance)
    {
        hips.linearVelocity = Vector3.zero;
        hips.angularVelocity = Vector3.zero;

        
        JointDrive noRotation = new JointDrive();
        noRotation.positionSpring = 0;
        noRotation.positionDamper = 0;
        noRotation.maximumForce = 0;
        
        hipsJoint.angularXDrive = noRotation;
        hipsJoint.angularYZDrive = noRotation;
        
        foreach (Rigidbody r in hips.transform.parent.GetComponentsInChildren<Rigidbody>())
        {
            r.Sleep();
        }
        return;
    }
        
        float proportional = speed * Mathf.Clamp01(distance / slowDownRadius);
        float damping = 8f;

        Vector3 desiredVelocity = direction * proportional;
        Vector3 steering = desiredVelocity - hips.linearVelocity;

        hips.AddForce(steering * damping, ForceMode.Acceleration);


        float maxSpeed = 4f;
        if (!isHitting && hips.linearVelocity.magnitude > maxSpeed)
            {
                hips.linearVelocity = hips.linearVelocity.normalized * maxSpeed;

            }
        
        animator.SetBool("isWalk", hips.linearVelocity.magnitude > 1f);
    }

    private void Jump() {
        if (isGrounded) {
            hips.AddForce(new Vector3(0, jumpForce, 0));
            isGrounded = false;
        }
    }

    public void hit()
    {
        if(healthPoints <= 0)return;
        movement = crowd.GetMovement();
        movement.Normalize();
        isHitting = true;
        hips.AddForce(movement * attackEnergy , ForceMode.Impulse);
        
        StartCoroutine(EndHit());
    }

    private IEnumerator EndHit()
    {
    yield return new WaitForSeconds(0.1f);
    isHitting = false;
    }

    public void grab(){
        
        animator.SetBool("isWalk", true);
        animator.SetBool("isPush", true);
        StartCoroutine(EndPush());
    }

    private IEnumerator EndPush()
    {
        yield return new WaitForSeconds(5f);
        animator.SetBool("isPush", false);
       
    }

}
