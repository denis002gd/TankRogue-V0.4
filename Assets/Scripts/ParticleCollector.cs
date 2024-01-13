using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollector : MonoBehaviour
{
    ParticleSystem ps;
    public AudioSource xpColect;
    Transform triggerTarget;
    GameObject Enemy;
    
    
    List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();
   
   void Awake() {
    xpColect = GetComponent<AudioSource>();
   }
    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        Enemy = GameObject.FindGameObjectWithTag("Enemy");
        triggerTarget = Enemy.transform;
        ps.trigger.AddCollider(triggerTarget);
         
    }
    void Update() {
        
        
    }
   private void OnParticleTrigger() 
{
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    int triggeredParticles = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    for (int i = 0; i < triggeredParticles; i++){
        ParticleSystem.Particle p = enter[i];
        p.remainingLifetime = 0f;
        enter[i] = p;
         xpColect.Play();
        GameManager.Instance.IncrementCollectedParticleCount(); 
        Destroy(this.gameObject,6000);
        
    
        
    }
   

    ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    
    
}
   
    

}
