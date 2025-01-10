using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private int numberOfAnimal = 10;


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
            // ���� ���
            if (GameManager.Instance.GetTimeInfo() == TimeInfo.Day)
            {
                if (!spawnAnimalOnce)
                {
                    for (int i = 0; i < numberOfAnimal; i++)
                    {
                        spawnManager.AnimalSpawn((int)Random.Range(0f, spawnManager.animalPrefabs.Length));
                    }

                    spawnAnimalOnce = true;
                    currentTime = 0f;
                }
            }
            // ���� ���
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
