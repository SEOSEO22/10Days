using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceInventory : MonoBehaviour
{
    [field : SerializeField] private SerializableDictionary<Resource, int> resources { get; set; }

    public int GetResourceCount(Resource type)
    {
        if (resources.TryGetValue(type, out int currentCount))
        {
            return currentCount;
        }
        else return 0;
    }

    public int AddResources(Resource type, int count)
    {
        if (resources.TryGetValue(type, out int currentCount))
        {
            return resources[type] += count;

        }
        else
        {
            resources.Add(type, count);
            return count;
        }
    }
}
