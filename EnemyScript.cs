using System.Collections;
using System.Collections.Generic;
using UnityEngine;
      [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class EnemyScript : MonoBehaviour
{
     UnityEngine.AI.NavMeshAgent navig;
     Transform target;
     [SerializeField] private float pushbackForce = 10f;
     Vector3 resetpos;
    void Start()
{
    ResetYposition();  
}

void Update()
{
    if (transform.position.y < 1)
    {
        Vector3 newPosition = transform.position;
        newPosition.y = resetpos.y;
        transform.position = newPosition;
    }
}
    IEnumerator RefreshPath(){
        float refreshRate = 0.25f;
        while(target != null){
            Vector3 targetPositiion = new Vector3(target.position.x,0,target.position.z);
            navig.SetDestination(targetPositiion);
            yield return new WaitForSeconds(refreshRate);
        }
    } 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            // Calculate the direction from the bullet to the enemy.
            Vector3 pushDirection = (transform.position - other.transform.position).normalized;

            // Apply a force to move the enemy in the opposite direction.
            ApplyPushbackForce(pushDirection);
        }
    }

    private void ApplyPushbackForce(Vector3 direction)
    {
        // Ensure the enemy does not have a Rigidbody.
        Rigidbody enemyRigidbody = GetComponent<Rigidbody>();
        if (enemyRigidbody == null)
        {
            // Move the enemy's Transform in the opposite direction with a certain force.
            transform.position += direction * pushbackForce * Time.deltaTime;
        }
    }

    void ResetYposition(){
    navig = GetComponent<UnityEngine.AI.NavMeshAgent>();
    target = GameObject.FindGameObjectWithTag("Basic enemy").transform;
    resetpos = new Vector3(0, 1, 0);
    RefreshPath();
    }

}
