using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpResources : MonoBehaviour
{
    [SerializeField] private InventorySO inventoryData;

    // 땅에 떨어진 리소스 오브젝트를 자동 수확하는 방식으로 변경 예정
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
