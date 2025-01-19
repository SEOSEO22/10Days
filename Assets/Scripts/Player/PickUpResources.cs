using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpResources : MonoBehaviour
{
    [SerializeField] private InventorySO inventoryData;

    // ���� ������ ���ҽ� ������Ʈ�� �ڵ� ��Ȯ�ϴ� ������� ���� ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ResourcePickUp item = collision.GetComponent<ResourcePickUp>();

        if (item)
        {
            int reminder = inventoryData.AddItem(item.InventoryItem, item.Quantity);

            if (reminder == 0) item.DestroyItem();
            else item.Quantity = reminder;
        }
    }
}
