using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    private bool Applied;
    private int cost = 2;
    public Button DamageButton;
    public Move playerStats;
    public GameManager points;
    void Start()
    {
        DamageButton.onClick.AddListener(UPgradeDamage);
    }

    void UPgradeDamage(){
        if(points.UpgradePoints >= cost){
         playerStats.playerDamage += 10f;
         points.UpgradePoints -= cost;
        }

        
    }
}
