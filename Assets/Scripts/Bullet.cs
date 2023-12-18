using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletLife = 5f;
    
    public GameObject Particle;
    public float bulletSpeed = 20f;
    public float reflectSpeed = 10f;
    private AudioSource audioSource;
    public AudioClip shotSound;
    GameObject Player;
    Move PLScript;
    Vector3 playerDirection;
    GameObject barrelDir;
    private Vector3 initialVelocity;
    private bool canAttack = false;
    public Material enemyCol;
    public TrailRenderer trailRenderer;
    
   

    void Start()
    {   
        if (Particle != null)
        {
            Particle.SetActive(false);
            
           
        }
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = shotSound;
        audioSource.Play();
        Player = GameObject.Find("Player");
        PLScript = Player.GetComponent<Move>();
        gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
        barrelDir = GameObject.Find("TankBarell");
        playerDirection = -transform.right;

         Rigidbody bulletRigidbody = gameObject.GetComponent<Rigidbody>();
        initialVelocity = playerDirection * bulletSpeed;
        bulletRigidbody.velocity = initialVelocity;
    }

    void Update()
    {     
        Destroy(gameObject, bulletLife); // Destroy the bullet after a certain time
         
        
         ScaleObject(PLScript.playerDamage);
         
    }

private void OnCollisionEnter(Collision coll)
{
    if (coll.collider.CompareTag("Enemy"))
    {
        if (Particle != null)
        {
            // Activate the particle object if it exists
            Particle.SetActive(true);
        }


        // Destroy the bullet on collision with an enemy
        Destroy(gameObject);
    }
    if(coll.collider.CompareTag("player") && canAttack){
        Destroy(gameObject);
    }
       else if (coll.collider.CompareTag("bounceArea"))
        {
            Rigidbody bulletRigidbody = gameObject.GetComponent<Rigidbody>();

            // Calculate the reflection direction based on the velocity
            Vector3 reflectionDirection = Vector3.Reflect(initialVelocity.normalized, coll.contacts[0].normal);

            // Set the new direction for the bullet
            bulletRigidbody.velocity = reflectionDirection * reflectSpeed;
            gameObject.tag = "Enemy bullet";
            canAttack = true;
            GetComponent<Renderer>().material = enemyCol;
            trailRenderer.enabled = false;
            
        }
}

    void ScaleObject(float damage)
    {
       
        float scaleFactor = PLScript.scaleOfBullet + damage * 0.01f;

         
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }
}
