using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    [SerializeField] Image imageScreen;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float maxHunger = 100f;
    [SerializeField] private float invincibleTime = 1.5f;

    private Image healthGauge;
    private Image hungerGauge;
    private bool isDamaging = false;
    private float currentHP;
    private float currentHunger;

    public float MaxHP => maxHealth;
    public float CurrentHP => currentHP;
    public float MaxHunger => maxHunger;
    public float CurrentHunger => currentHunger;

    private void Start()
    {
        #region ������ �ʱ�ȭ
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

        GameManager.Instance.SetGaugeText();
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
        GameManager.Instance.SetGaugeText();
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
        currentHunger = hungerGauge.fillAmount;
        GameManager.Instance.SetGaugeText();
    }

    private IEnumerator HandleDamage(float decreaseNum)
    {
        isDamaging = true;

        // ü�� ����
        healthGauge.fillAmount -= (decreaseNum / maxHealth);
        GameManager.Instance.SetGaugeText();

        // ���� �� ȭ�鿡 ���� ��� ���̵���/�ƿ�
        Color color = imageScreen.color;
        color.a = 0.4f;
        imageScreen.color = color;

        while ( color.a >= 0.0f)
        {
            color.a -= Time.deltaTime;
            imageScreen.color = color;

            yield return null;
        }

        // ���� �ð� ���� ���
        yield return new WaitForSeconds(invincibleTime);

        isDamaging = false;

        // ü�� 0 ���� �� ó��
        if (healthGauge.fillAmount <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died.");
    }
}
