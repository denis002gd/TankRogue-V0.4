using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DrillEnemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject playerObj;
    private Transform playerPos;
    private Move playerStats;
    public float health = 600f;
    private Animator drillEnemy;
    private AudioSource audioSou;
    private float rotateAmmount = 260f;
    private bool isFollowingPlayer = true;
    
    [SerializeField] private GameObject EnemyDeathEffect;
    [SerializeField] private PulseObj pulseObj;
    [SerializeField] private GameObject xpDrop;
    private LevelManager levelManager;
    private bool isDead = false;

    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerObj = GameObject.Find("Player");
        playerPos = playerObj.transform;
        playerStats = playerObj.GetComponent<Move>();
        drillEnemy = GetComponent<Animator>();
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

void Death()
{
    
    isFollowingPlayer = false;

    GameObject instantiatedMinXp = PrefabInstantiation.Instance.InstantiateMinXp(transform.position, Quaternion.Euler(-90f, 0f, 0f));
    GameObject instantiatedPrefabB = PrefabInstantiation.Instance.InstantiatePrefabB(transform.position, Quaternion.Euler(-90f, 0f, 0f));

    isDead = true;  // Set isDead to true
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
}
