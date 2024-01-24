using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBackDash : MonoBehaviour
{
    private GameObject player;
    private Dash moveScript;

    public float dashDistance = 5f;
    void Start()
    {
        player = GameObject.FindWithTag("HighDamageBullet");
        moveScript = player.GetComponent<Dash>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(moveScript.isDashing);
    }
    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.CompareTag("player") && moveScript.isDashing == true)
        {
            Debug.Log("hit");
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
