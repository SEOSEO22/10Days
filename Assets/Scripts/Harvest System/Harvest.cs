using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvest : MonoBehaviour
{
    [SerializeField] private int emitCount = 1;
    private ParticleSystem ps;

    private void Start()
    {
        ps = GetComponentInParent<ParticleSystem>();
    }

    public void SetHarvestParticle()
    {
        ps.Emit(emitCount);
    }

    public void HarvestObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
