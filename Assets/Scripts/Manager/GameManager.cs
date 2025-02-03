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

        if (DataManager.Instance != null)
            timeInfo = DataManager.Instance.currentGameData.dayCountData.timeInfo;
    }

    public void SetTimeInfo(int info)
    {
        timeInfo = (TimeInfo)info;
        DataManager.Instance.SaveData();

        LoadEscapeEnding();
    }

    public TimeInfo GetTimeInfo()
    {
        return timeInfo;
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
