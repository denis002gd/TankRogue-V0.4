using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class TankBotScript : MonoBehaviour
{

    [Header("RefferenceScripts")]
    [SerializeField] public Move playerScr;
    [SerializeField] public PulseObj pulseScr;
    [Header("Transforms")]
    [SerializeField] public Transform firePoint;
    [SerializeField] public Transform player;
    [SerializeField] public Transform TankObject;
    [SerializeField] public Transform explosionPos;
    [SerializeField] public GameObject Slider;
    private UnityEngine.AI.NavMeshAgent TankBot;
    private Vector3 fixPosition;
    [Header("Stats")]
    private float MinShootTime = 5f;
    private float MaxShootTime = 10f;
    private float nextShootTime;
    [SerializeField] public int numberOfBullets = 3;
    [SerializeField] public float shootInterval = 1f; 
    [SerializeField] public float BossHealth = 2000f;
    [Header("Effects&Audio")]
    private Animator TBAnimations;
    [SerializeField] public Animator SliderAnimation;
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public AudioClip shootSF;
    [SerializeField] public AudioClip damageSF;
    [SerializeField] public AudioSource scr;  
    [SerializeField] public Slider HealthBar;
    [SerializeField] public GameObject explosionDF;
    [SerializeField] private VisualEffect mozzeEffect;

    void Start()
    {
        TankBot = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("player").transform;
        TBAnimations = GetComponent<Animator>();

        StartCoroutine(UpdatePath());
        HealthBar.maxValue = BossHealth;
        nextShootTime = Time.time;
        Slider.SetActive(true);
      
    }

    void Update()
    {
        TankObject.LookAt(2 * TankObject.position - player.position,Vector3.up);
        fixPosition = new Vector3(TankObject.position.x, 1.68f, TankObject.position.z);
        transform.position = fixPosition;
        if (Time.time >= nextShootTime)
        {
            StartCoroutine(ShootWithDelay());
            nextShootTime = Time.time + Random.Range(MinShootTime, MaxShootTime);
        }
        HealthBar.value = BossHealth;
        if(BossHealth <= 0){
            explosionPos.position = transform.position;
            explosionDF.SetActive(true);
            SliderAnimation.SetBool("YepHeDead", true);
            Invoke("UnievitableDeath", 0.1f);
        }
    }

    IEnumerator ShootWithDelay()
    {
    TBAnimations.SetTrigger("ShootTR");
    yield return new WaitForSeconds(0.7f);
    yield return StartCoroutine(ShootThreeBullets());
    }

    IEnumerator UpdatePath()
    {
        float refresh = 0.2f;

        while (player != null)
        {
            TankBot.SetDestination(player.position);
            yield return new WaitForSeconds(refresh);
        }
    }

     void OnCollisionEnter(Collision collider)
    {   
    SwitchMaterialRecursive(transform);

         pulseScr.SwitchMaterial();
         DamagaSFX();
        BossHealth -= playerScr.playerDamage;
    
       
    }
     IEnumerator ShootThreeBullets()
    {
    
        
        for (int i = 0; i < numberOfBullets; i++)
        {
            ShootBullet();
           ShootSFX();
           mozzeEffect.Play();
            yield return new WaitForSeconds(shootInterval);
        }

        TBAnimations.SetTrigger("WalkTR");
 
    }
    void ShootBullet()
    {
    
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

       
    }
    void SwitchMaterialRecursive(Transform objTransform)
    {
        // Switch material for the current object
        PulseObj pulseObj = objTransform.GetComponent<PulseObj>();
        if (pulseObj != null)
        {
            pulseObj.SwitchMaterial();
        }

        // Switch material for all child objects
        foreach (Transform child in objTransform)
        {
            SwitchMaterialRecursive(child);
        }
    }
    void DamagaSFX(){
        scr.clip = damageSF;
        scr.Play();
    }
    void ShootSFX(){
        scr.clip = shootSF;
        scr.Play();
    }
    void UnievitableDeath(){
        Destroy(gameObject);
        Destroy(HealthBar);
        
    }
}