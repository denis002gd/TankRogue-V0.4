using UnityEngine;
using TMPro;

public class StatsManager : MonoBehaviour
{
    public Move playerStats;  // Assign this in the Inspector
    public TextMeshProUGUI damageIndicator;
    public TextMeshProUGUI FireRateIndicator;
    public GameManager gameManag;
    
    

    void Start()
    {
        // Check if enemyHealth is not assigned
        if (playerStats == null)
        {
            Debug.LogError("EnemyHealthbar is not assigned to StatsManager.");
        }

        // Check if damageIndicator is not assigned
        if (damageIndicator == null)
        {
            Debug.LogError("TextMeshProUGUI is not assigned to damageIndicator in StatsManager.");
        }


    }

    void Update()
    {
        // Check if enemyHealth is assigned before accessing damage
        if (playerStats != null)
        {
            
            damageIndicator.text = playerStats.playerDamage.ToString();
            FireRateIndicator.text = gameManag.FR.ToString();
        }
        
    }
    }


