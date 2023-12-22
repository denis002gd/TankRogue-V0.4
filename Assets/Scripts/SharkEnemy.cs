using System.Collections;
using UnityEngine;

public class SharkEnemy : MonoBehaviour
{
    public Transform target;
    public float moveDistance = 1f;
    public float moveDuration = 1f;
    public float frequency = 0.5f;
    public float rotationSpeed = 5f;
    public float yRotation = 0f;
    public PulseObj pulseObj;
    public float Health = 200f;
    public AudioSource player;
    public AudioClip damage1;
    public Move playerStats;
    public GameObject xpDrop;
    public GameObject explosionDF;
    public Transform explosionPos;

    private float initialY;

    void Start()
    {
        initialY = transform.position.y;
        StartCoroutine(PeriodicMove());
        xpDrop = GameObject.Find("Particle System");
    }

    void Update()
    {
        RotateToPlayer();
        if(Health <= 0f){
            Instantiate(xpDrop, transform.position, transform.rotation);
             explosionPos.position = transform.position;
            explosionDF.SetActive(true);
            Death();
        }
    }

    IEnumerator PeriodicMove()
    {
        while (true)
        {
            Vector3 startPos = transform.position;
            Vector3 directionToTarget = (target.position - startPos).normalized;
            Vector3 endPos = startPos + directionToTarget * moveDistance;

            float elapsedTime = 0f;

            while (elapsedTime < moveDuration)
            {
                float t = elapsedTime / moveDuration;
                transform.position = Vector3.Lerp(startPos, endPos, t);
                transform.position = new Vector3(transform.position.x, initialY, transform.position.z); // Keep the initial Y component
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Wait for the specified frequency before starting the next move
            yield return new WaitForSeconds(frequency);
        }
    }

    void RotateToPlayer()
    {
        if (target != null)
        {
            Vector3 directionToPlayer = target.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            targetRotation *= Quaternion.Euler(0f, yRotation, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Bullet")){
            HitSFX();
            Health = Health - playerStats.playerDamage;
           SwitchMaterialRecursive(transform);
           Debug.Log(Health);
          
         pulseObj.SwitchMaterial();
        }
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
    void Death(){
        
            Destroy(gameObject);
        
    }
    void HitSFX(){
        player.clip = damage1;
        player.Play();
    }
}