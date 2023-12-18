using UnityEngine;

public class BossFastBullet : MonoBehaviour
{
    public float bulletLife = 665f;
    public float bulletSpeed = 25f;
    private Rigidbody bulletRigidbody;

    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletRigidbody.velocity = transform.forward * bulletSpeed;

        // Destroy the bullet after a certain time
        Destroy(gameObject, bulletLife);
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("Player"))
        {
            // Destroy the bullet on collision with an enemy
            Destroy(gameObject);
        }
    }
}
