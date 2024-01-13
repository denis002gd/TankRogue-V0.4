using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabInstantiation : MonoBehaviour
{
    public GameObject prefabB; // Assign PrefabB in the Unity Editor or load it dynamically
    public GameObject minXp;
    public GameObject enemyBullet;
    private static PrefabInstantiation instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Duplicate PrefabManager instance. Destroying the new one.");
            Destroy(gameObject);
        }
    }

    public static PrefabInstantiation Instance
    {
        get { return instance; }
    }

    public GameObject InstantiatePrefabB(Vector3 position, Quaternion rotation)
    {
        if (prefabB != null)
        {
            return Instantiate(prefabB, position, rotation);
        }
        else
        {
            Debug.LogError("PrefabB not assigned to PrefabManager.");
            return null;
        }
    }
    public GameObject InstantiateMinXp(Vector3 position, Quaternion rotation)
    {
        if (prefabB != null)
        {
            return Instantiate(minXp, position, rotation);
        }
        else
        {
            Debug.LogError("PrefabB not assigned to PrefabManager.");
            return null;
        }
    }
    public GameObject InstantiateEnemyBullet(Vector3 position, Quaternion rotation)
    {
        if (prefabB != null)
        {
            return Instantiate(enemyBullet, position, rotation);
        }
        else
        {
            Debug.LogError("PrefabB not assigned to PrefabManager.");
            return null;
        }
    }
}