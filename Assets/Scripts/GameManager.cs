using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimeInfo { Day, night }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private TimeInfo timeInfo = TimeInfo.Day;

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
    }

    public void SetTimeInfo(int info)
    {
        timeInfo = (TimeInfo)info;
    }

    public TimeInfo GetTimeInfo()
    {
        return timeInfo;
    }
}
