using Inventory;
using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    // 플레이어 스탯 정보
    public float currentHP;
    public float currentHunger;

    public void SetPlayerData(float currentHP, float currentHunger)
    {
        this.currentHP = currentHP;
        this.currentHunger = currentHunger;
    }
}

[System.Serializable]
public class PlayerInventoryData
{
    // 인벤토리 정보
    public List<InventoryItemWrapper> inventory = new List<InventoryItemWrapper>();

    public void SetPlayerInventoryData(Dictionary<int, InventoryItem> currentInventory)
    {
        inventory.Clear();

        foreach (var item in currentInventory)
        {
            inventory.Add(new InventoryItemWrapper(item.Key, item.Value));
        }
    }

    public Dictionary<int, InventoryItem> GetInventoryData()
    {
        Dictionary<int, InventoryItem> inventoryDict = new Dictionary<int, InventoryItem>();

        foreach (var item in inventory)
        {
            inventoryDict[item.id] = item.item;
        }
        return inventoryDict;
    }
}

[System.Serializable]
public class InventoryItemWrapper
{
    public int id;
    public InventoryItem item;

    public InventoryItemWrapper(int id, InventoryItem item)
    {
        this.id = id;
        this.item = item;
    }
}

[System.Serializable]
public class TurretData
{
    // 건설 오브젝트 정보
    public List<TurretStructure> turrets;

    public void SetTurretData(List<TurretStructure> currentTurrets)
    {
        turrets = new List<TurretStructure>(currentTurrets);
    }
}

[System.Serializable]
public class BarrierData
{
    // 건설 오브젝트 정보
    public List<BarrierStructure> barriers;

    public void SetTurretData(List<BarrierStructure> currentBarriers)
    {
        barriers = new List<BarrierStructure>(currentBarriers);
    }
}

[System.Serializable]
public class TimeData
{
    // 날짜 정보
    public int dayCount;
    public TimeInfo timeInfo;

    public void SetTimeData(int dayCount)
    {
        this.dayCount = dayCount;
        timeInfo = dayCount % 2 == 0 ? TimeInfo.Day : TimeInfo.night;
    }
}

[System.Serializable]
public class GameData
{
    public PlayerData playerStat = new PlayerData();
    public PlayerInventoryData inventory = new PlayerInventoryData();
    public TurretData turrets = new TurretData();
    public BarrierData barriers = new BarrierData();
    public TimeData dayCount = new TimeData();
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public GameData currentGameData = new GameData();
    private string path;

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion

        path = Application.persistentDataPath + "/saveData";
    }

    private void Start()
    {

    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(currentGameData);
        File.WriteAllText(path, data);
    }

    public void LoadData()
    {
        string data = File.ReadAllText(path);
        currentGameData = JsonUtility.FromJson<GameData>(data);
    }

    public void DataClear()
    {
        currentGameData = new GameData();
    }
}
