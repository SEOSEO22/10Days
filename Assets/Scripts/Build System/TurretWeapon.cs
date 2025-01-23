using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponState { SearchTarget = 0, AttackToTarget }

public class TurretWeapon : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float attackRate = 2f; // 공격 속도
    [SerializeField] private float attackRange = 10f; // 공격 범위

    private GameObject projectileSpawner;
    private List<GameObject> projectilePool;

    private WeaponState weaponState = WeaponState.SearchTarget;
    private Transform attackTarget = null;
    private SpawnManager enemySpawner;

    private void Awake()
    {
        // 오브젝트 풀링 초기화
        projectileSpawner = GameObject.Find("Projectile Spawner");
        projectilePool = new List<GameObject>();
    }

    public void Setup(SpawnManager enemySpawner)
    {
        this.enemySpawner = enemySpawner;

        // 최초 상태를 타겟 서칭으로 설정
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

        // 시작점과 끝점을 지정하여 선 그리기
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

    // 오브젝트 풀링으로 Projectile 생성
    private GameObject Spawn()
    {
        GameObject select = null;

        // 비활성 게임 오브젝트가 있을 경우 select에 할당
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

        // 비활성 오브젝트가 없을 경우 새로 생성하여 select에 할당
        if (!select)
        {
            select = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity, projectileSpawner.transform);
            select.GetComponent<Projectile>().Setup(attackTarget);

            projectilePool.Add(select);
        }

        return select;
    }
}
