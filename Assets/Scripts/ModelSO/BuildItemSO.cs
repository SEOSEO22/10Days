using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BuildItemSO : ScriptableObject
{
    public GameObject prefab; // 건설을 위한 프리팹
    public BuildingItem[] buildingItem;

    [System.Serializable]
    public struct BuildingItem
    {
        public string name; // 오브젝트 이름
        public float damage; // 공격력
        public float defence; // 방어력
        public float rate; // 공격 속도
        public float range; // 공격 범위
        public SerializableDictionary<ItemSO, int> buildCost; // 필요 재료
    }
}
