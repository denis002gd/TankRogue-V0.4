using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
     private bool Applied;
    private int cost = 1;
    public Button HealthButton;
    public Move playerStats;
    public GameManager points;
    void Start()
    {
        HealthButton.onClick.AddListener(UgradeHealth);
    }

   
    void UgradeHealth(){
        if(points.UpgradePoints >= cost){    
         playerStats.playerHealth += 1;
         points.UpgradePoints -= cost;
    
        }

        
    }
}