using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] SpawnManager enemySpawner;

    public void SpawnTurret(Transform tileTranform)
    {
        Turret turret = tileTranform.GetComponent<Turret>();

        if (turret.isTurretBuilding == true) return;

        turret.isTurretBuilding = true;
        GameObject clone = Instantiate(prefab, tileTranform.position, Quaternion.identity, transform);
        clone.GetComponent<TurretWeapon>().Setup(enemySpawner);
    }
}
