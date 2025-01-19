using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu(fileName = "Resource", menuName = "HarvestSystem/Resource")]
    public class ItemSO : ScriptableObject
    {
        [field: SerializeField] public bool IsStackable { get; private set; }

        public int ID => GetInstanceID();

        [field: SerializeField] public int MaxStackSize { get; private set; } = 10;

        [field: SerializeField] public string DisplayName { get; private set; }

        [field: SerializeField] public Sprite Icon { get; private set; }

        [field: SerializeField]
        [field: TextArea]
        public string Description { get; private set; }
    }
}