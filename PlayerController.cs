using UnityEngine;
using Vector3 = UnityEngine.Vector3;



public class PLayerController : MonoBehaviour
{
    private Vector3 targetPosition;
    
    public Rigidbody hips;
    
    public float speed = 50f;
    

    public float jumpForce = 300f;

    public float rayDistance = 0.3f;
    public LayerMask ground;

    public bool isGrounded;


    private void CheckGround()
    {
        isGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            rayDistance,
            ground
        );
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(
            transform.position,
            transform.position + Vector3.down * rayDistance
        );
    }

    public void setTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    void Start()
    {
        hips = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        CheckGround();
        if (!isGrounded)
            return;

        if (Input.GetKey(KeyCode.Space)) {
            Jump();
        }

        MoveToTarget();
    }

    private void MoveToTarget()
    {

        Vector3 toTarget = targetPosition - hips.position;
        float distance = toTarget.magnitude;

        float stopDistance = 0.3f;
        float slowDownRadius = 6f;

        if (distance <= stopDistance)
        {
            hips.linearVelocity = Vector3.zero;
            hips.angularVelocity = Vector3.zero;
            return;
        }

        Vector3 direction = toTarget.normalized;
        float proportional = speed * Mathf.Clamp01(distance / slowDownRadius);
        float damping = 8f;

        Vector3 desiredVelocity = direction * proportional;
        Vector3 steering = desiredVelocity - hips.linearVelocity;

        hips.AddForce(steering * damping, ForceMode.Acceleration);

        float maxSpeed = 4f;
        if (hips.linearVelocity.magnitude > maxSpeed)
            hips.linearVelocity = hips.linearVelocity.normalized * maxSpeed;
    }

    private void Jump() {
        if (isGrounded) {
            hips.AddForce(Vector3.up * jumpForce);
        }
    }
}
