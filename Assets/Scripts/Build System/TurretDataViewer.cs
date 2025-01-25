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

        // 터렛 정보 갱신
        UpdateTurretData();

        // 터렛 공격 범위 표시
        turretAttackRange.OnAttackRange(currentTurret.transform.position, currentTurret.Range);
    }

    public void OnPanel(BuildItemSO buildItem)
    {
        // Turret Info Panel On
        gameObject.SetActive(true);

        // 터렛 정보 갱신
        UpdateTurretData(buildItem);
    }

    public void OffPanel()
    {
        // Turret Info Panel Off
        gameObject.SetActive(false);

        // 공격 범위 표시 Off
        turretAttackRange.OffAttackRange();
    }

    private void UpdateTurretData()
    {
        nameText.text = currentTurret.Name;
        damageText.text = "공격력 : " + ((int)currentTurret.Damage).ToString("D2");
        defenceText.text = "방어력 : " + ((int)currentTurret.Defecne).ToString("D2");

        upgradeButton.interactable = currentTurret.Level < currentTurret.MaxLevel ? true : false;
        deleteButton.interactable = true;
    }

    // 포탑 첫 생성 시 데이터 업데이트
    private void UpdateTurretData(BuildItemSO buildItem)
    {
        nameText.text = buildItem.buildingItem[0].name;
        damageText.text = "공격력 : " + ((int)buildItem.buildingItem[0].damage).ToString("D2");
        defenceText.text = "방어력 : " + ((int)buildItem.buildingItem[0].defence).ToString("D2");

        // 포탑 첫 생성 시 강화/삭제 버튼 비활성화
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
            // 재료 부족 메세지
        }
    }

    // 건설 비용 아이템 리스트 초기화
    public void InitializeCostItem(int costItemSize)
    {
        // 인벤토리 슬롯 초기화
        for (int i = 0; i < costItemSize; i++)
        {
            ShowCostItem costItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            costItem.transform.SetParent(costPanel);
            listOfCostItems.Add(costItem);
        }
    }
}
