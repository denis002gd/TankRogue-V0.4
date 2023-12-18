using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkEnemy : MonoBehaviour
{
     public Transform player;
    public float stepDistance = 1.0f;
    public float moveSpeed = 2.0f;

    void Start()
    {
        StartCoroutine(MoveTowardsPlayer());
    }

    IEnumerator MoveTowardsPlayer()
    {
        while (true)
        {
            if (player != null)
            {
                Vector3 startPosition = transform.position;
                Vector3 targetPosition = player.position;
                float distance = Vector3.Distance(startPosition, targetPosition);

                float journeyTime = distance / moveSpeed;
                float elapsedTime = 0f;

                while (elapsedTime < journeyTime)
                {
                    transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / journeyTime);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                // Ensure the final position is exactly the target position
                transform.position = targetPosition;

                // Wait for the next iteration
                yield return new WaitForSeconds(1.0f);
            }
            else
            {
                Debug.LogWarning("Player not assigned to the enemy script.");
                yield return null;
            }
        }
    }
}