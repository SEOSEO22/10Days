using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponState { SearchTarget = 0, AttackToTarget }

public class TurretWeapon : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float attackRate = 2f; // ���� �ӵ�
    [SerializeField] private float attackRange = 10f; // ���� ����

    private GameObject projectileSpawner;
    private List<GameObject> projectilePool;

    private WeaponState weaponState = WeaponState.SearchTarget;
    private Transform attackTarget = null;
    private SpawnManager enemySpawner;

    private void Awake()
    {
        // ������Ʈ Ǯ�� �ʱ�ȭ
        projectileSpawner = GameObject.Find("Projectile Spawner");
        projectilePool = new List<GameObject>();
    }

    public void Setup(SpawnManager enemySpawner)
    {
        this.enemySpawner = enemySpawner;

        // ���� ���¸� Ÿ�� ��Ī���� ����
        ChangeState(WeaponState.SearchTarget);
    }

    public void ChangeState(WeaponState newState)
    {
        StopCoroutine(weaponState.ToString());
        weaponState = newState;
        StartCoroutine(weaponState.ToString());
    }

    private void Update()
    {
        if (attackTarget != null && attackTarget.gameObject.activeSelf) RotateToTarget();

        // �������� ������ �����Ͽ� �� �׸���
        Vector3 start = transform.position;
        Vector3 end = transform.position + transform.right * attackRange;

        Debug.DrawLine(start, end, Color.red);
    }

    private void RotateToTarget()
    {
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;

        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, degree);
    }

    private IEnumerator SearchTarget()
    {
        while (true)
        {
            float closestDistSqr = Mathf.Infinity;

            foreach (List<GameObject> enemyList in enemySpawner.enemyPools)
            {
                foreach (GameObject enemy in enemyList)
                {
                    if (enemy.activeSelf == false) continue;

                    float distance = Vector3.Distance(enemy.transform.position, transform.position);

                    if (distance <= attackRange && distance <= closestDistSqr)
                    {
                        closestDistSqr = distance;
                        attackTarget = enemy.transform;
                    }
                }
            }

            if (attackTarget != null && attackTarget.gameObject.activeSelf) ChangeState(WeaponState.AttackToTarget);

            yield return null;
        }
    }

    private IEnumerator AttackToTarget()
    {
        while (true)
        {
            if (attackTarget == null || !attackTarget.gameObject.activeSelf)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            float distance = Vector3.Distance(attackTarget.position, transform.position);

            if (distance > attackRange)
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            yield return new WaitForSeconds(attackRate);

            SpawnProjectile();
        }
    }

    private void SpawnProjectile()
    {
        Spawn();
    }

    // ������Ʈ Ǯ������ Projectile ����
    private GameObject Spawn()
    {
        GameObject select = null;

        // ��Ȱ�� ���� ������Ʈ�� ���� ��� select�� �Ҵ�
        foreach (GameObject obj in projectilePool)
        {
            if (!obj.activeSelf)
            {
                select = obj;
                select.gameObject.transform.position = spawnPoint.position;
                select.SetActive(true);
                select.GetComponent<Projectile>().Setup(attackTarget);
                break;
            }
        }

        // ��Ȱ�� ������Ʈ�� ���� ��� ���� �����Ͽ� select�� �Ҵ�
        if (!select)
        {
            select = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity, projectileSpawner.transform);
            select.GetComponent<Projectile>().Setup(attackTarget);

            projectilePool.Add(select);
        }

        return select;
    }
}
