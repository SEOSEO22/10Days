using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpResources : MonoBehaviour
{
    [field : SerializeField] public Inventory inventory { get; private set; }

    // ���� ������ ���ҽ� ������Ʈ�� �ڵ� ��Ȯ�ϴ� ������� ���� ����
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
