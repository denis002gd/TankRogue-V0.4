using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
   public PulseObj PulseScript;
   public AudioClip ricochet;
   public AudioSource scr;

    // Update is called once per frame
    void Start()
    {
        scr.clip = ricochet;
    }
    void Update()
    {
        
    }
     private void OnCollisionEnter(Collision coll)
    {
        
            PulseScript.SwitchMaterial();
            scr.Play();
            Debug.Log("gotem");
        
        
    }

}
