using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Harvest : MonoBehaviour
{
    [SerializeField] private int emitCount = 1;
    private Slider hp;
    private ParticleSystem ps;

    private void Start()
    {
        ps = GetComponentInParent<ParticleSystem>();
        hp = GetComponentInChildren<Slider>();
    }

    public void SetHarvestParticle()
    {
        if (hp != null && hp.value <= 0.01f)
            ps.Emit(emitCount);
    }

    public void HarvestObject(GameObject obj)
    {
        if (obj.CompareTag("Animal"))
        {
            obj.GetComponent<GameObjectEmitter>().EmitDropItem(obj);
        }
        obj.SetActive(false);
    }
}
