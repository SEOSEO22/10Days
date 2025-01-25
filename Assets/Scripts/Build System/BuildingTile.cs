using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTile : MonoBehaviour
{
    public bool isStructureBuilding { set; get; }

    private void Awake()
    {
        isStructureBuilding = false;
    }
}
