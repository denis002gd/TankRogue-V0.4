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
        if (Time.time > lastUsedTime + coolDown)
        {
            playerStats.isInvincible = false;
            playerStats.canBeHit = true;
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
                backgroundImage.color = Color.red;
            }
        }
    }

    public void IncincibleStart()
    {
        if (wasBought)
        {
           
            if (Time.time > lastUsedTime + coolDown)
            {
                playerStats.invincibilityTimer = 5f;
               
                lastUsedTime = Time.time;
                playerStats.canBeHit = false;
            }
            else
            {
                
                playerStats.canBeHit = true;

            }
        }
    }

    public void WasAlreadyBought()
    {
        if (wasBought && inUse)
        {
            activesActivation.abilityId = 2;
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
        backgroundImage.color = Color.green;
        Destroy(price);
    }
}
