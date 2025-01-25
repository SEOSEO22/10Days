using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TurretSpawner : MonoBehaviour
{
    [SerializeField] BuildItemSO buildItemSO;
    [SerializeField] SpawnManager enemySpawner;
    [SerializeField] InventorySO playerInventory;
    [SerializeField] GameObject tile;
    [SerializeField] TurretDataViewer dataViewer;
    [SerializeField] GameObject buildUI; // �Ǽ� �ý����� �����ϴ� �г�

    private Dictionary<int, InventoryItem> currentInventory;
    public bool isOnBuildButton = false; // �Ǽ� ������Ʈ�� �����ߴ��� Ȯ��
    private GameObject followPrefabClone = null; // �ӽ� �ǹ� ��� �Ϸ� �� ������ ���� �����ϴ� ����

    private void Update()
    {
        if (isOnBuildButton == false && Input.GetKeyDown(KeyCode.Escape))
        {
            tile.SetActive(false);
            buildUI.SetActive(false);
        }

        if (tile.activeSelf == false)
        {
            isOnBuildButton = false;
            Destroy(followPrefabClone);
        }
    }

    public void ReadyToSpawn()
    {
        // ��ư�� �ߺ��ؼ� ������ �� �ӽ� �ǹ��� ��� �����Ǵ� ���� ���� ����
        if (isOnBuildButton == true || tile.activeSelf == false) return;

        isOnBuildButton = true;
        followPrefabClone = Instantiate(buildItemSO.followPrefab);
        dataViewer.OnPanel(buildItemSO);

        // �Ǽ��� ����� �� �ִ� �ڷ�ƾ �Լ�
        StartCoroutine(OnBuildCancleSystem());
    }

    // �Ǽ� ���� ���� ��ȯ (��ᰡ �������)
    public bool CheckBuildingPossibility()
    {
        currentInventory = playerInventory.GetCurrentInventoryState();
        bool isPossible = false;

        foreach (KeyValuePair<ItemSO, int> cost in buildItemSO.buildingItem[0].buildCost)
        {
            int count = 0;

            foreach (KeyValuePair<int, InventoryItem> inventory in currentInventory)
            {
                if (cost.Key == inventory.Value.item)
                {
                    count += inventory.Value.quantity;
                }

                if (count < cost.Value) isPossible = false;
                else isPossible = true;
            }

            if (isPossible == false) break;
        }

        return isPossible;
    }

    // ��ž ���� �� �κ��丮 ������ ����
    public void UseInventoryItem()
    {
        foreach (KeyValuePair<ItemSO, int> cost in buildItemSO.buildingItem[0].buildCost)
        {
            int count = cost.Value;

            foreach (KeyValuePair<int, InventoryItem> inventory in currentInventory)
            {
                if (cost.Key == inventory.Value.item)
                {
                    playerInventory.RemoveItem(inventory.Key, count);
                    count -= inventory.Value.quantity;
                }

                if (count <= 0) break;
            }
        }
    }

    // ��ž ����
    public void SpawnTurret(Transform tileTranform)
    {
        if (isOnBuildButton == false) return;

        if (CheckBuildingPossibility() == false)
        {
            // ��� ���� �޼���
            return;
        }

        Turret turret = tileTranform.GetComponent<Turret>();

        if (turret.isTurretBuilding == true) return;

        isOnBuildButton = false;
        turret.isTurretBuilding = true;
        UseInventoryItem();

        // Build Turret on Selected Tile
        Vector3 position = tileTranform.position + Vector3.back;
        GameObject clone = Instantiate(buildItemSO.prefab, position, Quaternion.identity, transform);
        clone.GetComponent<TurretWeapon>().Setup(enemySpawner, playerInventory, tileTranform);

        // Ÿ�� �Ǽ��� ����� �� �ִ� �ڷ�ƾ �Լ� ����
        StopCoroutine(OnBuildCancleSystem());

        Destroy(followPrefabClone);
    }

    private IEnumerator OnBuildCancleSystem()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(1))
            {
                isOnBuildButton = false;
                dataViewer.OffPanel();
                Destroy(followPrefabClone);
                break;
            }

            yield return null;
        }
    }
}
