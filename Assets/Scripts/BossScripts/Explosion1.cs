using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion1 : MonoBehaviour
{
    public GameObject tank;
    public Transform tankPosition;
    void Start()
    {
        transform.position = tankPosition.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
