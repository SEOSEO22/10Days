using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpResources : MonoBehaviour
{
    [field : SerializeField] public Inventory inventory { get; private set; }

    // 땅에 떨어진 리소스 오브젝트를 자동 수확하는 방식으로 변경 예정
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ResourcePickUp pickUp = collision.gameObject.GetComponent<ResourcePickUp>();

        if (pickUp)
        {
            inventory.AddResources(pickUp.resourceType, 1);
            Destroy(collision.gameObject);
        }
    }
}
