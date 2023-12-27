using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Inamici")]
    public int EnemyNumber = 10;
    public GameObject SharkEnemy;
    private Vector3 range;
    public float spawnDistance = 10f;
    public float spawnInterval = 2f;
    private Transform spawnPoint;

    [Header("Vectors")]
    Vector3 spawnPosition;
    Vector3 spawncoordonates;
    Vector3 spawnDirection;
    float spawnAngle;
    int Level = 1;
    public int deadEnemies;

    void Start()
    {
        spawnPoint = GameObject.Find("Player").transform;
    }

    void Update()
    {
        
    }
    void ExecuteLevel(){
        switch (Level)
        {
            case 1:
                Level1();
                Debug.Log("level 1");
                break;
            case 2:
            Level2();
                Debug.Log("Level 2");
                break;
            default:
                Debug.Log("End");
                break;
        }
    }

    void Level1()
    {
        for (int i = 0; i < EnemyNumber; i++)
        {
            spawnDirection = Quaternion.Euler(0f, Random.Range(0f,360f), 0f) * Vector3.forward;
            spawncoordonates = new Vector3(spawnPoint.transform.position.x, 1, spawnPoint.transform.position.z);
            spawnPosition = spawncoordonates + spawnDirection * spawnDistance;

        
            Instantiate(SharkEnemy, spawnPosition, Quaternion.identity);
        }
        EnemyNumber = 20;
        Level++;
    }
    void Level2(){
        for (int i = 0; i < EnemyNumber; i++)
        {
            spawnDirection = Quaternion.Euler(0f, Random.Range(0f,360f), 0f) * Vector3.forward;
            spawncoordonates = new Vector3(spawnPoint.transform.position.x, 1, spawnPoint.transform.position.z);
            spawnPosition = spawncoordonates + spawnDirection * spawnDistance;

        
            Instantiate(SharkEnemy, spawnPosition, Quaternion.identity);
        }

        Level++;
    }
}
