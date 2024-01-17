using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TuretEnemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject playerObj;
    private Transform playerPos;
    private Move playerStats;
    public float health = 300f;
    private Animator shootingEnemy;
    private AudioSource audioSou;
    private float rotateAmmount = 90f;
    private bool isFollowingPlayer = true;
    public Transform shootPoint;
    public GameObject BulletPrefab;
    private float followRange = 1000f;
    private float shootingRange = 17f;
    private float stoppingDistance = 10f;
    private float nextDamageTime;
    private bool canShoot = true;
    private float shootCooldown = 5f;
    
    
    [SerializeField] private GameObject EnemyDeathEffect;
    [SerializeField] private PulseObj pulseObj;
    [SerializeField] private GameObject xpDrop;
    private LevelManager levelManager;
    private bool isDead = false;

    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerObj = GameObject.Find("Player");
        playerPos = GameObject.Find("Aimmer").transform;
        playerStats = playerObj.GetComponent<Move>();
        shootingEnemy = GetComponent<Animator>();
        pulseObj = GetComponent<PulseObj>();
        audioSou = GetComponent<AudioSource>();
        xpDrop = GameObject.Find("Particle System");
        EnemyDeathEffect = GameObject.Find("EnemyDeathEffect");
         levelManager = GameObject.FindObjectOfType<LevelManager>();
        
        


        StartCoroutine(FollowPlayer());
    }

    void Update()
{
    if (isDead)
    {
        return;
    }

    if (health <= 1f)
    {
        Death();
    }

    RotateToPlayer();
    float distanceToPlayer = Vector3.Distance(transform.position, playerPos.position);

    if (distanceToPlayer <= followRange)
    {
        // Calculate direction to the player
        Vector3 directionToPlayer = playerPos.position - transform.position;

        // Check if the distance to the player is greater than the stopping distance
        if (distanceToPlayer > stoppingDistance)
        {
            // Move towards the player
            transform.Translate(Vector3.forward * Time.deltaTime);
        }

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

IEnumerator FollowPlayer()
{
    float refresh = 0.2f;

    while (isFollowingPlayer && playerPos != null)
    {
        if (gameObject == null)
        {
            yield break;
        }

        agent.SetDestination(playerPos.position);
        yield return new WaitForSeconds(refresh);
    }
}

void RotateToPlayer()
{
    Vector3 directionToPlayer = playerPos.position - transform.position;
    Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
    targetRotation *= Quaternion.Euler(0f, rotateAmmount, 0f);
    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
}

private void OnCollisionEnter(Collision other)
{
    if (other.gameObject.CompareTag("Bullet"))
    {
        health -= playerStats.playerDamage;
        SwitchMaterialRecursive(transform);
    }
}

void ResetShootCooldown()
{
    canShoot = true;
}

void Death()
{
    isFollowingPlayer = false;

    GameObject instantiatedMinXp = PrefabInstantiation.Instance.InstantiateMinXp(transform.position, Quaternion.Euler(-90f, 0f, 0f));
    GameObject instantiatedPrefabB = PrefabInstantiation.Instance.InstantiatePrefabB(transform.position, Quaternion.Euler(-90f, 0f, 0f));

    isDead = true;
    levelManager.EnemyDefeated();

    agent.isStopped = true;

    Destroy(gameObject);
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

void Shoot()
{
    // Instantiate the bullet prefab and get the shooting point from the prefab script
    GameObject bullet = PrefabInstantiation.Instance.InstantiateEnemyBullet(shootPoint.position, shootPoint.rotation);

    bullet.GetComponent<Rigidbody>().velocity = shootPoint.forward * 5f;
}
}