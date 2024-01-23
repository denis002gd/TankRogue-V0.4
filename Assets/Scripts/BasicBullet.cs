using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : MonoBehaviour
{
    public bool canReflect = false;
    public float reflectionSpeed = 8f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 15);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (canReflect && collision.gameObject.CompareTag("bounceArea"))
        {
            ReflectBullet(collision.contacts[0].normal);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void ReflectBullet(Vector3 collisionNormal)
    {
        Vector3 reflectionDirection = Vector3.Reflect(transform.forward, collisionNormal);

        GetComponent<Rigidbody>().velocity = reflectionDirection * reflectionSpeed;
    }
}
