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

    private TurretStructure currentTurret;
    private BarrierStructure currentBarrier;

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

        // �ͷ��� Ȱ��ȭ�� ���
        if (currentTurret != null)
        {
            // �ͷ� ���� ����
            UpdateTurretData();

            // �ͷ� ���� ���� ǥ��
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

        // �ͷ� ���� ����
        UpdateStructureData(buildItem);
    }

    public void OffPanel()
    {
        // Turret Info Panel Off
        gameObject.SetActive(false);

        if (currentTurret != null)
        {
            // ���� ���� ǥ�� Off
            structureAttackRange.OffAttackRange();
        }
    }

    private void UpdateTurretData()
    {
        nameText.text = currentTurret.Name;
        damageText.text = "���ݷ� : " + ((int)currentTurret.Damage).ToString("D2");
        defenceText.text = "���� : " + ((int)currentTurret.Defecne).ToString("D2");
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
        damageText.text = "���ݷ� : " + ((int)currentBarrier.Damage).ToString("D3");
        defenceText.text = "���� : " + ((int)currentBarrier.Defecne).ToString("D3");
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

    // �ǹ� ù ���� �� ������ ������Ʈ
    private void UpdateStructureData(BuildItemSO buildItem)
    {
        string numLength = "D2";
        if (buildItem.buildingItem[0].type == "�溮") numLength = "D3";

        nameText.text = buildItem.buildingItem[0].name;
        damageText.text = "���ݷ� : " + ((int)buildItem.buildingItem[0].damage).ToString(numLength);
        defenceText.text = "���� : " + ((int)buildItem.buildingItem[0].defence).ToString(numLength);

        costItem_1_Img.sprite = buildItem.buildingItem[0].buildCost.ElementAt(0).Key.Icon;
        costItem_1_Text.text = (buildItem.buildingItem[0].buildCost.ElementAt(0).Value).ToString("D2") + "�� �ʿ�";
        costItem_2_Img.sprite = buildItem.buildingItem[0].buildCost.ElementAt(1).Key.Icon;
        costItem_2_Text.text = (buildItem.buildingItem[0].buildCost.ElementAt(1).Value).ToString("D2") + "�� �ʿ�";

        // ��ž ù ���� �� ��ȭ/���� ��ư ��Ȱ��ȭ
        upgradeButton.interactable = false;
        deleteButton.interactable = false;
    }

    private void SetNeedCost()
    {
        SerializableDictionary<ItemSO, int> costs = new SerializableDictionary<ItemSO, int>();

        if (currentBarrier != null)
        {
            costs = currentBarrier.CreateCost();
        }
        else if (currentTurret != null)
        {
            costs = currentTurret.CreateCost();
        }

        costItem_1_Img.sprite = costs.ElementAt(0).Key.Icon;
        costItem_1_Text.text = (costs.ElementAt(0).Value).ToString("D2") + "�� �ʿ�";
        costItem_2_Img.sprite = costs.ElementAt(1).Key.Icon;
        costItem_2_Text.text = (costs.ElementAt(1).Value).ToString("D2") + "�� �ʿ�";
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
            else
            {
                // ��� ���� �޼���
            }
        }
        else if (currentBarrier != null)
        {
            bool isSuccess = currentBarrier.Upgrade();

            if (isSuccess == true)
            {
                UpdateBarrierData();
            }
            else
            {
                // ��� ���� �޼���
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
