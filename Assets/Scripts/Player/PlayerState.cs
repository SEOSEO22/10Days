using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    [SerializeField] float maxHealth = 1000f;
    [SerializeField] float maxHunger = 100f;

    private Image healthGauge;
    private Image hungerGauge;

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
        GameManager.Instance.SetGaugeText();
    }

    public float GetCurrentHealth()
    {
        return healthGauge.fillAmount;
    }

    public float GetCurrentHunger()
    {
        return hungerGauge.fillAmount;
    }

    public void SetHPFull()
    {
        healthGauge.fillAmount = 1;
    }

    public void SetHungerFull()
    {
        hungerGauge.fillAmount = 1;
    }

    public void IncreaseHealthStat(float increaseNum)
    {
        healthGauge.fillAmount += (increaseNum / maxHealth);
        GameManager.Instance.SetGaugeText();
    }

    public void DecreaseHealthStat(float decreaseNum)
    {
        healthGauge.fillAmount -= (decreaseNum / maxHealth);
        GameManager.Instance.SetGaugeText();

        /* 
         * if (healthGauge.value <= 0) {
         *      Die();
         * }
         */
    }

    public void IncreaseHungerStat(float increaseNum)
    {
        hungerGauge.fillAmount += (increaseNum / maxHunger);
        GameManager.Instance.SetGaugeText();
    }

    public void DecreaseHungerStat(float decreaseNum)
    {
        if (hungerGauge.fillAmount <= 0.019f)
        {
            DecreaseHealthStat(decreaseNum * (maxHealth / maxHunger));
            return;
        }

        hungerGauge.fillAmount -= (decreaseNum / maxHunger);
        GameManager.Instance.SetGaugeText();
    }
}
