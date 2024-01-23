using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBackDash : MonoBehaviour
{
    private GameObject player;
    private Move moveScript;

    public float dashDistance = 5f;
    void Start()
    {
        player = GameObject.FindWithTag("player");
        moveScript = player.GetComponent<Move>();
    }

    // Update is called once per frame
    void Update()
    {
    
    }
    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.CompareTag("player") && moveScript.canBeHit == false)
        {
            StartCoroutine(Dashes());
        }
    }
    IEnumerator Dashes()
    {
   

        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + -transform.forward * dashDistance;

        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            t = 1 - Mathf.Pow(1 - t, 3);

            transform.position = Vector3.Lerp(startPosition, endPosition, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

       transform.position = endPosition;

    }
}
