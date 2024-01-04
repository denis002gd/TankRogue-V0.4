using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : MonoBehaviour
{
    public float maxHealth = 100.0f;
    private GameObject player;
    public Image healthBar;
    public float knockbackForce = 10.0f;
    public float knockbackDuration = 1.0f;
    private float damage;
    private Renderer objectRender;
    Color lerpedColor = Color.white;
    private GameObject xpDrop;
    private Transform playerPos;
    private float currentHealth;
    private FollowPlayer enemyFollow;
    public float bulletSpeed = 20;
    private bool hit = false;
    private float damageCooldown = 0.2f; 
    public GameObject levelMan;
    
private float nextDamageTime;

     void LateUpdate() {
        GameObject playerTank = GameObject.Find("Player");
           if (playerTank != null)
    {   
        
        Move moveComponent = playerTank.GetComponent<Move>();
        damage = moveComponent.playerDamage;
    }
    }
    void Start()
    {
        
        currentHealth = maxHealth;
        enemyFollow = GetComponent<FollowPlayer>();
        objectRender = GetComponent<Renderer>();
        xpDrop = GameObject.Find("Particle System");
        playerPos = GameObject.Find("Player").transform;
        levelMan = GameObject.Find("LevelManager");
        
    }

    void Update()
    {  
        
          if (hit)
        {
            PulseColor();
            hit = false; // Reset hit flag
        }
        transform.LookAt(playerPos);
       
    }

   
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Bullet")){
            
            hit = true;
              StartCoroutine(PulseColor());
              if(Time.time >= nextDamageTime){
            currentHealth = currentHealth - damage;
             nextDamageTime = Time.time + damageCooldown;
            if(currentHealth <= 0f){
                Instantiate(xpDrop, transform.position, Quaternion.identity);
              
                    Die();

            }}
        }
    }

     void Die()
    {
        Destroy(gameObject);
    }
    private System.Collections.IEnumerator PulseColor(){
               // Change the color to red
        objectRender.material.color = Color.red;

        // Smoothly transition back to the original color over time
        float elapsedTime = 0f;
        float duration = 0.3f; // Adjust the duration as needed

        while (elapsedTime < duration)
        {
            // Interpolate between red and original color
            objectRender.material.color = Color.Lerp(Color.red, Color.black, elapsedTime / duration);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame
        }

        // Ensure the final color is the original color
        objectRender.material.color = Color.black;
    }

}

