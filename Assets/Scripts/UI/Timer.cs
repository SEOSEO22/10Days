using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering; // 볼륨 컴포넌트 접근을 위해 필요
using TMPro;
using UnityEngine.Rendering.Universal;
using System.Linq;
using Inventory.Model;

public class Timer : MonoBehaviour
{
    [SerializeField] private float dayMaxTime = 400f;
    [SerializeField] private float nightMaxTime = 200f;
    [SerializeField] private float maxTime = 400f;
    [SerializeField] private Volume ppv; // 포스트 프로세싱 볼륨
    [SerializeField] private TextMeshProUGUI dayCountText;
    [SerializeField] private SpawnMachineParts spawnMachineParts;

    [Header("Saving Data")]
    [SerializeField] private TurretSpawner turretSpawner;
    [SerializeField] private BarrierSpawner barrierSpawner;
    [SerializeField] private InventorySO playerInventory;

    private int timeInfo = (int) TimeInfo.Day;
    private int dayCount = 0;
    private Image image;

    [Space]
    public GameObject spotLight;
    public int DayCount => dayCount;

    private void Start()
    {
        image = transform.Find("Timer Image").GetComponent<Image>();
        image.fillAmount = 0;
        maxTime = dayMaxTime;

        if (!ppv) ppv = GameObject.Find("Volume").GetComponent<Volume>();
        if (!dayCountText) dayCountText = GetComponentInChildren<TextMeshProUGUI>();

        dayCount = DataManager.Instance.currentGameData.dayCountData.dayCount;
        timeInfo = (int)DataManager.Instance.currentGameData.dayCountData.timeInfo;
        if (timeInfo == (int)TimeInfo.night) GameManager.Instance.isAllEnemyDead = false;
        SetDayCount();

        if (timeInfo == (int)TimeInfo.Day)
            SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_DAY);
        else if (timeInfo == (int)TimeInfo.night)
            SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_NIGHT);
    }

    private void FixedUpdate()
    {
        CalcTime();
    }

    private void CalcTime()
    {
        if (image.fillAmount == 1 && GameManager.Instance.isAllEnemyDead)
        {
            RestartTimer();
            return;
        }

        if (GameManager.Instance.GetTimeInfo() == TimeInfo.night && GameManager.Instance.isAllEnemyDead)
        {
            RestartTimer();
            SetLightsActive(0);
            return;
        }

        image.fillAmount += Time.deltaTime / maxTime;
        ControlPPV();
    }

    // 타이머 재시작 함수
    private void RestartTimer()
    {
        timeInfo = ++dayCount % 2 == 0 ? (int)TimeInfo.Day : (int)TimeInfo.night;
        DataManager.Instance.currentGameData.dayCountData.SetTimeData(dayCount);
        DataManager.Instance.currentGameData.turretsData.SetTurretData(turretSpawner.GetComponentsInChildren<TurretStructure>().ToList());
        DataManager.Instance.currentGameData.barriersData.SetBarrierData(barrierSpawner.GetComponentsInChildren<BarrierStructure>().ToList());
        DataManager.Instance.currentGameData.inventoryData.SetPlayerInventoryData(playerInventory.GetCurrentInventoryState());
        image.fillAmount = 0;

        if (timeInfo == (int)TimeInfo.Day)
        {
            maxTime = dayMaxTime;
            GameObject.FindWithTag("Player").GetComponent<PlayerState>().SetHPFull();
            GameManager.Instance.isAllEnemyDead = true;
            SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_DAY);

            if (5 < ((dayCount / 2) + 1) && ((dayCount / 2) + 1) <= 10)
            {
                spawnMachineParts.SpawnPart();
            }
        }
        else
        {
            maxTime = nightMaxTime;
            GameManager.Instance.isAllEnemyDead = false;
            SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_NIGHT);

            if (5 < ((dayCount / 2) + 1) && ((dayCount / 2) + 1) <= 10)
            {
                spawnMachineParts.DestroyPart();
            }
        }

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
