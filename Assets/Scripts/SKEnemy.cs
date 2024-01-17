using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SKEnemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject playerObj;
    private Transform playerPos;
    private Move playerStats;
    public float health = 200f;
    private float moveSpeed;
    private float frequency;
    private bool canMove = true;
    private Animator sharkAnim;
    private AudioSource stepSound;
    [SerializeField] private GameObject EnemyDeathEffect;
    [SerializeField] private PulseObj pulseObj;
    [SerializeField] private GameObject xpDrop;
    private LevelManager levelManager;

    private bool isMoving = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerObj = GameObject.Find("Player");
        playerPos = GameObject.Find("Aimmer").transform;
        //playerPos = playerObj.transform;
        playerStats = playerObj.GetComponent<Move>();
        sharkAnim = GetComponent<Animator>();
        pulseObj = GetComponent<PulseObj>();
        stepSound = GetComponent<AudioSource>();
        xpDrop = GameObject.Find("Particle System");
        EnemyDeathEffect = GameObject.Find("EnemyDeathEffect");
         levelManager = GameObject.FindObjectOfType<LevelManager>();
        


        StartCoroutine(PeriodicMove());
    }

    void Update()
    {
Debug.Log(health);
    if (health <= 1f)
    {
        Death();
    }

    RotateToPlayer();
    }

    IEnumerator PeriodicMove()
    {
        while (true)
        {
            if (canMove && !isMoving)
            {
               
                isMoving = true;
                sharkAnim.SetTrigger("Jump");
                stepSound.Play();
                
                
                Vector3 directionToPlayer = (playerPos.position - transform.position).normalized;
                moveSpeed = Random.Range(4.00f, 6.59f);
                frequency = Random.Range(0.1f, 2.4f);
        
                float elapsedTime = 0f;
                while (elapsedTime < 0.5f)
                {
                    transform.position += directionToPlayer * moveSpeed * Time.deltaTime;
                    elapsedTime += Time.deltaTime;


                    yield return null;
                }

                isMoving = false;
            }
            
            yield return new WaitForSeconds(frequency);
            
            
        }
    }

    void RotateToPlayer()
    {
        Vector3 directionToPlayer = playerPos.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        targetRotation *= Quaternion.Euler(0f, 90f, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1.5f * Time.deltaTime);
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
    if(gameObject != null){
    GameObject instantiatedXpDrop = PrefabInstantiation.Instance.InstantiateMinXp(transform.position,Quaternion.Euler(-90f, 0f, 0f));
     GameObject instantiatedPrefabB = PrefabInstantiation.Instance.InstantiatePrefabB(transform.position,Quaternion.Euler(-90f, 0f, 0f));
    }else{
       levelManager.EnemyDefeated();
    Destroy(gameObject); 
    }
     levelManager.EnemyDefeated();
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