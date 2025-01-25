using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class TurretDataViewer : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI defenceText;
    [SerializeField] private TurretAttackRange turretAttackRange;

    [Header("Buttons")]
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private TextMeshProUGUI upgradeButtonText;

    [Header("For Cost Item List")]
    [SerializeField] private ShowCostItem itemPrefab;
    [SerializeField] private RectTransform costPanel;

    List<ShowCostItem> listOfCostItems = new List<ShowCostItem>();
    private TurretWeapon currentTurret;

    private void Awake()
    {
        OffPanel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OffPanel();
        }
    }

    public void OnPanel(Transform towerWeapon)
    {
        currentTurret = towerWeapon.GetComponent<TurretWeapon>();

        // Turret Info Panel On
        gameObject.SetActive(true);

        // �ͷ� ���� ����
        UpdateTurretData();

        // �ͷ� ���� ���� ǥ��
        turretAttackRange.OnAttackRange(currentTurret.transform.position, currentTurret.Range);
    }

    public void OnPanel(BuildItemSO buildItem)
    {
        // Turret Info Panel On
        gameObject.SetActive(true);

        // �ͷ� ���� ����
        UpdateTurretData(buildItem);
    }

    public void OffPanel()
    {
        // Turret Info Panel Off
        gameObject.SetActive(false);

        // ���� ���� ǥ�� Off
        turretAttackRange.OffAttackRange();
    }

    private void UpdateTurretData()
    {
        nameText.text = currentTurret.Name;
        damageText.text = "���ݷ� : " + ((int)currentTurret.Damage).ToString("D2");
        defenceText.text = "���� : " + ((int)currentTurret.Defecne).ToString("D2");

        upgradeButton.interactable = currentTurret.Level < currentTurret.MaxLevel ? true : false;
        deleteButton.interactable = true;
    }

    // ��ž ù ���� �� ������ ������Ʈ
    private void UpdateTurretData(BuildItemSO buildItem)
    {
        nameText.text = buildItem.buildingItem[0].name;
        damageText.text = "���ݷ� : " + ((int)buildItem.buildingItem[0].damage).ToString("D2");
        defenceText.text = "���� : " + ((int)buildItem.buildingItem[0].defence).ToString("D2");

        // ��ž ù ���� �� ��ȭ/���� ��ư ��Ȱ��ȭ
        upgradeButton.interactable = false;
        deleteButton.interactable = false;
    }

    public void OnClickUpgrade()
    {
        bool isSuccess = currentTurret.Upgrade();

        if (isSuccess == true)
        {
            UpdateTurretData();
            turretAttackRange.OnAttackRange(currentTurret.transform.position, currentTurret.Range);
        }
        else
        {
            // ��� ���� �޼���
        }
    }

    // �Ǽ� ��� ������ ����Ʈ �ʱ�ȭ
    public void InitializeCostItem(int costItemSize)
    {
        // �κ��丮 ���� �ʱ�ȭ
        for (int i = 0; i < costItemSize; i++)
        {
            ShowCostItem costItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            costItem.transform.SetParent(costPanel);
            listOfCostItems.Add(costItem);
        }
    }
}
