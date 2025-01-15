using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Resource", menuName = "HarvestSystem/Resource")]
public class Resource : ScriptableObject
{
    [field : SerializeField] public string DisplayName { get; private set; }
    [field : SerializeField] public Sprite Icon { get; private set; }
    [field : SerializeField] public string Description { get; private set; }
    [field : SerializeField] public float Value { get; private set; }
}
