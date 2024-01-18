using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootingenemy : MonoBehaviour
{   
    public GameObject player;
    public Transform playerPos;
    public Move playerMove;
    private float followRange = 1000f;
    private float shootingRange = 10f;
    public GameObject bulletPrefab;
    public Transform shootingPoint;
    private float shootCooldown = 5f;
    private float maxHealth = 80;
    private float stoppingDistance = 10f;
    private float playerDmg;
    private GameObject xpDrop;

    private float currentHealth;
    private bool canShoot = true;
    private float damageCooldown = 0.2f;
private float nextDamageTime;
    

    void Start()
    {
        currentHealth = maxHealth;
        xpDrop = GameObject.Find("Particle System");
        player = GameObject.Find("Player");
        playerPos = GameObject.Find("Player").transform;
        playerMove = player.GetComponent<Move>();
          Rigidbody rb = GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.constraints = RigidbodyConstraints.FreezePositionY;
    }
    }

    void Update()
    {
   
        float distanceToPlayer = Vector3.Distance(transform.position, playerPos.position);

        if (distanceToPlayer <= followRange)
        {
            // Calculate direction to the player
            Vector3 directionToPlayer = playerPos.position - transform.position;
            if (distanceToPlayer > stoppingDistance)
            {
                // Move towards the player
                
                transform.Translate(Vector3.forward * Time.deltaTime);
            }
            transform.LookAt(playerPos);

            if (distanceToPlayer <= shootingRange)
            {
                // Check if the enemy can shoot
                if (canShoot)
                {
                    Shoot();
                    canShoot = false;
                    Invoke("ResetShootCooldown", shootCooldown);
                }
            }
        }
       
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage();
        }
    }

    

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = shootingPoint.forward * 5f;
    }

    void TakeDamage()
{
    // Check if the cooldown period has elapsed
    if (Time.time >= nextDamageTime)
    {
        currentHealth -= playerMove.playerDamage;
        nextDamageTime = Time.time + damageCooldown;
        if (currentHealth <= 0)
        {
            if (xpDrop != null)
            {
                Instantiate(xpDrop, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}

    void ResetShootCooldown()
    {
        canShoot = true;
    }
    void EnemyBullet(){

    }

    void ShootAt(GameObject target)
    {
        // Calculate direction to the target
        Vector3 directionToTarget = (target.transform.position - shootingPoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.LookRotation(directionToTarget));
        bullet.GetComponent<Rigidbody>().velocity = directionToTarget * 10f;
    }
}
