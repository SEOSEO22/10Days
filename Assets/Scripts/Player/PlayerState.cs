using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    [SerializeField] float maxHealth = 1000f;
    [SerializeField] float maxHunger = 100f;

    private Slider healthGauge;
    private Slider hungerGauge;

    private void Start()
    {
        #region 슬라이더 초기화
        if (!healthGauge)
        {
            healthGauge = GameObject.FindWithTag("Health Gauge").GetComponent<Slider>();
        }

        if (!hungerGauge)
        {
            hungerGauge = GameObject.FindWithTag("Hunger Gauge").GetComponent<Slider>();
        }
        #endregion
    }

    public float GetCurrentHealth()
    {
        return healthGauge.value;
    }

    public float GetCurrentHunger()
    {
        return hungerGauge.value;
    }

    public void IncreaseHealthStat(float increaseNum)
    {
        healthGauge.value += (increaseNum / maxHealth);
    }

    public void DecreaseHealthStat(float decreaseNum)
    {
        healthGauge.value -= (decreaseNum / maxHealth);

        /* 
         * if (healthGauge.value <= 0) {
         *      Die();
         * }
         */
    }

    public void IncreaseHungerStat(float increaseNum)
    {
        hungerGauge.value += (increaseNum / maxHunger);
    }

    public void DecreaseHungerStat(float decreaseNum)
    {
        hungerGauge.value -= (decreaseNum / maxHunger);

        /* 
         * if (hungerGauge.value <= 0) {
         *      DecreaseHealthStat(???);
         * }
         */
    }
}
