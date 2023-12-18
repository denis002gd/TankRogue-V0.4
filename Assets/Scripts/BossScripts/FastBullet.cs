using UnityEngine;

public class FastBullet : MonoBehaviour
{
    public float bulletSpeed = 50f;  // Speed of the bullet
    public float bulletLifetime = 10f;  // Time before the bullet is destroyed

    void Start()
    {
        // Get the forward direction of the gun
        Vector3 bulletDirection = -transform.right;

         Quaternion rotationOffset = Quaternion.Euler(0, -10f, 0);
        bulletDirection = rotationOffset * bulletDirection;

        // Set initial velocity to move the bullet in the adjusted direction
        GetComponent<Rigidbody>().velocity = bulletDirection * bulletSpeed;

        // Destroy the bullet after the specified lifetime
        Destroy(gameObject, bulletLifetime);
    }
    void OnTriggerEnter(Collider other){
        if(other.CompareTag("player")){
            Destroy(gameObject);
        }
    }

   
}