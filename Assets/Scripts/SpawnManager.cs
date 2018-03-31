using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SpawnManager: MonoBehaviour
{
    public TextMeshPro enemyCount;
    public float timeToWait = 0;
    public List<EnemySpawner> spawners = new List<EnemySpawner>();
    public bool nuking = false;

    protected List<int> unusedSpawnerIndex = new List<int>();
    protected List<GameObject> spawnedEnemies = new List<GameObject>();
    float spawnTimer = 0;

    public void Start()
    {
        for (int i = 0; i < spawners.Count; ++i)
        {
            unusedSpawnerIndex.Add(i);
        }
    }

    public int GetUnusedRandomIndex()
    {
        if(unusedSpawnerIndex.Count <= 0)
        {
            for (int i = 0; i < spawners.Count; ++i)
            {
                unusedSpawnerIndex.Add(i);
            }
        }

        int randIndex = UnityEngine.Random.Range(0, unusedSpawnerIndex.Count);

        return randIndex;
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount.text = spawnedEnemies.Count.ToString();
        if (!nuking)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer > timeToWait)
            {
                spawnTimer = 0;
                int randUnusedIndex = GetUnusedRandomIndex();
                GameObject enemy = spawners[unusedSpawnerIndex[randUnusedIndex]].SpawnRandomEnemy();
                unusedSpawnerIndex.RemoveAt(randUnusedIndex);
                enemy.GetComponent<DefaultEnemy>().OnDeath = RemoveFromEnemiesList;
                spawnedEnemies.Add(enemy);
            }
        }
    }

    public void RemoveFromEnemiesList(GameObject obj)
    {
        spawnedEnemies.Remove(obj);
    }

}

