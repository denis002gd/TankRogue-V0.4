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
    private bool wasBought = false;
    public Color originalCol;
    public Image backgroundImage;
    private bool inUse = false;

    [Header("UiElements")]
    public Button boughtButton;
    public Text priceText;
    public Text price;

    void Start()
    {
        boughtButton.onClick.AddListener(WasAlreadyBought);
        lastUsedTime = -coolDown;
        originalReloadSpeed = playerStats.ReloadSpeed;
        originalFireRate = playerStats.fireInterval;
    }

    void Update()
    {
        // Check if enough time has passed since the last use
        if (Time.time > lastUsedTime + coolDown + 5f)
        {
            // If 10 seconds have passed since the last use, reset ReloadSpeed to the original value
            playerStats.ReloadSpeed = originalReloadSpeed;
            playerStats.fireInterval = originalFireRate;
        }
        if (wasBought)
        {
            if (activesActivation.abilityId == 1)
            {
                inUse = true;
            }
            else
            {
                inUse = false;
                priceText.text = ("Deactivated");
                Debug.Log("already bought, but not used");
                backgroundImage.color = Color.red;
            }
        }
    }

    public void NoReload()
    {
        if (wasBought)
        {
            // Check if enough time has passed since the last use
            if (Time.time > lastUsedTime + coolDown)
            {
                // Apply the ability effects
                playerStats.ReloadSpeed = 0f;
                playerStats.fireInterval -= 0.1f;
                lastUsedTime = Time.time;
            }
            else
            {
                // If not enough time has passed, reset ReloadSpeed to the original value
                playerStats.ReloadSpeed = originalReloadSpeed;
            }
        }
    }

    public void WasAlreadyBought()
    {
        if (wasBought && inUse)
        {
            activesActivation.abilityId = 1;
            Debug.Log("already bought");
        }
        else
        {
            BuyButton();
        }
    }

    public void BuyButton()
    {
        wasBought = true;
        activesActivation.abilityId = 1;
        activesActivation.noAbility = true;
        gameManager.UpgradePoints -= 15;
        priceText.text = ("Activated!");
        Debug.Log("ill buy it!");
        activesActivation.waitTime = 10f;
        activesActivation.rechargeTime = 30f;
        backgroundImage.color = Color.green;
        Destroy(price);
    }
}
