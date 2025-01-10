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
    private List<GameObject> enemySpawnPoint = new List<GameObject>(); // 적 생성 위치 모음
    private RandomSpawner randomSpawnLocation; // 오브젝트 랜덤 생성 위치

    private void Awake()
    {
        #region 오브젝트 풀 초기화
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

    // 풀에서 비활성화 된 동물 오브젝트 찾기
    public GameObject AnimalSpawn(int index)
    {
        GameObject select = null;

        // 비활성 게임 오브젝트가 있을 경우 select에 할당
        foreach (GameObject obj in animalPools[index])
        {
            if (!obj.activeSelf)
            {
                select = obj;
                select.gameObject.transform.position = randomSpawnLocation.GetRandomSpawnPosition(); // 오브젝트 위치를 랜덤으로 설정
                select.SetActive(true);
                break;
            }
        }

        // 비활성 오브젝트가 없을 경우 새로 생성하여 select에 할당
        if (!select)
        {
            select = Instantiate(animalPrefabs[index], animalSpawner.transform);
            select.gameObject.transform.position = randomSpawnLocation.GetRandomSpawnPosition(); // 오브젝트 위치를 랜덤으로 설정
            animalPools[index].Add(select);
        }

        return select;
    }

    // 풀에서 비활성화 된 적 오브젝트 찾기
    public GameObject EnemySpawn(int index)
    {
        GameObject select = null;

        // 비활성 게임 오브젝트가 있을 경우 select에 할당
        foreach (GameObject obj in enemyPools[index])
        {
            if (!obj.activeSelf)
            {
                select = obj;
                select.SetActive(true);
                break;
            }
        }

        // 비활성 오브젝트가 없을 경우 새로 생성하여 select에 할당
        if (!select)
        {
            int random = Random.Range(0, enemySpawnPoint.Count);

            select = Instantiate(enemyPrefabs[index], enemySpawnPoint[random].transform);
            enemyPools[index].Add(select);
        }

        return select;
    }
}
