using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnMachineParts : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private RandomSpawner randomSpawner;

    private GameObject currentPart = null;

    private void Start()
    {
        randomSpawner = GetComponent<RandomSpawner>();
    }

    public void SpawnPart()
    {
        GameObject part = Instantiate(prefab, randomSpawner.GetRandomSpawnPosition(), Quaternion.identity);

        currentPart = part;
    }

    public void DestroyPart()
    {
        if (currentPart == null) return;

        Destroy(currentPart.gameObject);
        currentPart = null;
    }
}
