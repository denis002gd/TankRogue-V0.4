using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    public Move playerStats;
    public TextMeshProUGUI damageIndicator;
    public TextMeshProUGUI FireRateIndicator;
    public GameManager gameManag;
    public Slider musicSlider;
    public AudioSource Music;
    public Slider SFX;
    public AudioSource[] gameAudioSources;
    public Button openSettings;
    public Button closeSettings;
    public GameObject SettingsMenu;
    
    
    

    void Start()
    {
        openSettings.onClick.AddListener(MenuOpen);
        closeSettings.onClick.AddListener(MenuClose);
        
        SFX.value = gameAudioSources[0].volume;
        
    }

    void Update()
    {
        Music.volume = musicSlider.value;
        if (playerStats != null)
        {
            
            damageIndicator.text = playerStats.playerDamage.ToString();
            FireRateIndicator.text = gameManag.FR.ToString();
        }
        UpdateAudioVolumes();
        
    }
    public void UpdateAudioVolumes()
{
  
        foreach (var audioSource in gameAudioSources)
        {
         
                audioSource.volume = SFX.value;
        }
    }
    void MenuOpen(){
        SettingsMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    void MenuClose(){
        SettingsMenu.SetActive(false);
        Time.timeScale = 1f;
    }
 }


