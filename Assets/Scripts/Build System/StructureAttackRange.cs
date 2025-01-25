using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureAttackRange : MonoBehaviour
{
    public void OnAttackRange(Vector3 position, float range)
    {
        gameObject.SetActive(true);

        // Size of Attack Range
        float diameter = range * 2f;
        transform.localScale = Vector3.one * diameter;

        // Location of Attack Range
        transform.position = position;
    }

    public void OffAttackRange()
    {
        gameObject.SetActive(false);
    }
}
