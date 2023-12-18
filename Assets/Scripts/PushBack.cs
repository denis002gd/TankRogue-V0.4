using UnityEngine;

public class KnockbackObjects : MonoBehaviour
{
    [SerializeField] private float knockbackForce = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            // Calculate the direction from the bullet to the object.
            Vector3 knockbackDirection = (transform.position - other.transform.position).normalized;

            // Apply the knockback effect to move the object in the opposite direction.
            ApplyKnockback(knockbackDirection);
        }
    }

    private void ApplyKnockback(Vector3 direction)
    {
        // Adjust the position of the object in the opposite direction with a certain force.
        transform.position += direction * knockbackForce * Time.deltaTime;
    }
}
