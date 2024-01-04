using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Transform target;
    public LayerMask obstacleLayer;
    public float moveSpeed = 5.0f;
    public float avoidanceRadius = 2.0f;
    public float knockbackForce = 10.0f;
    public float knockbackDuration = 1.0f;

    private Rigidbody rb;
    private bool isKnockedBack;
    private Vector3 knockbackDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("player");
        target = player.transform;
    }

    void Update()
    {
        if (!isKnockedBack)
        {
            if (player != null)
            {
                Vector3 desiredDirection = (target.position - transform.position).normalized;
                Vector3 avoidanceDirection = Vector3.zero;

                // Check for nearby obstacles and calculate avoidance force.
                Collider[] obstacles = Physics.OverlapSphere(transform.position, avoidanceRadius, obstacleLayer);
                foreach (var obstacle in obstacles)
                {
                    Vector3 obstacleDirection = (obstacle.transform.position - transform.position).normalized;
                    avoidanceDirection += -obstacleDirection;
                }

                Vector3 steeringDirection = desiredDirection + avoidanceDirection;

                // Apply the steering direction to move the enemy.
                rb.velocity = steeringDirection * moveSpeed;
            }
        }
    }

    public void Knockback(Vector3 direction)
    {
        if (!isKnockedBack)
        {
            isKnockedBack = true;
            knockbackDirection = direction;
            rb.velocity = knockbackDirection * knockbackForce;
            Invoke("EndKnockback", knockbackDuration);
        }
    }

    void EndKnockback()
    {
        isKnockedBack = false;
    }
}

