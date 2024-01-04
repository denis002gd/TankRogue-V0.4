using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance

    private int totalCollectedParticles = 0;
    public Slider slider;
    public TextMeshProUGUI levelTXT;
    public TextMeshProUGUI framIndicator;
    public TextMeshProUGUI Timer;
    public TextMeshProUGUI Health;
    public TextMeshProUGUI PointsBar;
    public Text pointsIndicator;

    public GameObject DeathScreen;
    public Button RestartGame;
    public Button QuitGame;
    public Button UpgradesBut;

    public Move playerStats;
    public EnemySpawner Spawn;
    public AudioSource DeathSF;
    public AudioSource MainSong;
    public GameObject FieldForce;
    public GameObject Camera;
    public GameObject UpgradeMenuObj;
    public ParticleSystemForceField pickRange;
    public int UpgradePoints;
    private float time;
    public int FR = 1;
    
    private float fps;
    private int level;
    private float t = 0.0f;
    private bool playerDied = false;
      
      void Start()
      {
        UpgradesBut.onClick.AddListener(UpgradeMenu);
        DeathScreen.SetActive(false);
        
        RestartGame.onClick.AddListener(RestartAPP);
        QuitGame.onClick.AddListener(QuitAPP);
        DeathSF = GetComponent<AudioSource>();
        
        MainSong = Camera.GetComponent<AudioSource>();
        pickRange = FieldForce.GetComponent<ParticleSystemForceField>();
        
      }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
       
       
    }
    private void Update() {
        if(UpgradePoints < 0){
            UpgradePoints = 0;
        }
        fps = (int)(1f/ Time.unscaledDeltaTime);
        framIndicator.text = fps.ToString();
        time = Time.time;
        Timer.text = time.ToString("F1");
        if(slider != null){
        slider.value = totalCollectedParticles;
       
        }
        pointsIndicator.text = UpgradePoints.ToString();
        PointsBar.text = UpgradePoints.ToString();

        if(slider.value == slider.maxValue){
            UpgradePoints++;
            totalCollectedParticles = 0;
            slider.maxValue += 1;
            
            level++;
            levelTXT.text = level.ToString();
            slider.value = slider.minValue;
            t = 0.0f;
           

        }
        t += 0.3f * Time.deltaTime;
        
        

        
       if(playerStats.playerHealth <= 0 && !playerDied){
        
            MainSong.pitch = 0.5f;
         
        Time.timeScale = 0.2f;
        DeathScreen.SetActive(true);
        
        
        if (DeathSF != null && !DeathSF.isPlaying)
            {
                DeathSF.Play();
                
            }
        playerDied = true;
       
       }
        
    }

    public void IncrementCollectedParticleCount()
    {
        totalCollectedParticles++;
       
        
    }

    // Optionally, you can add a method to retrieve the total collected particles
    public int GetTotalCollectedParticles()
    {
        return totalCollectedParticles;
    }
   void UpgradeMenu(){
    UpgradeMenuObj.SetActive(true);
    Time.timeScale = 0f;
    
   }
   void RestartAPP()  {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
    void QuitAPP(){
        Application.Quit();
    }

}