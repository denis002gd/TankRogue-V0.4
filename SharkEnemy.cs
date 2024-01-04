using System.Collections;
using UnityEngine;

public class SharkEnemy : MonoBehaviour
{
    [Header("EnemyStats")]
    [SerializeField] public Transform target;
    [SerializeField] public float moveDistance = 1f;
    [SerializeField] public float moveDuration = 1f;
    [SerializeField] public float frequency = 0.5f;
    [SerializeField] public float rotationSpeed = 5f;
    [SerializeField] public float Health = 200f;

    [SerializeField] public float yRotation = 0f;
    [SerializeField] public PulseObj pulseObj;

    [SerializeField] public AudioSource MusicPlayer;
    [SerializeField] public AudioClip damage1;
    [SerializeField] public Move playerStats;
    [SerializeField] public GameObject xpDrop;
    [SerializeField] public GameObject explosionDF;

    private float initialY;
    private bool canMove = true; // Flag to control movement

    void Start()
    {
        initialY = transform.position.y;
        StartCoroutine(PeriodicMove());
        xpDrop = GameObject.Find("Particle System");
        target = GameObject.Find("Player").transform;
        pulseObj = GetComponent<PulseObj>();
        MusicPlayer = GetComponent<AudioSource>();
    }

    void Update()
    {
        RotateToPlayer();
        if (Health <= 0f)
        {
            Instantiate(xpDrop, transform.position, transform.rotation);
            Death();
        }
    }

    IEnumerator PeriodicMove()
    {
        while (true)
        {
            if (canMove)
            {
                Vector3 startPos = transform.position;
                Vector3 directionToTarget = (target.position - startPos).normalized;
                Vector3 endPos = startPos + directionToTarget * moveDistance;

                float elapsedTime = 0f;

                while (elapsedTime < moveDuration)
                {
                    float t = elapsedTime / moveDuration;
                    transform.position = Vector3.Lerp(startPos, endPos, t);
                    transform.position = new Vector3(transform.position.x, initialY, transform.position.z);
                    elapsedTime += Time.deltaTime;

                    // Check for collisions during the movement
                    CheckCollisions();

                    yield return null;
                }
            }

            // Wait for the specified frequency before starting the next move
            yield return new WaitForSeconds(frequency);
        }
    }

    void CheckCollisions()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f); // Adjust the radius based on your needs

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player") || collider.CompareTag("Enemy"))
            {
                // Handle the collision with the player or enemy
                // For example, stop movement or apply a force in the opposite direction
                canMove = false;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            // Re-enable movement when exiting the collision with player or enemy
            canMove = true;
        }
    }

    void RotateToPlayer()
    {
        if (target != null)
        {
            Vector3 directionToPlayer = target.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            targetRotation *= Quaternion.Euler(0f, yRotation, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            HitSFX();
            Health = Health - playerStats.playerDamage;
            SwitchMaterialRecursive(transform);
            Debug.Log(Health);
            pulseObj.SwitchMaterial();
        }
    }

    void SwitchMaterialRecursive(Transform objTransform)
    {
        PulseObj pulseObj = objTransform.GetComponent<PulseObj>();
        if (pulseObj != null)
        {
            pulseObj.SwitchMaterial();
        }

        foreach (Transform child in objTransform)
        {
            SwitchMaterialRecursive(child);
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }

    void HitSFX()
    {
        MusicPlayer.clip = damage1;
        MusicPlayer.Play();
    }
}
