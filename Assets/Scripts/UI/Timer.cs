using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering; // 볼륨 컴포넌트 접근을 위해 필요
using TMPro;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;

public class Timer : MonoBehaviour
{
    [SerializeField] private float dayMaxTime = 600f;
    [SerializeField] private float nightMaxTime = 200f;
    [SerializeField] private float maxTime = 600f;
    [SerializeField] private Volume ppv; // 포스트 프로세싱 볼륨
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

        // 게임 매니저 혹은 데이터 매니저에 저장된 생존 일수 기록을 불러와 dayCountText에 적용
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

    // 타이머 재시작 함수
    private void RestartTimer()
    {
        timeInfo = ++dayCount % 2 == 0 ? (int)TimeInfo.Day : (int)TimeInfo.night;
        image.fillAmount = 0;

        if (timeInfo == (int)TimeInfo.Day) maxTime = dayMaxTime;
        else maxTime = nightMaxTime;

        SetDayCount();
        GameManager.Instance.SetTimeInfo(timeInfo);
    }

    // 포스트 프로세싱 적용(낮밤 변환)
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

    // Light 활성화
    private void SetLightsActive(float colorAlphaValue)
    {
        Light2D light2D = spotLight.GetComponent<Light2D>();
        light2D.color = new Color(light2D.color.r, light2D.color.g, light2D.color.b, colorAlphaValue);
    }

    // 생존 일수 타이머에 표기
    private void SetDayCount()
    {
        dayCountText.text = "DAY " + ((dayCount / 2) + 1).ToString("D2");
    }
}
