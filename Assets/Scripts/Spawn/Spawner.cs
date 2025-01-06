using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private SpawnManager spawnManager;

    [Header("Set Spawn Time")]
    [SerializeField] float spawnTime = 1f;
    [SerializeField] float totalSpawnTime = 20f;

    private bool spawnAnimalOnce;

    private void Start()
    {
        StartCoroutine("SpawnObstacle");
    }

    IEnumerator SpawnObstacle()
    {
        float currentTime = 0f;

        while (true)
        {
            // 撤老 版快
            if (GameManager.Instance.GetTimeInfo() == TimeInfo.Day)
            {
                if (!spawnAnimalOnce)
                {
                    spawnManager.AnimalSpawn((int)Random.Range(0f, spawnManager.animalPrefabs.Length));
                    spawnAnimalOnce = true;
                    currentTime = 0f;
                }
            }
            // 广老 版快
            else if (GameManager.Instance.GetTimeInfo() == TimeInfo.night)
            {
                spawnAnimalOnce = false;

                while (currentTime < totalSpawnTime)
                {
                    currentTime += spawnTime;
                    spawnManager.EnemySpawn((int)Random.Range(0f, spawnManager.enemyPrefabs.Length));

                    yield return new WaitForSeconds(spawnTime);
                }
            }

            yield return null;
        }
    }
}
