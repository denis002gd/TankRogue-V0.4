using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BigShot : MonoBehaviour
{
    private bool Applied = true;
    private int cost = 5;
    public Button ScaleButton;
    public Move playerStats;
    public GameManager points;
    void Start()
    {
        ScaleButton.onClick.AddListener(UPgradeScale);
    }

    // Update is called once per frame
    void Update()
    {  
    }
    void UPgradeScale(){     
      
        if(points.UpgradePoints >= cost && Applied){
         playerStats.scaleOfBullet += 0.4f;
         points.UpgradePoints -= cost;
         ScaleButton.interactable = false;
         Applied = false;
        }

        
    }
}

