using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnMachineParts : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    private GameObject currentPart = null;
    private RandomSpawner randomSpawner;

    private void Start()
    {
        randomSpawner = GetComponent<RandomSpawner>();
    }

    public void SpawnPart()
    {
        GameObject part = Instantiate(prefab);
        part.transform.position = randomSpawner.GetRandomSpawnPosition();

        currentPart = part;
    }

    public void DestroyPart()
    {
        if (currentPart == null) return;

        Destroy(currentPart.gameObject);
        currentPart = null;
    }
}
