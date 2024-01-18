using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletLife = 5f;
    
    #pragma warning disable 0649
    [SerializeField] private GameObject Particle;  // Serialized variable
    #pragma warning restore 0649
    public float reflectSpeed = 10f;
    GameObject Player;
    Move PLScript;
    Vector3 playerDirection;
    GameObject barrelDir;
    private Vector3 initialVelocity;
    private bool canAttack = false;
    public Material enemyCol;

    
    
   

    void Start()
    {   
        if (Particle != null)
        {
            Particle.SetActive(true);
           
            
           
        }

        Player = GameObject.Find("Player");
        PLScript = Player.GetComponent<Move>();
        gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
        barrelDir = GameObject.Find("TankBarell");
        playerDirection = -transform.right;

         Rigidbody bulletRigidbody = gameObject.GetComponent<Rigidbody>();
        initialVelocity = playerDirection * PLScript.BulletSpeed;
        bulletRigidbody.velocity = initialVelocity;
    }

    void Update()
    {     
        Destroy(gameObject, bulletLife); // Destroy the bullet after a certain time
         
        
         ScaleObject(PLScript.playerDamage);
         
    }

private void OnCollisionEnter(Collision coll)
{
    if (coll.collider.CompareTag("Enemy") || coll.collider.CompareTag("Wall") || coll.collider.CompareTag("UniqueEnemy"))
    {
        if (Particle != null)
        {
            // Activate the particle object if it exists
            Particle.SetActive(true);
        }

        // Destroy the bullet on collision with an enemy
        Destroy(gameObject);
    }
    else if (coll.collider.CompareTag("player") && canAttack)
    {
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

        // Check if Particle is assigned before using it
        if (Particle != null)
        {
            Particle.SetActive(false);
        }
    }
}

    void ScaleObject(float damage)
    {
       
        float scaleFactor = PLScript.scaleOfBullet + damage * 0.01f;

         
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }
}
