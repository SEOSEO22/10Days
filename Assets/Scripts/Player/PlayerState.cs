using System.Collections;
using System.Collections.Generic;
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
        #region 슬라이더 초기화
        if (!healthGauge)
        {
            healthGauge = GameObject.FindWithTag("Health Gauge").GetComponent<Image>();
        }

        if (!hungerGauge)
        {
            hungerGauge = GameObject.FindWithTag("Hunger Gauge").GetComponent<Image>();
        }
        #endregion
    }

    public float GetCurrentHealth()
    {
        return healthGauge.fillAmount;
    }

    public float GetCurrentHunger()
    {
        return hungerGauge.fillAmount;
    }

    public void IncreaseHealthStat(float increaseNum)
    {
        healthGauge.fillAmount += (increaseNum / maxHealth);
    }

    public void DecreaseHealthStat(float decreaseNum)
    {
        healthGauge.fillAmount -= (decreaseNum / maxHealth);

        /* 
         * if (healthGauge.value <= 0) {
         *      Die();
         * }
         */
    }

    public void IncreaseHungerStat(float increaseNum)
    {
        hungerGauge.fillAmount += (increaseNum / maxHunger);
    }

    public void DecreaseHungerStat(float decreaseNum)
    {
        hungerGauge.fillAmount -= (decreaseNum / maxHunger);

        /* 
         * if (hungerGauge.value <= 0) {
         *      DecreaseHealthStat(???);
         * }
         */
    }
}
