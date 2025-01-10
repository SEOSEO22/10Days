using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject animalSpawner;
    [SerializeField] private GameObject enemySpawner;

    public GameObject[] animalPrefabs;
    public GameObject[] enemyPrefabs;

    private List<GameObject>[] animalPools;
    private List<GameObject>[] enemyPools;
    private List<GameObject> enemySpawnPoint = new List<GameObject>(); // �� ���� ��ġ ����
    private RandomSpawner randomSpawnLocation; // ������Ʈ ���� ���� ��ġ

    private void Awake()
    {
        #region ������Ʈ Ǯ �ʱ�ȭ
        animalPools = new List<GameObject>[animalPrefabs.Length];
        enemyPools = new List<GameObject>[enemyPrefabs.Length];

        for (int i = 0; i < animalPrefabs.Length; i++)
        {
            animalPools[i] = new List<GameObject>();
        }

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            enemyPools[i] = new List<GameObject>();
        }
        #endregion

        randomSpawnLocation = GetComponent<RandomSpawner>();

        for (int i = 0; i < enemySpawner.transform.childCount; i++)
        {
            enemySpawnPoint.Add(enemySpawner.transform.GetChild(i).gameObject);
        }
    }

    // Ǯ���� ��Ȱ��ȭ �� ���� ������Ʈ ã��
    public GameObject AnimalSpawn(int index)
    {
        GameObject select = null;

        // ��Ȱ�� ���� ������Ʈ�� ���� ��� select�� �Ҵ�
        foreach (GameObject obj in animalPools[index])
        {
            if (!obj.activeSelf)
            {
                select = obj;
                select.gameObject.transform.position = randomSpawnLocation.GetRandomSpawnPosition(); // ������Ʈ ��ġ�� �������� ����
                select.SetActive(true);
                break;
            }
        }

        // ��Ȱ�� ������Ʈ�� ���� ��� ���� �����Ͽ� select�� �Ҵ�
        if (!select)
        {
            select = Instantiate(animalPrefabs[index], animalSpawner.transform);
            select.gameObject.transform.position = randomSpawnLocation.GetRandomSpawnPosition(); // ������Ʈ ��ġ�� �������� ����
            animalPools[index].Add(select);
        }

        return select;
    }

    // Ǯ���� ��Ȱ��ȭ �� �� ������Ʈ ã��
    public GameObject EnemySpawn(int index)
    {
        GameObject select = null;

        // ��Ȱ�� ���� ������Ʈ�� ���� ��� select�� �Ҵ�
        foreach (GameObject obj in enemyPools[index])
        {
            if (!obj.activeSelf)
            {
                select = obj;
                select.SetActive(true);
                break;
            }
        }

        // ��Ȱ�� ������Ʈ�� ���� ��� ���� �����Ͽ� select�� �Ҵ�
        if (!select)
        {
            int random = Random.Range(0, enemySpawnPoint.Count);

            select = Instantiate(enemyPrefabs[index], enemySpawnPoint[random].transform);
            enemyPools[index].Add(select);
        }

        return select;
    }
}
