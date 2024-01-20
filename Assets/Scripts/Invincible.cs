using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Invincible : MonoBehaviour
{
    public Move playerStats;
    public GameManager gameManager;
    public ActivesScript activesActivation;
    private float coolDown = 5f;
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
    }

    void Update()
    {
        // Check if enough time has passed since the last use
        if (Time.time > lastUsedTime + coolDown)
        {
            playerStats.isInvincible = false;
        }
        if (wasBought)
        {
            if (activesActivation.abilityId == 2)
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

    public void IncincibleStart()
    {
        if (wasBought)
        {
            // Check if enough time has passed since the last use
            if (Time.time > lastUsedTime + coolDown)
            {
                playerStats.invincibilityTimer = 5f;
                playerStats.isInvincible = true;
                lastUsedTime = Time.time;
            }
            else
            {
                playerStats.isInvincible = false;

            }
        }
    }

    public void WasAlreadyBought()
    {
        if (wasBought && inUse)
        {
            activesActivation.abilityId = 2;
            priceText.text = ("Activated!");

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
        activesActivation.abilityId = 2;
        activesActivation.noAbility = true;
        activesActivation.waitTime = 5f;
        activesActivation.rechargeTime = 15f;
        gameManager.UpgradePoints -= 10;
        priceText.text = ("Activated!");
        Debug.Log("ill buy it!");
        backgroundImage.color = Color.green;
        Destroy(price);
    }
}
