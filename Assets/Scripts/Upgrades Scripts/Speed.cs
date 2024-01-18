using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speed : MonoBehaviour
{
     private bool Applied;
    private int cost = 2;
    public Button SpeedButton;
    public Move playerStats;
    public GameManager points;
    void Start()
    {
        SpeedButton.onClick.AddListener(UgradeSpeed);
    }

   
    void UgradeSpeed(){
        if(points.UpgradePoints >= cost){
            if(playerStats.speed <= 10){
         playerStats.speed += 1f;
         points.UpgradePoints -= cost;
        
            } else Debug.Log("Max Speed limit");
    
        }

        
    }
}
