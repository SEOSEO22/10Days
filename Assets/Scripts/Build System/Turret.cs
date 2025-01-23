using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public bool isTurretBuilding { set; get; }

    private void Awake()
    {
        isTurretBuilding = false;
    }
}
