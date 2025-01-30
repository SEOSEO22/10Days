using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretSpawner : MonoBehaviour
{
    [SerializeField] BuildItemSO buildItemSO;
    [SerializeField] SpawnManager enemySpawner;
    [SerializeField] InventorySO playerInventory;
    [SerializeField] GameObject tile;
    [SerializeField] StructureDataViewer dataViewer;
    [SerializeField] GameObject[] buildUI; // 건설 시스템을 관리하는 패널

    private Dictionary<int, InventoryItem> currentInventory;
    public bool isOnBuildButton = false; // 건설 오브젝트를 선택했는지 확인
    private GameObject followPrefabClone = null; // 임시 건물 사용 완료 시 삭제를 위해 저장하는 변수

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

        DataManager.Instance.currentGameData.turrets.SetTurretData(GetComponentsInChildren<TurretStructure>().ToList());
    }

    public void ReadyToSpawn()
    {
        // 버튼을 중복해서 눌렀을 때 임시 건물이 계속 생성되는 것을 막기 위함
        if (isOnBuildButton == true || tile.activeSelf == false) return;
        if (GameManager.Instance.isStructureSelected == true) return;

        isOnBuildButton = true;
        GameManager.Instance.isStructureSelected = true;
        followPrefabClone = Instantiate(buildItemSO.followPrefab);
        dataViewer.OnPanel(buildItemSO);

        // 건설을 취소할 수 있는 코루틴 함수
        StartCoroutine(OnBuildCancleSystem());
    }

    // 건설 가능 여부 반환 (재료가 충분한지)
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

    // 포탑 생성 시 인벤토리 아이템 감소
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

    // 포탑 생성
    public void SpawnTurret(Transform tileTranform)
    {
        if (isOnBuildButton == false) return;
        if (GameManager.Instance.GetTimeInfo() == TimeInfo.night) return;

        if (CheckBuildingPossibility() == false)
        {
            // 재료 부족 메세지
            return;
        }

        BuildingTile turret = tileTranform.GetComponent<BuildingTile>();

        if (turret.isStructureBuilding == true) return;

        isOnBuildButton = false;
        GameManager.Instance.isStructureSelected = false;
        turret.isStructureBuilding = true;
        UseInventoryItem();

        // Build Turret on Selected Tile
        Vector3 position = tileTranform.position + Vector3.back;
        GameObject clone = Instantiate(buildItemSO.prefab, position, Quaternion.identity, transform);
        clone.GetComponent<TurretStructure>().Setup(enemySpawner, playerInventory, tileTranform);

        // 타워 건설을 취소할 수 있는 코루틴 함수 중지
        StopCoroutine(OnBuildCancleSystem());

        Destroy(followPrefabClone);
    }

    // 이어 하기 시 이미 생성된 포탑 재생성
    public void ReSpawnTurret(Transform tileTranform)
    {
        BuildingTile turret = tileTranform.GetComponent<BuildingTile>();

        if (turret.isStructureBuilding == true) return;

        isOnBuildButton = false;
        GameManager.Instance.isStructureSelected = false;
        turret.isStructureBuilding = true;
        UseInventoryItem();

        // Build Turret on Selected Tile
        Vector3 position = tileTranform.position + Vector3.back;
        GameObject clone = Instantiate(buildItemSO.prefab, position, Quaternion.identity, transform);
        clone.GetComponent<TurretStructure>().Setup(enemySpawner, playerInventory, tileTranform);

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
