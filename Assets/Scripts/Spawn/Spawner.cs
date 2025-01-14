using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private int numberOfAnimal = 10;
    [SerializeField] private int numberOfTotalResource = 100;


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
            TimeInfo currentTimeInfo = GameManager.Instance.GetTimeInfo();

            // 撤老 版快
            if (currentTimeInfo == TimeInfo.Day)
            {
                if (!spawnAnimalOnce)
                {
                    for (int i = 0; i < numberOfAnimal; i++)
                    {
                        spawnManager.AnimalSpawn((int)Random.Range(0f, spawnManager.animalPrefabs.Length));
                    }

                    for (int i = 0; i < numberOfTotalResource; i++)
                    {
                        spawnManager.ResourceSpawn((int)Random.Range(0f, spawnManager.resourcePrefabs.Length));
                    }

                    spawnAnimalOnce = true;
                    currentTime = 0f;
                }
            }
            // 广老 版快
            else if (currentTimeInfo == TimeInfo.night)
            {
                spawnAnimalOnce = false;
                spawnManager.SetAnimalActiveFalse();
                spawnManager.SetResourceActiveFalse();

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
