using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretDataViewer : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI defenceText;
    [SerializeField] private TurretAttackRange turretAttackRange;

    [Header("Buttons")]
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI upgradeButtonText;

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

        // �ǹ� ��ġ �ÿ��� "����", ��ġ �Ŀ��� "���׷��̵�"�� ��ư �ؽ�Ʈ ��ȯ
        upgradeButton.interactable = currentTurret.Level < currentTurret.MaxLevel ? true : false;
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
}
