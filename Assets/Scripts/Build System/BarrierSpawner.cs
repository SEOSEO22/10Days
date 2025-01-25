using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierSpawner : MonoBehaviour
{
    [SerializeField] BuildItemSO buildItemSO;
    [SerializeField] InventorySO playerInventory;
    [SerializeField] GameObject tile;
    [SerializeField] StructureDataViewer dataViewer;
    [SerializeField] GameObject[] buildUI; // �Ǽ� �ý����� �����ϴ� �г�

    private Dictionary<int, InventoryItem> currentInventory;
    public bool isOnBuildButton = false; // �Ǽ� ������Ʈ�� �����ߴ��� Ȯ��
    private GameObject followPrefabClone = null; // �ӽ� �ǹ� ��� �Ϸ� �� ������ ���� �����ϴ� ����

    private void Update()
    {
        if (isOnBuildButton == false && Input.GetKeyDown(KeyCode.Escape))
        {
            tile.SetActive(false);

            foreach (GameObject ui in buildUI)
            {
                ui.SetActive(false);
            }
        }

        if (tile.activeSelf == false)
        {
            isOnBuildButton = false;
            GameManager.Instance.isStructureSelected = false;
            Destroy(followPrefabClone);
        }
    }

    public void ReadyToSpawn()
    {
        // ��ư�� �ߺ��ؼ� ������ �� �ӽ� �ǹ��� ��� �����Ǵ� ���� ���� ����
        if (isOnBuildButton == true || tile.activeSelf == false) return;
        if (GameManager.Instance.isStructureSelected == true) return;

        isOnBuildButton = true;
        GameManager.Instance.isStructureSelected = true;
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

    // �ǹ� ���� �� �κ��丮 ������ ����
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

    // �ǹ� ����
    public void SpawnBarrier(Transform tileTranform)
    {
        if (isOnBuildButton == false) return;

        if (CheckBuildingPossibility() == false)
        {
            // ��� ���� �޼���
            return;
        }

        BuildingTile barrier = tileTranform.GetComponent<BuildingTile>();

        if (barrier.isStructureBuilding == true) return;

        isOnBuildButton = false;
        GameManager.Instance.isStructureSelected = false;
        barrier.isStructureBuilding = true;
        UseInventoryItem();

        // Build Turret on Selected Tile
        Vector3 position = tileTranform.position + Vector3.back;
        GameObject clone = Instantiate(buildItemSO.prefab, position, Quaternion.identity, transform);
        clone.GetComponent<BarrierStructure>().Setup(playerInventory, tileTranform);

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
                GameManager.Instance.isStructureSelected = false;
                dataViewer.OffPanel();
                Destroy(followPrefabClone);
                break;
            }

            yield return null;
        }
    }
}
