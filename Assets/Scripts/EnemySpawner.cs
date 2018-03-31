using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyParent;
    public List<GameObject> enemies = new List<GameObject>();
    public float leftMostSpawnPoint;
    public float RightMostSpawnPoint;
    public float yPos;

    // Use this for initialization
    void Start()
    {
    }

    public GameObject SpawnRandomEnemy()
    {
        int randIndex = UnityEngine.Random.Range(0, enemies.Count);
        float xPos = UnityEngine.Random.Range(leftMostSpawnPoint, RightMostSpawnPoint);
        GameObject enemy = Instantiate(enemies[randIndex], new Vector3(xPos, yPos), Quaternion.identity);
        enemy.transform.parent = enemyParent.transform;
        return enemy;
    }


}
