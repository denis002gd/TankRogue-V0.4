using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public float spawnInterval = 5f; 
    public float spawnDistance = 10f;
    public float rareSpawnChance = 0.2f;
    public GameObject spawner;
    private float nextSpawnTime = 0f;
    void Start()
    {
    
    }
    void Update()
    {
        // Check if it's time to spawn a new enemy
        if (Time.time >= nextSpawnTime)
        {
            if(Random.value < rareSpawnChance){
                SpawnEnemyOutsideVision(1);
            }else{
                SpawnEnemyOutsideVision(0);
            }
            
            // Update the next spawn time
            nextSpawnTime = Time.time + spawnInterval;
        }
    }
    void SpawnEnemyOutsideVision(int enemyIndex)
    {
        // Calculate a random angle and position outside the player's vision range
        float spawnAngle = Random.Range(0f, 360f);
        Vector3 spawnDirection = Quaternion.Euler(0f, spawnAngle, 0f) * Vector3.forward;
        Vector3 spawncoordonates = new Vector3(spawner.transform.position.x,1,spawner.transform.position.z);
        Vector3 spawnPosition = spawncoordonates + spawnDirection * spawnDistance;
        // Instantiate the enemy at the calculated position
        Instantiate(enemyPrefab[enemyIndex], spawnPosition, Quaternion.identity);
    }
}

