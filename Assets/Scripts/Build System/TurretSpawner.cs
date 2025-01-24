using Inventory.Model;
using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TurretSpawner : MonoBehaviour
{
    [SerializeField] BuildItemSO buildItemSO;
    [SerializeField] SpawnManager enemySpawner;
    [SerializeField] InventorySO playerInventory;
    [SerializeField] GameObject tile;

    private Dictionary<int, InventoryItem> currentInventory;

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
        if (CheckBuildingPossibility() == false) return;

        Turret turret = tileTranform.GetComponent<Turret>();

        if (turret.isTurretBuilding == true) return;

        turret.isTurretBuilding = true;
        UseInventoryItem();

        // Build Turret on Selected Tile
        Vector3 position = tileTranform.position + Vector3.back;
        GameObject clone = Instantiate(buildItemSO.prefab, position, Quaternion.identity, transform);
        clone.GetComponent<TurretWeapon>().Setup(enemySpawner, playerInventory);

        tile.SetActive(false);
    }
}
