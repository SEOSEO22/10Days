using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering; // ���� ������Ʈ ������ ���� �ʿ�
using TMPro;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;

public class Timer : MonoBehaviour
{
    [SerializeField] private float dayMaxTime = 600f;
    [SerializeField] private float nightMaxTime = 200f;
    [SerializeField] private float maxTime = 600f;
    [SerializeField] private Volume ppv; // ����Ʈ ���μ��� ����
    [SerializeField] private TextMeshProUGUI dayCountText;

    private int timeInfo = (int) TimeInfo.Day;
    private int dayCount = 0;
    private Image image;

    public GameObject spotLight;

    private void Start()
    {
        image = transform.Find("Timer Image").GetComponent<Image>();
        image.fillAmount = 0;
        maxTime = dayMaxTime;

        if (!ppv) ppv = GameObject.Find("Volume").GetComponent<Volume>();
        if (!dayCountText) dayCountText = GetComponentInChildren<TextMeshProUGUI>();

        // ���� �Ŵ��� Ȥ�� ������ �Ŵ����� ����� ���� �ϼ� ����� �ҷ��� dayCountText�� ����
        SetDayCount();
    }

    private void FixedUpdate()
    {
        CalcTime();
    }

    private void CalcTime()
    {
        if (image.fillAmount == 1)
        {
            RestartTimer();
            return;
        }

        image.fillAmount += Time.deltaTime / maxTime;
        ControlPPV();
    }

    // Ÿ�̸� ����� �Լ�
    private void RestartTimer()
    {
        timeInfo = ++dayCount % 2 == 0 ? (int)TimeInfo.Day : (int)TimeInfo.night;
        image.fillAmount = 0;

        if (timeInfo == (int)TimeInfo.Day) maxTime = dayMaxTime;
        else maxTime = nightMaxTime;

        SetDayCount();
        GameManager.Instance.SetTimeInfo(timeInfo);
    }

    // ����Ʈ ���μ��� ����(���� ��ȯ)
    private void ControlPPV()
    {
        if (timeInfo == (int)TimeInfo.Day)
        {
            if (image.fillAmount < 0.75f) ppv.weight = 0;
            else
            {
                ppv.weight = (image.fillAmount - 0.75f) / 0.25f;
                SetLightsActive(ppv.weight);
            }
        }
        else
        {
            if (image.fillAmount < 0.75f) ppv.weight = 1;
            else
            {
                ppv.weight = (1f - image.fillAmount) / 0.25f;
                SetLightsActive(ppv.weight);
            }
        }
    }

    // Light Ȱ��ȭ
    private void SetLightsActive(float colorAlphaValue)
    {
        Light2D light2D = spotLight.GetComponent<Light2D>();
        light2D.color = new Color(light2D.color.r, light2D.color.g, light2D.color.b, colorAlphaValue);
    }

    // ���� �ϼ� Ÿ�̸ӿ� ǥ��
    private void SetDayCount()
    {
        dayCountText.text = "DAY " + ((dayCount / 2) + 1).ToString("D2");
    }
}
