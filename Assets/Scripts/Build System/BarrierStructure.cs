using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BarrierStructure : MonoBehaviour
{
    [SerializeField] private BuildItemSO buildItemSO;   // �Ǽ� ������ ����

    private InventorySO playerInventory;
    private Dictionary<int, InventoryItem> currentInventory;
    private int level = 0;  // �Ǽ� ������ ����
    private Transform tileTransform; // �Ǽ��� Ÿ���� ��ġ

    // Info Panel�� ǥ���� ����
    public string Name => buildItemSO.buildingItem[level].name;
    public float Damage => buildItemSO.buildingItem[level].damage;
    public float Defecne => buildItemSO.buildingItem[level].defence;
    public float Rate => buildItemSO.buildingItem[level].rate;
    public float Range => buildItemSO.buildingItem[level].range;
    public int Level => level + 1;
    public int MaxLevel => buildItemSO.buildingItem.Length;
    public Transform TileTransform => tileTransform;

    public void SetLevel(int level)
    {
        this.level = level;
    }

    public void Setup(InventorySO playerInventory, Transform tileTransform)
    {
        this.playerInventory = playerInventory;
        this.tileTransform = tileTransform;
    }

    public void GetObjectAtPosition(Vector3 position, string tag, float radius = 0.1f)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag(tag)) // Ư�� �±� Ȯ��
            {
                tileTransform = col.gameObject.transform; // ù ��°�� ã�� �±� ��ġ ������Ʈ ��ȯ
                tileTransform.GetComponent<BuildingTile>().isStructureBuilding = true;
                return;
            }
        }
        tileTransform = null; // �ش� ��ġ�� �±װ� �´� ������Ʈ�� ������ null
    }

    // �Ǽ� ������Ʈ ���׷��̵�
    public bool Upgrade()
    {
        currentInventory = playerInventory.GetCurrentInventoryState();
        bool isPossible = false;

        foreach (KeyValuePair<ItemSO, int> cost in buildItemSO.buildingItem[level + 1].buildCost)
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

        if (isPossible)
        {
            level++;
            UseInventoryItem();
        }

        return isPossible;
    }

    // �ǹ� ���� �� �κ��丮 ������ ����
    public void UseInventoryItem()
    {
        foreach (KeyValuePair<ItemSO, int> cost in buildItemSO.buildingItem[level].buildCost)
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

    // ���� ��� ��ȯ
    public SerializableDictionary<ItemSO, int> CreateCost()
    {
        if (level + 1 >= buildItemSO.buildingItem.Length) return null;
        return buildItemSO.buildingItem[level + 1].buildCost;
    }
}
