using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponState { SearchTarget = 0, AttackToTarget }

public class TurretStructure : MonoBehaviour
{
    [SerializeField] private BuildItemSO buildItemSO;   // �Ǽ� ������ ����
    [SerializeField] private GameObject projectilePrefab;   // �߻�ü ������
    [SerializeField] private Transform spawnPoint;  // �߻�ü ���� ��ġ

    private GameObject projectileSpawner;
    private List<GameObject> projectilePool;

    private WeaponState weaponState = WeaponState.SearchTarget;
    private Transform attackTarget = null;
    private SpawnManager enemySpawner;
    private InventorySO playerInventory;
    private Dictionary<int, InventoryItem> currentInventory;
    private int level = 0;  // �Ǽ� ������ ����
    private Transform tileTransform; // �Ǽ��� Ÿ���� ��ġ

    // Info Panel�� ǥ���� ����
    public string Name => buildItemSO.buildingItem[level].name;
    public float Damage => buildItemSO.buildingItem[level].damage;
    public float Defecne => buildItemSO.buildingItem[level].defence;
    public float Rate => buildItemSO.buildingItem[level].rate;
    public float Range => buildItemSO.buildingItem[level].range;
    public int Level => level + 1;
    public int MaxLevel => buildItemSO.buildingItem.Length;
    public Transform TileTransform => tileTransform;

    private void Awake()
    {
        // ������Ʈ Ǯ�� �ʱ�ȭ
        projectileSpawner = GameObject.Find("Projectile Spawner");
        projectilePool = new List<GameObject>();
    }

    public void SetLevel(int level)
    {
        this.level = level;
    }

    public void Setup(SpawnManager enemySpawner, InventorySO playerInventory, Transform tileTransform)
    {
        this.enemySpawner = enemySpawner;
        this.playerInventory = playerInventory;
        this.tileTransform = tileTransform;

        // ���� ���¸� Ÿ�� ��Ī���� ����
        ChangeState(WeaponState.SearchTarget);
    }

    public void GetObjectAtPosition(Vector3 position, string tag, float radius = 0.1f)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag(tag)) // Ư�� �±� Ȯ��
            {
                tileTransform = col.gameObject.transform; // ù ��°�� ã�� �±� ��ġ ������Ʈ ��ȯ
                tileTransform.GetComponent<BuildingTile>().isStructureBuilding = true;
                return;
            }
        }
        tileTransform = null; // �ش� ��ġ�� �±װ� �´� ������Ʈ�� ������ null
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
        Vector3 end = transform.position + transform.right * buildItemSO.buildingItem[level].range;

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

                    if (distance <= buildItemSO.buildingItem[level].range && distance <= closestDistSqr)
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

            if (distance > buildItemSO.buildingItem[level].range)
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            yield return new WaitForSeconds(buildItemSO.buildingItem[level].rate);

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
                select.GetComponent<Projectile>().Setup(attackTarget, buildItemSO.buildingItem[level].damage);
                break;
            }
        }

        // ��Ȱ�� ������Ʈ�� ���� ��� ���� �����Ͽ� select�� �Ҵ�
        if (!select)
        {
            select = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity, projectileSpawner.transform);
            select.GetComponent<Projectile>().Setup(attackTarget, buildItemSO.buildingItem[level].damage);

            projectilePool.Add(select);
        }

        return select;
    }

    // �Ǽ� ������Ʈ ���׷��̵�
    public bool Upgrade()
    {
        currentInventory = playerInventory.GetCurrentInventoryState();
        bool isPossible = false;

        foreach (KeyValuePair<ItemSO, int> cost in buildItemSO.buildingItem[level + 1].buildCost)
        {
            int count = 0;

            foreach (KeyValuePair<int, InventoryItem> inventory in currentInventory)
            {
                if (cost.Key == inventory.Value.item)
                {
                    count += inventory.Value.quantity;
                }

                if (count < cost.Value) isPossible = false;
                else isPossible = true;
            }

            if (isPossible == false) break;
        }

        if (isPossible)
        {
            level++;
            UseInventoryItem();
        }

        return isPossible;
    }

    // ��ž ���� �� �κ��丮 ������ ����
    public void UseInventoryItem()
    {
        foreach (KeyValuePair<ItemSO, int> cost in buildItemSO.buildingItem[level].buildCost)
        {
            int count = cost.Value;

            foreach (KeyValuePair<int, InventoryItem> inventory in currentInventory)
            {
                if (cost.Key == inventory.Value.item)
                {
                    playerInventory.RemoveItem(inventory.Key, count);
                    count -= inventory.Value.quantity;
                }

                if (count <= 0) break;
            }
        }
    }
}
