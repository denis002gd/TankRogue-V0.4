using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Move : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float rotateSpeed = 90;
    [SerializeField] private Rigidbody playerRB;
    [SerializeField] public float speed = 5.0f;
    [SerializeField] private bool canMove = true;
    bool isWalking = false;
    bool isRotating = false;
    [SerializeField] public bool isInvincible = false;
    [SerializeField] public float invincibilityDuration = 1f;
    [SerializeField] public float invincibilityTimer = 0f;
    [SerializeField] public int playerHealth;
    [SerializeField] public float pushbackForce = 5f;
    [SerializeField] public float pushbackDuration = 0.5f;
    [SerializeField] public float resetDistance = -0.5f;
    [SerializeField] public bool canShoot = true;
    [SerializeField] public bool canShootRL = true;
    [SerializeField] public bool canBeHit;
    public static Move Instance;
    [Header("Weapon Stats")]
    [SerializeField] public float playerDamage = 20f;
    [SerializeField] public float scaleOfBullet = 0.01f;
    [SerializeField] public float fireInterval = 0.5f;
    [SerializeField] public Transform spawnPoint;
    [SerializeField] public GameObject bulletObject;
    [SerializeField] public int Ammo = 1;
    [SerializeField] public int MaxAmmo = 100;
    [SerializeField] public float ReloadSpeed = 2.0f;
    [SerializeField] public float BulletSpeed;
    float nextFire;
    [Header("Audio Elements")]
    [SerializeField] public AudioSource audioSF;
    [SerializeField] public AudioSource walkAudioSource;
    [SerializeField] public AudioSource rotateAudioSource;
    [SerializeField] public AudioClip hitSF;
    [SerializeField] public AudioClip recharge;
    [SerializeField] public AudioClip shotSound;
    [SerializeField] public AudioClip walkSound;
    [SerializeField] public AudioClip rotateSoundFX;
    [Header("Ui Elements")]
    [SerializeField] public int experience;
    [SerializeField] public TextMeshProUGUI AmmoCount;
    [SerializeField] public Slider HealthBar;
    [SerializeField] public LayerMask uiLayer;
    [SerializeField] public LayerMask overlayLayer;
    [SerializeField] public CameraShake CamShake;
    [SerializeField] public Transform aimmer;
    [SerializeField] public Transform pos1;
    [SerializeField] public Transform pos2;
    [SerializeField] public ParticleSystem MozzleFlash;
    [SerializeField] public ParticleSystem arrows;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        nextFire = Time.time + fireInterval;
        playerRB = GetComponent<Rigidbody>();

        // Initialize the AudioSources
        audioSF = gameObject.AddComponent<AudioSource>();
        walkAudioSource = gameObject.AddComponent<AudioSource>();
        rotateAudioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        var transAmount = speed * Time.deltaTime;
        var rotateAmount = rotateSpeed * Time.deltaTime;
        if (!canBeHit)
        {
            Debug.Log(canBeHit);
        }
      
        AmmoCount.text = Ammo.ToString();
        HealthBar.maxValue = 8;
        HealthBar.value = playerHealth;
        //this whole string checks player input
        if (canMove == true)
        {
            if (Input.GetKey("w"))
            {
                transform.Translate(0, 0, transAmount);
                aimmer.position = pos1.position;
                if (!isWalking)
                {
                    isWalking = true;
                    WalkForwardSound();
                }
            }
            else if (Input.GetKey("s"))
            {
                transform.Translate(0, 0, -transAmount);
                aimmer.position = pos2.position;
                if (!isWalking)
                {
                    isWalking = true;
                    WalkBackwardSound();
                }
            }
            else
            {
                aimmer.position = transform.position;
                if (isWalking)
                {
                    isWalking = false;
                    StopWalkSound();
                }
            }

            if (Input.GetKey("a") && transAmount > 0)
            {
                transform.Rotate(0, -rotateAmount, 0);
                if (!isRotating)
                {
                    isRotating = true;
                    RotateSound();
                }
            }
            else if (Input.GetKey("d") && transAmount > 0)
            {
                transform.Rotate(0, rotateAmount, 0);
                if (!isRotating)
                {
                    isRotating = true;
                    RotateSound();
                }
            }
            else
            {
                isRotating = false;
                StopRotateSound();

                if (Input.GetKey("w") || Input.GetKey("s"))
                {
                    if (!isWalking)
                    {
                        isWalking = true;
                        WalkForwardSound();
                    }
                }
                else
                {
                    isWalking = false;
                    StopWalkSound();
                }
            }

            if (Input.GetKey("r") && transAmount > 0)
            {
                Reloading();
            }
            if (Input.GetKey("0") && transAmount > 0)
            {
                string currentSceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentSceneName);
            }
            // shoot if the player presses the left click and the mouse is not on top of ui elements
            if (!IsPointerOverUI() && !IsPointerOverOverlay())
            {
                if (Input.GetButton("Fire1") && Time.time > nextFire && canShoot && canShootRL)
                {
                    if (Ammo > 0)
                    {
                        nextFire = Time.time + fireInterval;
                        fire();
                        Ammo--;
                    }
                    else
                    {
                        Debug.Log("Reloading");
                        StartCoroutine(Reloading());
                        nextFire = Time.time + ReloadSpeed;
                        canShootRL = false;
                    }
                }
            }

            if (playerHealth <= 0)
            {
                gameObject.SetActive(false);
            }

            if (isInvincible)
            {
                invincibilityTimer -= Time.deltaTime;
                Debug.Log("invincible");
                if (invincibilityTimer <= 0f)
                {
                    isInvincible = false;
                    
                }
            }
        }
    }

    public void fire()
    {
        var bullet = Instantiate(bulletObject, spawnPoint.position, spawnPoint.rotation);
        MozzleFlash.Play();
        Shoot();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy bullet"))
        {
            playerHealth--;
            Hit();
            SwitchMaterialRecursive(transform);
            isInvincible = true;
            CamShake.StartShake();
            invincibilityTimer = invincibilityDuration;
        }
        if (other.CompareTag("XP"))
        {
            experience++;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        float pushForce = 150f;
        if (!isInvincible && (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Enemy bullet")) && canBeHit)
        {
            if (playerHealth > 0)
            {
                CamShake.StartShake();
                playerHealth--;
                audioSF.clip = hitSF;
                audioSF.Play();
                SwitchMaterialRecursive(transform);
                isInvincible = true;
                invincibilityTimer = invincibilityDuration;

                Vector3 pushDirection = transform.position - other.contacts[0].point;
                playerRB.AddForce(pushDirection.normalized * pushForce, ForceMode.Impulse);
            }
            // push the player away from the enemies if it gets hit
            if (canMove && other.gameObject.CompareTag("Enemy"))
            {
                if (other.contacts.Length > 0)
                {
                    Vector3 averageContactPoint = Vector3.zero;

                    foreach (ContactPoint contactPoint in other.contacts)
                    {
                        averageContactPoint += contactPoint.point;
                    }

                    averageContactPoint /= other.contacts.Length;

                    Vector3 pushbackDirection = transform.position - averageContactPoint;
                    pushbackDirection.Normalize();

                    float adjustedForce = pushbackForce * 0.5f;

                    canMove = false;

                    Rigidbody rb = GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.AddForce(pushbackDirection * adjustedForce, ForceMode.Impulse);
                    }

                    Invoke("EnableMovement", pushbackDuration);
                }
            }
        }
        //makes shure the player does not go through walls
        if (other.gameObject.CompareTag("Wall"))
        {
            Vector3 bounceDirection = -other.contacts[0].normal;
            transform.position += bounceDirection * resetDistance;
        }
    }

    public Vector3 GetFacingDirection()
    {
        return transform.forward;
    }

    public IEnumerator Reloading()
    {
        Reload();
        yield return new WaitForSeconds(ReloadSpeed);
        Ammo = MaxAmmo;
        canShootRL = true;
    }
    //flashes the playse and its child objects white
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

    bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    bool IsPointerOverOverlay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, overlayLayer))
        {
            return true;
        }

        return false;
    }

    void EnableMovement()
    {
        canMove = true;
    }

    void Hit()
    {
        audioSF.clip = hitSF;
        audioSF.Play();
    }

    void Reload()
    {
        audioSF.clip = recharge;
        audioSF.Play();
    }

    void Shoot()
    {
        audioSF.clip = shotSound;
        audioSF.pitch = Random.Range(1.50f, 1.90f);
        audioSF.volume = 0.1f;
        audioSF.Play();
    }

    void WalkForwardSound()
    {
        walkAudioSource.clip = walkSound;
        walkAudioSource.pitch = 1.5f;
        walkAudioSource.volume = 0.2f;
    
        walkAudioSource.Play();
    }

    void WalkBackwardSound()
    {
        walkAudioSource.clip = walkSound;
        walkAudioSource.pitch = 1.3f;
        walkAudioSource.volume = 0.5f;
        walkAudioSource.Play();
    }

    void RotateSound()
    {
        rotateAudioSource.clip = rotateSoundFX;
        rotateAudioSource.pitch = 1.3f;
        rotateAudioSource.volume = 0.2f;
        rotateAudioSource.Play();
    }

    void StopRotateSound()
    {
        if (rotateAudioSource.isPlaying)
        {
            rotateAudioSource.Stop();
        }
    }

    void StopWalkSound()
    {
        if (walkAudioSource.isPlaying)
        {
            walkAudioSource.Stop();
        }
    }
}
