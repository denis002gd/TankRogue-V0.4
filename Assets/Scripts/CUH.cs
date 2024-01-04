using UnityEngine;

public class CUH : MonoBehaviour
{
    public float jumpForce = 5.0f;
    public float gravity = -9.8f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Check if the object is grounded.
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f, groundLayer);

        if (isGrounded)
        {
            // Reset velocity when grounded to ensure consistent jumps.
            rb.velocity = Vector3.zero;
        }

        // Handle input (e.g., mouse click or touch).
        if (Input.GetMouseButtonDown(0))
        {
            if (isGrounded)
            {
                // Jump when grounded.
                Jump();
            }
        }
    }

    void FixedUpdate()
    {
        // Apply gravity to the object when it's not grounded.
        if (!isGrounded)
        {
            rb.AddForce(Vector3.up * gravity, ForceMode.Force);
        }
    }

    void Jump()
    {
        // Apply an upward force to make the object jump.
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        Debug.Log("cuh");
    }
}
