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

        // 터렛 정보 갱신
        UpdateTurretData();

        // 터렛 공격 범위 표시
        turretAttackRange.OnAttackRange(currentTurret.transform.position, currentTurret.Range);
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

        // 건물 배치 시에는 "생성", 배치 후에는 "업그레이드"로 버튼 텍스트 변환
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
            // 재료 부족 메세지
        }
    }
}
