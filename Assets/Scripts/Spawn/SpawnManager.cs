using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject animalSpawner;
    [SerializeField] private GameObject enemySpawner;
    [SerializeField] private GameObject resourceSpawner;

    public GameObject[] animalPrefabs;
    public GameObject[] enemyPrefabs;
    public GameObject[] resourcePrefabs;

    private List<GameObject>[] animalPools;
    private List<GameObject>[] enemyPools;
    private List<GameObject>[] resourcePools;
    private List<GameObject> enemySpawnPoint = new List<GameObject>(); // 적 생성 위치 모음
    private RandomSpawner randomSpawnLocation; // 오브젝트 랜덤 생성 위치

    private bool isEnemySpawned = false;
    private bool isChecking; // 적 오브젝트 활성화 여부를 체크하고 있는지 확인

    private void Awake()
    {
        #region 오브젝트 풀 초기화
        animalPools = new List<GameObject>[animalPrefabs.Length];
        enemyPools = new List<GameObject>[enemyPrefabs.Length];
        resourcePools = new List<GameObject>[resourcePrefabs.Length];

        for (int i = 0; i < animalPrefabs.Length; i++)
        {
            animalPools[i] = new List<GameObject>();
        }

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            enemyPools[i] = new List<GameObject>();
        }

        for (int i = 0; i < resourcePrefabs.Length; i++)
        {
            resourcePools[i] = new List<GameObject>();
        }
        #endregion

        randomSpawnLocation = GetComponent<RandomSpawner>();

        for (int i = 0; i < enemySpawner.transform.childCount; i++)
        {
            enemySpawnPoint.Add(enemySpawner.transform.GetChild(i).gameObject);
        }
    }

    private void Update()
    {
        // 밤일 경우 적 오브젝트가 모두 비활성화 되었는지 확인
        if (GameManager.Instance.GetTimeInfo() == TimeInfo.night && !isChecking) CheckEnemyAlive();
    }


    private void CheckEnemyAlive()
    {
        if (!isEnemySpawned) return;
        isChecking = true;

        foreach (List<GameObject> pools in enemyPools)
        {
            foreach (GameObject enemy in pools)
            {
                if (enemy.activeSelf)
                {
                    isChecking = false;
                    return;
                }
            }
        }

        GameManager.Instance.isAllEnemyDead = true;
        isChecking = false;
        isEnemySpawned = false;
    }

    public GameObject AnimalSpawn(int index)
    {
        return Spawn(animalPrefabs, animalPools, animalSpawner, index);
    }

    public GameObject EnemySpawn(int index)
    {
        isEnemySpawned = true;
        return Spawn(enemyPrefabs, enemyPools, enemySpawner, index);
    }

    public GameObject ResourceSpawn(int index)
    {
        return Spawn(resourcePrefabs, resourcePools, resourceSpawner, index);
    }

    // 동물 오브젝트 전체 비활성화
    public void SetAnimalActiveFalse()
    {
        SetObjectActiveFalse(animalPools);
    }

    // 자원 오브젝트 전체 비활성화
    public void SetResourceActiveFalse()
    {
        SetObjectActiveFalse(resourcePools);
    }

    // 오브젝트 풀링으로 게임 오브젝트 생성
    private GameObject Spawn(GameObject[] prefabs, List<GameObject>[] pools, GameObject spawner, int idx)
    {
        GameObject select = null;

        // 비활성 게임 오브젝트가 있을 경우 select에 할당
        foreach (GameObject obj in pools[idx])
        {
            if (!obj.activeSelf)
            {
                select = obj;
                if (pools != enemyPools)
                    select.gameObject.transform.position = randomSpawnLocation.GetRandomSpawnPosition(); // 오브젝트 위치를 랜덤으로 설정
                else
                {
                    int random = UnityEngine.Random.Range(0, enemySpawnPoint.Count);
                    select.gameObject.transform.position = enemySpawnPoint[random].transform.position;
                }
                select.SetActive(true);
                break;
            }
        }

        // 비활성 오브젝트가 없을 경우 새로 생성하여 select에 할당
        if (!select)
        {
            if (pools == enemyPools)
            {
                int random = UnityEngine.Random.Range(0, enemySpawnPoint.Count);
                select = Instantiate(prefabs[idx], enemySpawnPoint[random].transform);

            }
            else
            {
                select = Instantiate(prefabs[idx], spawner.transform);
                select.gameObject.transform.position = randomSpawnLocation.GetRandomSpawnPosition(); // 오브젝트 위치를 랜덤으로 설정
            }
            pools[idx].Add(select);
        }

        return select;
    }

    // 오브젝트 전체 비활성화
    public void SetObjectActiveFalse(List<GameObject>[] pools)
    {
        foreach (List<GameObject> list in pools)
        {
            foreach (GameObject prefab in list)
            {
                prefab.SetActive(false);
            }
        }
    }
}
