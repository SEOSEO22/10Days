using Inventory.Model;
using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class StructureDataViewer : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI defenceText;
    [SerializeField] private Image costItem_1_Img;
    [SerializeField] private TextMeshProUGUI costItem_1_Text;
    [SerializeField] private Image costItem_2_Img;
    [SerializeField] private TextMeshProUGUI costItem_2_Text;
    [SerializeField] private StructureAttackRange structureAttackRange;

    [Header("Buttons")]
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button deleteButton;

    [Space]
    [SerializeField] private InventorySO playerInventory;

    private TurretStructure currentTurret;
    private BarrierStructure currentBarrier;
    private Dictionary<int, InventoryItem> currentInventory;

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

    public void InitCurrentStructure()
    {
        currentBarrier = null;
        currentTurret = null;
        structureAttackRange.OffAttackRange();
    }

    public void OnPanel(Transform structure)
    {
        if (structure.GetComponent<TurretStructure>() != null)
            currentTurret = structure.GetComponent<TurretStructure>();
        else if (structure.GetComponent<BarrierStructure>() != null)
            currentBarrier = structure.GetComponent<BarrierStructure>();

        // Turret Info Panel On
        gameObject.SetActive(true);

        // 터렛이 활성화된 경우
        if (currentTurret != null)
        {
            // 터렛 정보 갱신
            UpdateTurretData();

            // 터렛 공격 범위 표시
            structureAttackRange.OnAttackRange(currentTurret.transform.position, currentTurret.Range);
        }
        else if (currentBarrier != null)
        {
            UpdateBarrierData();
        }
    }

    public void OnPanel(BuildItemSO buildItem)
    {
        // Turret Info Panel On
        gameObject.SetActive(true);

        // 터렛 정보 갱신
        UpdateStructureData(buildItem);
    }

    public void OffPanel()
    {
        // Turret Info Panel Off
        gameObject.SetActive(false);

        if (currentTurret != null)
        {
            // 공격 범위 표시 Off
            structureAttackRange.OffAttackRange();
        }
    }

    private void UpdateTurretData()
    {
        nameText.text = currentTurret.Name;
        damageText.text = "공격력 : " + ((int)currentTurret.Damage).ToString("D2");
        defenceText.text = "방어력 : " + ((int)currentTurret.Defecne).ToString("D2");
        SetNeedCost();

        if (GameManager.Instance.GetTimeInfo() == TimeInfo.night)
        {
            upgradeButton.interactable = false;
            deleteButton.interactable = false;
            return;
        }

        upgradeButton.interactable = currentTurret.Level < currentTurret.MaxLevel ? true : false;
        deleteButton.interactable = true;
    }

    private void UpdateBarrierData()
    {
        nameText.text = currentBarrier.Name;
        damageText.text = "공격력 : " + ((int)currentBarrier.Damage).ToString("D3");
        defenceText.text = "방어력 : " + ((int)currentBarrier.Defecne).ToString("D3");
        SetNeedCost();

        if (GameManager.Instance.GetTimeInfo() == TimeInfo.night)
        {
            upgradeButton.interactable = false;
            deleteButton.interactable = false;
            return;
        }

        upgradeButton.interactable = currentBarrier.Level < currentBarrier.MaxLevel ? true : false;
        deleteButton.interactable = true;
    }

    // 건물 첫 생성 시 데이터 업데이트
    private void UpdateStructureData(BuildItemSO buildItem)
    {
        string numLength = "D2";
        if (buildItem.buildingItem[0].type == "방벽") numLength = "D3";

        nameText.text = buildItem.buildingItem[0].name;
        damageText.text = "공격력 : " + ((int)buildItem.buildingItem[0].damage).ToString(numLength);
        defenceText.text = "방어력 : " + ((int)buildItem.buildingItem[0].defence).ToString(numLength);

        currentInventory = playerInventory.GetCurrentInventoryState();

        int firstItemQuantity = 0;
        int secondItemQuantity = 0;
        int index = 0;

        foreach (KeyValuePair<ItemSO, int> cost in buildItem.buildingItem[0].buildCost)
        {
            int count = 0;

            foreach (KeyValuePair<int, InventoryItem> inventory in currentInventory)
            {
                if (cost.Key == inventory.Value.item)
                {
                    count += inventory.Value.quantity;
                }
            }

            if (index == 0) firstItemQuantity = count;
            else if (index == 1) secondItemQuantity = count;

            index++;
        }

        costItem_1_Img.sprite = buildItem.buildingItem[0].buildCost.ElementAt(0).Key.Icon;
        costItem_1_Text.text = firstItemQuantity.ToString("D2") + " / " + (buildItem.buildingItem[0].buildCost.ElementAt(0).Value).ToString("D2");
        if (firstItemQuantity < buildItem.buildingItem[0].buildCost.ElementAt(0).Value) costItem_1_Text.color = Color.red;
        else costItem_1_Text.color = Color.white;

        costItem_2_Img.sprite = buildItem.buildingItem[0].buildCost.ElementAt(1).Key.Icon;
        costItem_2_Text.text = secondItemQuantity.ToString("D2") + " / " + (buildItem.buildingItem[0].buildCost.ElementAt(1).Value).ToString("D2");
        if (secondItemQuantity < buildItem.buildingItem[0].buildCost.ElementAt(1).Value) costItem_2_Text.color = Color.red;
        else costItem_2_Text.color = Color.white;

        // 포탑 첫 생성 시 강화/삭제 버튼 비활성화
        upgradeButton.interactable = false;
        deleteButton.interactable = false;
    }

    private void SetNeedCost()
    {
        SerializableDictionary<ItemSO, int> costs = new SerializableDictionary<ItemSO, int>();
        currentInventory = playerInventory.GetCurrentInventoryState();

        int firstItemQuantity = 0;
        int secondItemQuantity = 0;
        int index = 0;

        if (currentBarrier != null)
        {
            costs = currentBarrier.CreateCost();
        }
        else if (currentTurret != null)
        {
            costs = currentTurret.CreateCost();
        }

        foreach (KeyValuePair<ItemSO, int> cost in costs)
        {
            int count = 0;

            foreach (KeyValuePair<int, InventoryItem> inventory in currentInventory)
            {
                if (cost.Key == inventory.Value.item)
                {
                    count += inventory.Value.quantity;
                }
            }

            if (index == 0) firstItemQuantity = count;
            else if (index == 1) secondItemQuantity = count;

            index++;
        }

        costItem_1_Img.sprite = costs.ElementAt(0).Key.Icon;
        costItem_1_Text.text = firstItemQuantity.ToString("D2") + " / " + (costs.ElementAt(0).Value).ToString("D2");
        if (firstItemQuantity < costs.ElementAt(0).Value) costItem_1_Text.color = Color.red;
        else costItem_1_Text.color = Color.white;

        costItem_2_Img.sprite = costs.ElementAt(1).Key.Icon;
        costItem_2_Text.text = secondItemQuantity.ToString("D2") + " / " + (costs.ElementAt(1).Value).ToString("D2");
        if (secondItemQuantity < costs.ElementAt(1).Value) costItem_2_Text.color = Color.red;
        else costItem_2_Text.color = Color.white;
    }

    public void OnClickUpgrade()
    {
        if (currentTurret != null)
        {
            bool isSuccess = currentTurret.Upgrade();

            if (isSuccess == true)
            {
                UpdateTurretData();
                structureAttackRange.OnAttackRange(currentTurret.transform.position, currentTurret.Range);
            }
        }
        else if (currentBarrier != null)
        {
            bool isSuccess = currentBarrier.Upgrade();

            if (isSuccess == true)
            {
                UpdateBarrierData();
            }
        }
    }

    public void OnClickDelete()
    {
        if (currentTurret != null)
        {
            currentTurret.TileTransform.GetComponent<BuildingTile>().isStructureBuilding = false;
            Destroy(currentTurret.gameObject);
            structureAttackRange.OffAttackRange();
        }
        else if (currentBarrier != null)
        {
            currentBarrier.TileTransform.GetComponent<BuildingTile>().isStructureBuilding = false;
            Destroy(currentBarrier.gameObject);
        }
    }
}
