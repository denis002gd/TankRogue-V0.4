using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public LevelData[] levels;
    public GameObject player;
    public float spawnRadius = 10f;
    public Transform spawnPoint;
    public Text LevelNR;
    public Text EnemiesNR;
    public GameObject TankBot;
    public Transform TBtransform;
    public GameObject TBheathbar;

public Transform PlayerPos;

    public int currentLevelIndex = 1;
    private int enemiesRemaining = 0; // Counter for remaining enemies

    void Start()
    {
        StartLevel();
        TBheathbar.SetActive(false); 
        
    }

    private void Update()
    {
        LevelNR.text = currentLevelIndex.ToString();
        EnemiesNR.text = enemiesRemaining.ToString();
    }

    void StartLevel()
    {
        if (currentLevelIndex < levels.Length)
        {
            enemiesRemaining = 0; // Reset the counter for the new level
            StartCoroutine(SpawnEnemies(levels[currentLevelIndex], 1f));
        }
       if(currentLevelIndex == 10)
        {
            
            TankBot.SetActive(true);
            enemiesRemaining++;
        }
    }

IEnumerator SpawnEnemies(LevelData levelData, float spawnRate)
{
    foreach (var enemyInfo in levelData.enemies)
    {
        if (enemyInfo.enemyPrefab != null)
        {
          

            for (int i = 0; i < enemyInfo.count; i++)
            {
                // Calculate position on the circle
                float angle = Random.Range(0f, 360f);
                float spawnX = player.transform.position.x + spawnRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
                float spawnZ = player.transform.position.z + spawnRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
                Vector3 spawnPosition = new Vector3(spawnX, 0f, spawnZ);

                if (enemyInfo.enemyPrefab != null)
                {
                    Instantiate(enemyInfo.enemyPrefab, spawnPosition, Quaternion.identity);
                    enemiesRemaining++;
                }
               

                yield return new WaitForSeconds(spawnRate);
            }
        }
       
    }
}

    public void EnemyDefeated()
    {
        enemiesRemaining--;

        if (enemiesRemaining <= 0)
        {
            // All enemies defeated, progress to the next level
            currentLevelIndex++;
            StartCoroutine(NextLevel());
        }
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(2f);
        StartLevel();
    }
}

