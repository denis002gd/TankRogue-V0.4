using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LowLevelAbiliities : MonoBehaviour
{
    public Move playerStats;
    public GameManager gameManager;
    public ActivesScript activesActivation;
    private float coolDown = 5f;
    private float originalReloadSpeed;
    private float originalFireRate;
    private float lastUsedTime;
    private float originalInvTimer;
    public bool invincibilityBought;
    [Header("UiElements")]
    public Button[] boughtButton;
    public int upgradeID;
    void Start()
    {
        lastUsedTime = -coolDown;
        originalReloadSpeed = playerStats.ReloadSpeed;
        originalFireRate = playerStats.fireInterval;
        originalInvTimer = playerStats.invincibilityDuration;



    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > lastUsedTime + coolDown + 5f)
        {
            // If 10 seconds have passed since the last use, reset ReloadSpeed to the original value
            playerStats.ReloadSpeed = originalReloadSpeed;
            playerStats.fireInterval = originalFireRate;
            playerStats.invincibilityDuration = originalInvTimer;
        }
    }

    public void NoReload()
    {
        if (Time.time > lastUsedTime + coolDown)
        {
            playerStats.ReloadSpeed = 0f;
            playerStats.fireInterval -= 0.1f;
            lastUsedTime = Time.time;
        }
        else
        {
            playerStats.ReloadSpeed = 2f;
        }
    }
    public void Invincible()
    {
        if (invincibilityBought)
        {

            gameManager.UpgradePoints -= 9;
        }
        if (Time.time > lastUsedTime + coolDown)
        {
            playerStats.isInvincible = true;
            playerStats.invincibilityTimer = 5f;
        }
    }

}
