using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePickUp : MonoBehaviour
{
    [field : SerializeField] public Resource resourceType { get; private set; }
}
