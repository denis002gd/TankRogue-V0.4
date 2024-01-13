using System.Collections;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class HealTower : MonoBehaviour
{
    private NavMeshAgent agent;
    private Rigidbody rb;
    private GameObject playerObj;
    private Transform playerPos;
    private Move playerStats;
    private float health = 3f;
    private Animator drillEnemy;
    private AudioSource audioSou;
    private float rotateAmount = 260f;
    private bool isFollowingPlayer = true;
    private float healRadius = 5f;
    private float healAmount = 20f;
    private float healCooldown = 3f;
    private LevelManager levelManager;
    private bool isDead = false;

    [SerializeField] private GameObject pulseHeal;
    [SerializeField] private GameObject enemyDeathEffectPrefab;
    [SerializeField] private PulseObj pulseObjPrefab;
    [SerializeField] private GameObject xpDropPrefab;
    [SerializeField] private GameObject healOrbPrefab;

    private Vector3 knockbackDirection;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>(); // Added Rigidbody component
        playerObj = GameObject.Find("Player");

        if (playerObj != null)
        {
            playerPos = playerObj.transform;
            playerStats = playerObj.GetComponent<Move>();
        }
        else
        {
            Debug.LogError("Player object not found!");
            isFollowingPlayer = false;
        }

        drillEnemy = GetComponent<Animator>();
        audioSou = GetComponent<AudioSource>();
        pulseHeal = GameObject.Find("pulseHeal");
        levelManager = FindObjectOfType<LevelManager>();

        StartCoroutine(PeriodicHeal());
        StartCoroutine(FollowClosestEnemy());
    }

     void Update()
    {
        if (isDead)
            return;

        if (health <= 1f)
        {
            Death();
        }

       
    }

     void RotateToEnemy(Vector3 enemyPosition)
    {
        Vector3 directionToEnemy = enemyPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);

        // Extract only the x and z rotations from the targetRotation
        targetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
    }

    IEnumerator FollowClosestEnemy()
    {
        float refresh = 0.1f;

        while (isFollowingPlayer && playerPos != null)
        {
            if (gameObject == null)
            {
                yield break;
            }


            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            if (enemies.Length > 0)
            {
                enemies = enemies.OrderBy(enemy => Vector3.Distance(transform.position, enemy.transform.position)).ToArray();
                GameObject closestEnemy = enemies[0];
                Vector3 targetPosition = new Vector3(closestEnemy.transform.position.x, transform.position.y, closestEnemy.transform.position.z);
                transform.LookAt(targetPosition);
                agent.SetDestination(closestEnemy.transform.position); 
                RotateToEnemy(closestEnemy.transform.position);
                yield return StartCoroutine(FollowEnemyUntilDead(closestEnemy));
            }
            else
            {
                
                Vector3 targetPosition = new Vector3(playerPos.position.x, transform.position.y, playerPos.position.z);
                transform.LookAt(targetPosition);
                agent.SetDestination(playerPos.position);
            }

            yield return new WaitForSeconds(refresh);
        }
    }

    IEnumerator FollowEnemyUntilDead(GameObject enemy)
    {
        while (enemy != null)
        {
            agent.SetDestination(enemy.transform.position);
            RotateToEnemy(enemy.transform.position);

            yield return null;
        }

        // Enemy is dead or no longer exists, switch back to following the player
        StartCoroutine(FollowClosestEnemy());
    }




    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            
            knockbackDirection = (transform.position - other.transform.position).normalized;

           
            agent.isStopped = true;

            
            rb.AddForce(knockbackDirection * 10f, ForceMode.Impulse);


            health--;
            SwitchMaterialRecursive(transform);
        }
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
        PulseObj pulseObject = objTransform.GetComponent<PulseObj>();
        if (pulseObject != null)
        {
            pulseObject.SwitchMaterial();
        }

        foreach (Transform child in objTransform)
        {
            SwitchMaterialRecursive(child);
        }
    }

    IEnumerator PeriodicHeal()
    {
        while (true)
        {
            yield return new WaitForSeconds(healCooldown);
            HealNearbyEnemies();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, healRadius);
    }

    void HealNearbyEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, healRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy") && collider.gameObject != gameObject)
            {
                SKEnemy skScript = collider.GetComponent<SKEnemy>();
                DrillEnemy deScript = collider.GetComponent<DrillEnemy>();
                TuretEnemy teScript = collider.GetComponent<TuretEnemy>();

                if (skScript != null)
                {
                    StartCoroutine(BlinkAndRestoreColor(skScript.gameObject));
                    skScript.health += healAmount;
                }
                if (deScript != null)
                {
                    StartCoroutine(BlinkAndRestoreColor(deScript.gameObject));
                    deScript.health += healAmount;
                }
                if (teScript != null)
                {
                    StartCoroutine(BlinkAndRestoreColor(teScript.gameObject));
                    teScript.health += healAmount;
                }
            }
        }
    }

    IEnumerator BlinkAndRestoreColor(GameObject obj)
    {
        pulseHeal.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        GameObject orb = Instantiate(healOrbPrefab, obj.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2f);
        pulseHeal.SetActive(false);
        Destroy(orb);
    }
}

