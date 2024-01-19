using UnityEngine;
using UnityEngine.UI;

public class ActivesScript : MonoBehaviour
{
    public Button ability;
    public Button spawner;
    public Button ultimate;
    public Image abilityOverlay;
    public Text countdownText;
    public bool coolingDown;
    private float waitTime = 10f;
    public LowLevelAbiliities abilities1;
    private float cooldownTimer;
    private float additionalCooldownTimer;

    void Start()
    {
        ability.onClick.AddListener(AbilityLogic);
        spawner.onClick.AddListener(SpawnerLogic);
        ultimate.onClick.AddListener(UltimateLogic);

        cooldownTimer = 0.0f;
        additionalCooldownTimer = 0.0f;
        HideCountdownText();
    }

    void Update()
    {
        CheckKeyPress();

        if (coolingDown)
        {
            cooldownTimer += Time.deltaTime;

            // Calculate the fill amount from 0 to 1
            float fillAmount = Mathf.Clamp01(cooldownTimer / (waitTime));
            abilityOverlay.fillAmount = 0.0f + fillAmount;

            if (cooldownTimer >= waitTime)
            {
                coolingDown = false;
                abilityOverlay.fillAmount = 1.0f; // Set fill amount to 0 when cooldown is complete
                additionalCooldownTimer = 30.0f; // Start the additional cooldown timer
                HideCountdownText();
            }
        }

        // Additional cooldown timer after the main cooldown is complete
        if (additionalCooldownTimer > 0)
        {
            additionalCooldownTimer -= Time.deltaTime;

            // Update countdown text
            int countdownValue = Mathf.CeilToInt(additionalCooldownTimer);
            countdownText.text = countdownValue.ToString();

            if (additionalCooldownTimer <= 0)
            {
                // Additional cooldown is complete, allow using the ability again
                ResetAbilities();
                abilityOverlay.fillAmount = 0.0f; // Set fill amount to 0 when the additional cooldown is complete
            }
            else
            {
                ShowCountdownText();
            }
        }
        else
        {
            HideCountdownText();
        }
    }

    void CheckKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !coolingDown && additionalCooldownTimer <= 0)
        {
            ability.onClick.Invoke();
            coolingDown = true;
            cooldownTimer = 0.0f;
        }
    }

    void AbilityLogic()
    {
        Debug.Log("ability used");
        abilities1.NoReload();
    }

    void SpawnerLogic()
    {
        Debug.Log("spawn used");
    }

    void UltimateLogic()
    {
        Debug.Log("ultimate used");
    }

    void ResetAbilities()
    {
        cooldownTimer = 0.0f;
        additionalCooldownTimer = 0.0f;
    }

    void ShowCountdownText()
    {
        countdownText.gameObject.SetActive(true);
    }

    void HideCountdownText()
    {
        countdownText.gameObject.SetActive(false);
    }
}
