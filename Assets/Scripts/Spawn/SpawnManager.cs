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
    private List<GameObject> enemySpawnPoint = new List<GameObject>(); // �� ���� ��ġ ����
    private RandomSpawner randomSpawnLocation; // ������Ʈ ���� ���� ��ġ

    private void Awake()
    {
        #region ������Ʈ Ǯ �ʱ�ȭ
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

    public GameObject AnimalSpawn(int index)
    {
        return Spawn(animalPrefabs, animalPools, animalSpawner, index);
    }

    public GameObject EnemySpawn(int index)
    {
        return Spawn(enemyPrefabs, enemyPools, enemySpawner, index);
    }

    public GameObject ResourceSpawn(int index)
    {
        return Spawn(resourcePrefabs, resourcePools, resourceSpawner, index);
    }

    // ���� ������Ʈ ��ü ��Ȱ��ȭ
    public void SetAnimalActiveFalse()
    {
        SetObjectActiveFalse(animalPools);
    }

    // �ڿ� ������Ʈ ��ü ��Ȱ��ȭ
    public void SetResourceActiveFalse()
    {
        SetObjectActiveFalse(resourcePools);
    }

    // ������Ʈ Ǯ������ ���� ������Ʈ ����
    private GameObject Spawn(GameObject[] prefabs, List<GameObject>[] pools, GameObject spawner, int idx)
    {
        GameObject select = null;

        // ��Ȱ�� ���� ������Ʈ�� ���� ��� select�� �Ҵ�
        foreach (GameObject obj in pools[idx])
        {
            if (!obj.activeSelf)
            {
                select = obj;
                if (pools != enemyPools)
                    select.gameObject.transform.position = randomSpawnLocation.GetRandomSpawnPosition(); // ������Ʈ ��ġ�� �������� ����
                else
                {
                    int random = UnityEngine.Random.Range(0, enemySpawnPoint.Count);
                    select.gameObject.transform.position = enemySpawnPoint[random].transform.position;
                }
                select.SetActive(true);
                break;
            }
        }

        // ��Ȱ�� ������Ʈ�� ���� ��� ���� �����Ͽ� select�� �Ҵ�
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
                select.gameObject.transform.position = randomSpawnLocation.GetRandomSpawnPosition(); // ������Ʈ ��ġ�� �������� ����
            }
            pools[idx].Add(select);
        }

        return select;
    }

    // ���� ������Ʈ ��ü ��Ȱ��ȭ
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
