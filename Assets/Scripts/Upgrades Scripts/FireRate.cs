using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireRate : MonoBehaviour
{
     private bool Applied;
    private int cost = 2;
    public Button FireRateButton;
    public Move playerStats;
    public GameManager points;
    void Start()
    {
        FireRateButton.onClick.AddListener(UgradeFireRate);
    }

   
    void UgradeFireRate(){
        if(points.UpgradePoints >= cost){
            if(playerStats.fireInterval >= 0.1){
         playerStats.fireInterval -= 0.02f;
         cost++;
            } else Debug.Log("Max FireRate limit");
        
         points.UpgradePoints -= cost;
        }

        
    }
}
