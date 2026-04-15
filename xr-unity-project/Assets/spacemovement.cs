using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpaceMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveForce = 8f;
    public float maxSpeed = 6f;

    [Header("Float Feel")]
    public float gravityScale = 0.08f; 
    public float linearDrag = 0.6f; 

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;            
        rb.linearDamping = linearDrag;
        rb.angularDamping = 10f; 
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        // Input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 inputDir = new Vector3(h, 0, v);

  
        if (inputDir.sqrMagnitude > 0.01f)
            rb.AddForce(inputDir.normalized * moveForce);

        rb.AddForce(Vector3.down * gravityScale, ForceMode.Acceleration);

        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
    }
}