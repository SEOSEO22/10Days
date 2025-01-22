using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectEmitter : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    private ParticleSystem ps;
    private List<ParticleSystem.Particle> exitParticles = new();

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnParticleTrigger()
    {
        ps.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exitParticles);

        foreach (ParticleSystem.Particle p in exitParticles)
        {
            GameObject spawnedObject = Instantiate(prefab);
            spawnedObject.transform.position = p.position;
        }
    }

    public void EmitDropItem(GameObject obj)
    {
        GameObject spawnedObject = Instantiate(prefab);
        spawnedObject.transform.position = obj.transform.position;
    }
}
