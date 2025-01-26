using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FindTarget : MonoBehaviour
{
    public GameObject FindClosetTarget(GameObject[] targets)
    {
        // ���� ����� Ÿ�ٰ��� �Ÿ�
        float closetDistSqr = Mathf.Infinity;
        GameObject target = null;

        foreach(GameObject entity in targets)
        {
            float distance = (entity.transform.position - transform.position).sqrMagnitude;

            if (distance < closetDistSqr)
            {
                closetDistSqr = distance;
                target = entity;
            }
        }

        return target;
    }
}
