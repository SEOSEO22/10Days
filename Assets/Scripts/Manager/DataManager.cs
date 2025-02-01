using Inventory;
using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Security.Cryptography;
using System.Text;
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
public class TurretsData
{
    // 건설 오브젝트 정보
    public List<TurretInfoData> turrets = new List<TurretInfoData>();

    public void SetTurretData(List<TurretStructure> currentTurrets)
    {
        turrets.Clear();

        foreach (var turret in currentTurrets)
        {
            turrets.Add(new TurretInfoData(turret.Level - 1, turret.TileTransform.position));
        }
    }
}

[System.Serializable]
public class TurretInfoData
{
    public int level;
    public Vector3 tileTransform;

    public TurretInfoData(int level, Vector3 tileTransform)
    {
        this.level = level;
        this.tileTransform = tileTransform;
    }
}

[System.Serializable]
public class BarriersData
{
    // 건설 오브젝트 정보
    public List<BarrierInfoData> barriers = new List<BarrierInfoData>();

    public void SetBarrierData(List<BarrierStructure> currentBarriers)
    {
        barriers.Clear();

        foreach (var barrier in currentBarriers)
        {
            barriers.Add(new BarrierInfoData(barrier.Level - 1, barrier.TileTransform.position));
        }
    }
}

[System.Serializable]
public class BarrierInfoData
{
    public int level;
    public Vector3 tileTransform;

    public BarrierInfoData(int level, Vector3 tileTransform)
    {
        this.level = level;
        this.tileTransform = tileTransform;
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
    public PlayerData playerStatData = new PlayerData();
    public PlayerInventoryData inventoryData = new PlayerInventoryData();
    public TurretsData turretsData = new TurretsData();
    public BarriersData barriersData = new BarriersData();
    public TimeData dayCountData = new TimeData();
}

[System.Serializable]
public class SoundData
{
    public float bgmVolume = 0.1f;
    public float sfxVolume = 0.2f;
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public GameData currentGameData = new GameData();
    public SoundData currentSoundData = new SoundData();
    private string path;
    private string soundDataPath;

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
        soundDataPath = Application.persistentDataPath + "/soundData";
    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(currentGameData);
        string encryptedData = Encrypt(data);
        File.WriteAllText(path, encryptedData);
    }

    public void SaveSoundData()
    {
        string data = JsonUtility.ToJson(currentSoundData);
        File.WriteAllText(soundDataPath, data);
    }

    public void LoadData()
    {
        if (File.Exists(path))
        {
            string data = File.ReadAllText(path);
            string decryptData = Decrypt(data);
            currentGameData = JsonUtility.FromJson<GameData>(decryptData);
        }
    }

    public void LoadSoundData()
    {
        if (File.Exists(soundDataPath))
        {
            string data = File.ReadAllText(soundDataPath);
            currentSoundData = JsonUtility.FromJson<SoundData>(data);
        }
    }

    public bool IsSaveFileExist()
    {
        return File.Exists(path);
    }

    public bool IsSoundFileExist()
    {
        return File.Exists(soundDataPath);
    }

    public void DataClear()
    {
        currentGameData = new GameData();

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    // AES 암호화 키 및 초기화 벡터(IV)를 설정
    private static readonly string encryptionKey = "1234567890123456"; // 16 characters, 128-bit key
    private static readonly string iv = "6543210987654321"; // 16 characters, 128-bit IV

    private string Encrypt(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.IV = Encoding.UTF8.GetBytes(iv);

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            return Convert.ToBase64String(encryptedBytes);
        }
    }

    private string Decrypt(string cipherText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.IV = Encoding.UTF8.GetBytes(iv);

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
