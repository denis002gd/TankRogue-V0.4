using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Move: MonoBehaviour
{
    public float rotateSpeed = 90;
    public float scaleOfBullet = 0.01f;
    public float speed = 5.0f;
    public float fireInterval = 0.5f;
    public Transform spawnPoint;
    public GameObject bulletObject;
    public float kickpower = 10f;
    private Rigidbody playerRB;
    public int experience;
    public static Move Instance;
    public int playerHealth;
    private bool isInvincible = false;
    private float invincibilityDuration = 1f;
    private float invincibilityTimer = 0f;
    public bool canShoot = true;
    public bool canShootRL = true;
    AudioSource audioSF;
    public AudioClip hitSF;
    private AudioSource audioSource;
    public AudioClip shotSound;
    public int Ammo = 1;
    public int MaxAmmo = 100;
    public float ReloadSpeed = 2.0f;
    public TextMeshProUGUI AmmoCount;
    
     
    public float playerDamage = 20f;
    
    
    float nextFire;
  void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = shotSound;
        nextFire = Time.time + fireInterval;
        playerRB = GetComponent<Rigidbody>();
        audioSF = GetComponent<AudioSource>();
        
        
        
    }

    void Update()
    {
        var transAmount = speed * Time.deltaTime;
        var rotateAmount = rotateSpeed * Time.deltaTime;
        Vector3 pulse = Vector3.back * kickpower;
        AmmoCount.text = Ammo.ToString();

        if (Input.GetKey("w"))
        {
            transform.Translate(0, 0, transAmount);
        }
        if (Input.GetKey("s"))
        {
            transform.Translate(0, 0, -transAmount);
        }
        if (Input.GetKey("a") && transAmount > 0)
        {
            transform.Rotate(0, -rotateAmount, 0);
        }
        if (Input.GetKey("d") && transAmount > 0)
        {
            transform.Rotate(0, rotateAmount, 0);
        }
        if (Input.GetKey("r") && transAmount > 0)
        {
            Reloading();
        }
        if (Input.GetKey("0") && transAmount > 0)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;

        // Reload the current scene
        SceneManager.LoadScene(currentSceneName);
        }
        if (Input.GetButton("Fire1") && Time.time > nextFire && canShoot && canShootRL)
        { 
            if(Ammo > 0){
                nextFire = Time.time + fireInterval;
            fire();
            
            playerRB.AddRelativeForce(pulse,ForceMode.Impulse);
            Ammo--;
            
            }else{
                Debug.Log("Reloading");
                StartCoroutine(Reloading());
                  nextFire = Time.time + ReloadSpeed;
                  canShootRL = false;
            }
            
         
         
        }
        if(playerHealth <= 0){
           
            gameObject.SetActive(false);
        }
        if (isInvincible)
    {
        invincibilityTimer -= Time.deltaTime;

        // Check if the invincibility period has ended
        if (invincibilityTimer <= 0f)
        {
            isInvincible = false;
        }
    }
    }

    void fire()
    {
        var bullet = Instantiate(bulletObject, spawnPoint.position, spawnPoint.rotation);
        audioSource.Play();
      
         
       
    }
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Enemy bullet")){
             playerHealth--;
                audioSF.clip = hitSF;
                audioSF.Play();
                 SwitchMaterialRecursive(transform);

                // Set the player to be invincible and start the invincibility timer
                isInvincible = true;
                invincibilityTimer = invincibilityDuration;

                // Apply force to the player in the direction opposite to the collision
                
        }
        if(other.CompareTag("XP")){
            experience++; 
        }
        
    }
    private void OnCollisionEnter(Collision other) {
        float pushForce = 150f;
        if (!isInvincible && other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Enemy bullet"))
        {
            // Check if playerHealth is greater than zero before decreasing it
            if (playerHealth > 0)
            {
                playerHealth--;
                audioSF.clip = hitSF;
                audioSF.Play();
                 SwitchMaterialRecursive(transform);

                // Set the player to be invincible and start the invincibility timer
                isInvincible = true;
                invincibilityTimer = invincibilityDuration;

                // Apply force to the player in the direction opposite to the collision
                Vector3 pushDirection = transform.position - other.contacts[0].point;
                playerRB.AddForce(pushDirection.normalized * pushForce, ForceMode.Impulse);
              
            }
        }
    
    }
     public Vector3 GetFacingDirection()
    {
        return transform.forward;
    }

    public IEnumerator Reloading(){
        
      
          yield return new WaitForSeconds(ReloadSpeed);
          Ammo = MaxAmmo;
          canShootRL = true;
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
    

}