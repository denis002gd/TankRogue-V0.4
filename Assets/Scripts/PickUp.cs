using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{
    private bool Applied;
    private int cost = 2;
    public Button PickUpButton;
    public Move playerStats;
    public GameManager points;
    void Start()
    {
        PickUpButton.onClick.AddListener(UPgradePickUp);
    }

    void UPgradePickUp(){
        if(points.UpgradePoints >= cost){
        points.pickRange.endRange += 1f;
         points.UpgradePoints -= cost;
        }

        
    }
}