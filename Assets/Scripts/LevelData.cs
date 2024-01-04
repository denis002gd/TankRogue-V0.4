using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
   [System.Serializable]
    public class EnemyInfo
    {
        public GameObject enemyPrefab;
        public int count;
    }

    public EnemyInfo[] enemies;
    
}
