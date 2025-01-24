using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BuildItemSO : ScriptableObject
{
    public GameObject prefab; // �Ǽ��� ���� ������
    public BuildingItem[] buildingItem;

    [System.Serializable]
    public struct BuildingItem
    {
        public string name; // ������Ʈ �̸�
        public float damage; // ���ݷ�
        public float defence; // ����
        public float rate; // ���� �ӵ�
        public float range; // ���� ����
        public SerializableDictionary<ItemSO, int> buildCost; // �ʿ� ���
    }
}
