using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash : MonoBehaviour
{
    private Move playerThing;
    public GameManager gameManager;
    public ActivesScript activesActivation;
    private bool wasBought = false;
    public Color originalCol;
    public Image backgroundImage;
    private bool inUse = false;
    public Transform player;
    public GameObject PlayerObj;
    public float dashDistance = 5f; 
    public bool isDashing = false;

    [Header("UiElements")]
    public Button boughtButton;
    public Text priceText;
    public Text price;

    void Start()
    {
        boughtButton.onClick.AddListener(WasAlreadyBought);
        playerThing = PlayerObj.GetComponent<Move>();
    }

    void Update()
    {
    
        if (wasBought)
        {
            if (activesActivation.abilityId == 3)
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

    public void DashStart()
    {
        if (wasBought)
        {
            
            if (!isDashing)
            {
         
                StartCoroutine(Dashes());
            }
        }
    }

    public void WasAlreadyBought()
    {
        if (wasBought && inUse)
        {
            activesActivation.abilityId = 3;
            priceText.text = ("Activated!");
        }
        else
        {
            BuyButton();
        }
    }




    public void BuyButton()
    {
        wasBought = true;
        activesActivation.abilityId = 3;
        activesActivation.noAbility = true;
        activesActivation.waitTime = 0.5f;
        activesActivation.rechargeTime = 3f;
        gameManager.UpgradePoints -= 5;
        priceText.text = ("Activated!");
        backgroundImage.color = Color.green;
        Destroy(price);
    }
    IEnumerator Dashes()
    {
        
        isDashing = true;

        Vector3 startPosition = player.transform.position;
        Vector3 endPosition = startPosition + player.transform.forward * dashDistance;

        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            t = 1 - Mathf.Pow(1 - t, 3);

            player.transform.position = Vector3.Lerp(startPosition, endPosition, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the player reaches the exact end position
        player.transform.position = endPosition;
        isDashing = false;
    }
}
