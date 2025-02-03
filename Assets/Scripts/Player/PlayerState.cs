using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    [SerializeField] Image imageScreen;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float maxHunger = 100f;
    [SerializeField] private float invincibleTime = 1.5f;
    [SerializeField] private Image healthGauge;
    [SerializeField] private Image hungerGauge;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI hungerText;

    private bool isDamaging = false;
    private float currentHP;
    private float currentHunger;

    public float MaxHP => maxHealth;
    public float CurrentHP => currentHP;
    public float MaxHunger => maxHunger;
    public float CurrentHunger => currentHunger;

    private void Start()
    {
        #region 게이지 초기화
        if (!healthGauge)
        {
            healthGauge = GameObject.FindWithTag("Health Gauge").GetComponent<Image>();
        }

        if (!hungerGauge)
        {
            hungerGauge = GameObject.FindWithTag("Hunger Gauge").GetComponent<Image>();
        }
        #endregion

        SetHPFull();
        SetHungerFull();

        if (DataManager.Instance.IsSaveFileExist())
        {
            currentHP = DataManager.Instance.currentGameData.playerStatData.currentHP;
            currentHunger = DataManager.Instance.currentGameData.playerStatData.currentHunger;

            healthGauge.fillAmount = currentHP;
            hungerGauge.fillAmount = currentHunger;
        }
        else
        {
            currentHP = maxHealth;
            currentHunger = maxHunger;
        }

        SetGaugeText();
    }

    public void SetGaugeText()
    {
        hpText.text = ((int)(healthGauge.fillAmount * 100)).ToString("D3") + " / 100";
        hungerText.text = ((int)(hungerGauge.fillAmount * 100)).ToString("D3") + " / 100";

        DataManager.Instance.currentGameData.playerStatData.SetPlayerData(healthGauge.fillAmount, hungerGauge.fillAmount);
    }

    public void SetHPFull()
    {
        healthGauge.fillAmount = 1;
        currentHP = healthGauge.fillAmount;
    }

    public void SetHungerFull()
    {
        hungerGauge.fillAmount = 1;
        currentHunger = hungerGauge.fillAmount;
    }

    public void IncreaseHealthStat(float increaseNum)
    {
        healthGauge.fillAmount += (increaseNum / maxHealth);
        currentHP = healthGauge.fillAmount;
        SetGaugeText();
    }

    public void DecreaseHealthStat(float decreaseNum)
    {
        if (isDamaging) return;

        StartCoroutine(HandleDamage(decreaseNum));
        currentHP = healthGauge.fillAmount;
    }

    public void IncreaseHungerStat(float increaseNum)
    {
        hungerGauge.fillAmount += (increaseNum / maxHunger);
        currentHunger = hungerGauge.fillAmount;
        SetGaugeText();
    }

    public void DecreaseHungerStat(float decreaseNum)
    {
        if (hungerGauge.fillAmount <= 0.019f)
        {
            DecreaseHealthStat(1f);
            return;
        }

        hungerGauge.fillAmount -= (decreaseNum / maxHunger);
        currentHunger = hungerGauge.fillAmount;
        SetGaugeText();
    }

    private IEnumerator HandleDamage(float decreaseNum)
    {
        isDamaging = true;

        // 체력 감소
        healthGauge.fillAmount -= (decreaseNum / maxHealth);
        SetGaugeText();

        // 피해 시 화면에 붉은 배경 페이드인/아웃
        Color color = imageScreen.color;
        color.a = 0.4f;
        imageScreen.color = color;

        while ( color.a >= 0.0f)
        {
            color.a -= Time.deltaTime;
            imageScreen.color = color;

            yield return null;
        }

        // 무적 시간 동안 대기
        yield return new WaitForSeconds(invincibleTime);

        isDamaging = false;

        // 체력 0 이하 시 처리
        if (healthGauge.fillAmount <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        SceneManager.LoadScene("PlayerDeadEnding");
    }
}
