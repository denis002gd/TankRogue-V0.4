using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public Button restartButton;
    public Button quitButton;
    private bool isOpen = false;
    
    
    

    void Start()
    {
        openSettings.onClick.AddListener(MenuOpen);
        closeSettings.onClick.AddListener(MenuClose);
        restartButton.onClick.AddListener(Restart);
        quitButton.onClick.AddListener(QuitButton);
        
        SFX.value = gameAudioSources[0].volume;
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOpen)
                MenuClose();
            else
                MenuOpen();
        }

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
        isOpen = true;
        SettingsMenu.SetActive(true);
        Time.timeScale = 0.03f;
    }
    void MenuClose(){
        isOpen = false;
        SettingsMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    void QuitButton(){
        Time.timeScale = 1f;
       string currentSceneName = SceneManager.GetActiveScene().name;
       Application.Quit();
    }
    void Restart(){
        Time.timeScale = 1f;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
 }


