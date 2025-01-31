using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum TimeInfo { Day, night }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private TimeInfo timeInfo = TimeInfo.Day;

    // 체력 / 허기 게이지 UI
    private Image healthGauge;
    private Image hungerGauge;
    private TextMeshProUGUI hpText;
    private TextMeshProUGUI hungerText;

    public bool isAllEnemyDead { get; set; } = true;    // 적 오브젝트 존재 여부
    public bool isStructureSelected { get; set; } = false;  // 건축물 선택 여부
    public bool isMachinePartsEnough { get; set; } = false; // 기계 부품 개수가 탈출하기에 충분한지 확인

    private void Awake()
    {
        #region Singleton
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion

        healthGauge = GameObject.FindWithTag("Health Gauge").GetComponent<Image>();
        hungerGauge = GameObject.FindWithTag("Hunger Gauge").GetComponent<Image>();
        hpText = healthGauge.transform.parent.GetComponentInChildren<TextMeshProUGUI>();
        hungerText = hungerGauge.transform.parent.GetComponentInChildren<TextMeshProUGUI>();

        timeInfo = DataManager.Instance.currentGameData.dayCountData.timeInfo;
    }

    public void SetGaugeText()
    {
        hpText.text = ((int)(healthGauge.fillAmount * 100)).ToString("D3") + " / 100";
        hungerText.text = ((int)(hungerGauge.fillAmount * 100)).ToString("D3") + " / 100";
    }

    public void SetTimeInfo(int info)
    {
        timeInfo = (TimeInfo)info;

        SavePlayerStatData();
        DataManager.Instance.SaveData();

        LoadEscapeEnding();
    }

    public TimeInfo GetTimeInfo()
    {
        return timeInfo;
    }

    private void SavePlayerStatData()
    {
        DataManager.Instance.currentGameData.playerStatData.SetPlayerData(healthGauge.fillAmount, hungerGauge.fillAmount);
    }

    private void LoadEscapeEnding()
    {
        if (DataManager.Instance.currentGameData.dayCountData.dayCount >= 20)
        {
            if (isMachinePartsEnough)
                SceneManager.LoadScene("EscapeEnding");
            else
                SceneManager.LoadScene("PlayerDeadEnding");
        }
    }
}
