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
    [SerializeField] Timer timer;

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

                switch ((timer.DayCount / 2) + 1) {
                    case 1:
                        totalSpawnTime = 5f;
                        break;
                    case 2:
                        totalSpawnTime = 5f;
                        break;
                    case 3:
                        totalSpawnTime = 8f;
                        break;
                    case 4:
                        totalSpawnTime = 10f;
                        break;
                    case 5:
                        totalSpawnTime = 10f;
                        break;
                    case 6:
                        totalSpawnTime = 15f;
                        break;
                    case 7:
                        totalSpawnTime = 15f;
                        break;
                    case 8:
                        totalSpawnTime = 17f;
                        break;
                    case 9:
                        totalSpawnTime = 20f;
                        break;
                    case 10:
                        totalSpawnTime = 20f;
                        break;
                }

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
