using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private float maxTime = 600f;

    private int timeInfo = (int) TimeInfo.Day;
    private int dayCount = 0;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        image.fillAmount = 0;
    }

    private void Update()
    {
        SetTimer();
    }

    // Ÿ�̸� ���� �Լ�
    private void SetTimer()
    {
        if (image.fillAmount == 1)
        {
            RestartTimer();
            return;
        }

        image.fillAmount += Time.deltaTime / maxTime;
    }

    // Ÿ�̸� ����� �Լ�
    private void RestartTimer()
    {
        timeInfo = ++dayCount % 2 == 0 ? (int)TimeInfo.Day : (int)TimeInfo.night;
        image.fillAmount = 0;

        GameManager.Instance.SetTimeInfo(timeInfo);
    }
}
