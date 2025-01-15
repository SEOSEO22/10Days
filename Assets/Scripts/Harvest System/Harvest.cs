using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvest : MonoBehaviour
{
    public void HarvestObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
